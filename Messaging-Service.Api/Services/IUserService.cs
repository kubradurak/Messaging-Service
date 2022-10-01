using MessagingService.Base.Dtos;
using MessagingService.Entities.Dtos;
using MessagingService.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<ResponseDto<User>> RegisterAsync(UserDto user);
        Task<ResponseDto<User>> LoginAsync(UserLoginDto user);
        bool CheckByUserName(string userName);
        bool CheckLoginByUsernameAndPassword(string userName, string password);
        bool CheckByEmail(string email);
    }
}
