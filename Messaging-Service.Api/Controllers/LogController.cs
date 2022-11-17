using Messaging_Service.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog.Fluent;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Controllers
{
    /// <summary>
    /// Log Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IUserActivityLogService _userActivityLogService;

        /// ctor
        public LogController(IUserActivityLogService userActivityLogService)
        {
            _userActivityLogService = userActivityLogService;
        }
        /// <summary>
        /// User Activity Logs
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET Log/UserActivityLogs
        /// </remarks>
        /// <param name="email"></param>
        /// <returns>Last three acctivities from user </returns>
        [HttpGet]
        [Route("UserActivityLogs")]
        public async Task<IActionResult> GetUserActivityLogsAsync(string email)
        {
            if (email == null || string.IsNullOrEmpty(email))
            {
                string errorMessage = string.Format("LoginAsync: Please fill in the required fields! Parametres: Email:{0}", email);
                Log.Error(errorMessage);
                return BadRequest("Please fill in the required fields!");
            }

            var response = await _userActivityLogService.GetUserActivityLogsAsync(email);
            return Ok(response);
        }
    }
}
