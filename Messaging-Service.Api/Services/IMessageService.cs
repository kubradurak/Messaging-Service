using MessagingService.Base.Dtos;
using MessagingService.Entities.Dtos;
using MessagingService.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Services
{
    public interface IMessageService
    {
        Task<ResponseDto<Message>> SendMessageAsync(SendMessageDto sendMessage);
        Task<ResponseDto<List<MessageHistoryDto>>> GetMessageHistoryAsync(MessageHistoryItemDto viewer);
        Task<List<MessageHistoryDto>> GetMessageHistoryList(MessageHistoryItemDto viewer);

    }
}
