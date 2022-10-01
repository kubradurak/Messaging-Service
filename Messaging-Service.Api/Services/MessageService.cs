using AutoMapper;
using Messaging_Service.Api.Settings;
using MessagingService.Base.Dtos;
using MessagingService.Entities.Dtos;
using MessagingService.Entities.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using NLog.Fluent;

namespace Messaging_Service.Api.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMongoCollection<Message> _messageCollection;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public MessageService(IDatabaseSettings databaseSettings, IUserService userService, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _messageCollection = database.GetCollection<Message>(databaseSettings.MessageCollections);
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<ResponseDto<List<MessageHistoryDto>>> GetMessageHistoryAsync(MessageHistoryItemDto messageHistoryItem)
        {
            var _sendMessage = _mapper.Map<Message>(messageHistoryItem);

            var toUser = _userService.CheckByUserName(messageHistoryItem.To);
            if (!toUser) return ResponseDto<List<MessageHistoryDto>>.Fail("bulunamadı", 404);

            var messageList = await GetMessageHistoryList(messageHistoryItem);
            if (messageList.Any())
            {
                Log.Info("Mesaj geçmişi bulunamadı!");
            }

            return ResponseDto<List<MessageHistoryDto>>.Success(messageList, 200);
        }

        public async Task<List<MessageHistoryDto>> GetMessageHistoryList(MessageHistoryItemDto messageHistoryItem)
        {
            var _sendMessage = _mapper.Map<Message>(messageHistoryItem);

            var messageList = _messageCollection.Find(x => (x.To == _sendMessage.To && x.SenderUserName == _sendMessage.SenderUserName) ||
                (x.SenderUserName == messageHistoryItem.To && x.To == _sendMessage.SenderUserName)).ToList().OrderByDescending(d => d.CreateDate);
            var list = messageList.Skip((messageHistoryItem.PageIndex - 1) * messageHistoryItem.PageSize)
                .Take(messageHistoryItem.PageSize).ToList();
            return list.Select(x => new MessageHistoryDto
            {
                MessageHistoryList = list
            }).ToList();
        }
        public async Task<ResponseDto<Message>> SendMessageAsync(SendMessageDto sendMessage)
        {
            var _sendMessage = _mapper.Map<Message>(sendMessage);

            var receiverUser = _userService.CheckByUserName(sendMessage.To);
            if (!receiverUser) return ResponseDto<Message>.Fail("Böyle bir kullanıcı adı yok! Göndereceğiniz kullanıcı adını kontrol ediniz!", 400);

            await _messageCollection.InsertOneAsync(new Message
            {
                SenderUserName = sendMessage.SenderUserName,
                Content = sendMessage.Content,
                To = sendMessage.To,
                CreateDate = DateTime.Now

            });

            return ResponseDto<Message>.Success(_sendMessage, 200);
        }
    }
}
