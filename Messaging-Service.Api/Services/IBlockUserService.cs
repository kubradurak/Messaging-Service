using MessagingService.Base.Dtos;
using MessagingService.Entities.Dtos;
using MessagingService.Entities.Models;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Services
{
    public interface IBlockUserService
    {
        Task<ResponseDto<BlockUser>> BlockUserAsync(BlockUserDto user);
        Task<ResponseDto<BlockUser>> RemoveBlockUserAsync(BlockUserDto user);
        bool IsBlockedUser(BlockUserDto user);
        BlockUser CheckBlockedUser(BlockUser user);

    }
}
