using AutoMapper;
using MessagingService.Entities.Dtos;
using MessagingService.Entities.Models;

namespace Messaging_Service.Api.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserLoginDto, User>().ReverseMap();
            CreateMap<SendMessageDto, Message>().ReverseMap();
            CreateMap<MessageDto, Message>().ReverseMap();
            CreateMap<MessageHistoryItemDto, Message>().ReverseMap();
            CreateMap<MessageHistoryDto, Message>().ReverseMap();
            CreateMap<BlockUserDto, BlockUser>().ReverseMap();
        }
    }
}
