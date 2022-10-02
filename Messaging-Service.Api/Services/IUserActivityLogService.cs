using MessagingService.Base.Dtos;
using MessagingService.Entities.Dtos;
using MessagingService.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Services
{
    public interface IUserActivityLogService
    {
        Task<ResponseDto<UserActivityLog>> AddLogForUser(UserActivityLogDto activityLog);
        Task<ResponseDto<List<UserActivityLog>>> GetUserActivityLogsAsync(string email);
        Task<ResponseDto<UserActivityLog>> DeleteLastLogByEmail(UserActivityLogDto activityLog);

    }
}
