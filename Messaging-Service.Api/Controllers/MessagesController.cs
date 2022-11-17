using AutoMapper;
using Messaging_Service.Api.Services;
using MessagingService.Entities.Dtos;
using MessagingService.Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog.Fluent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Controllers
{
    /// <summary>
    /// Messages Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        ///ctor
        public MessagesController(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }
        /// <summary>
        /// Send message to user by username.
        /// </summary>
        /// ///  Sample request:
        ///     POST Messages/SendMail
        ///     {
        ///         "To" : "kub",
        ///         "Content": "selam"
        ///      }
        /// <param name="sendMessage"></param>
        /// <returns>Send message response</returns>
        [HttpPost]
        [Route("SendMail")]
        public async Task<IActionResult> SendMessageAsync(SendMessageDto sendMessage)
        {
            if (sendMessage == null || string.IsNullOrWhiteSpace(sendMessage.To) || string.IsNullOrWhiteSpace(sendMessage.Content))
            {
                string errorMessage = string.Format("SendMessageAsync: Parametres: Content:{0}, To:{1}", sendMessage.Content, sendMessage.To);
                Log.Error(errorMessage);
                return BadRequest("Please fill in the required fields!");
            }
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var message = _mapper.Map<Message>(sendMessage);
            message.SenderUserName = identity?.FindFirst("unique_name")?.Value;
            var response = await _messageService.SendMessageAsync(message);
            return Ok(response);
        }

        /// <summary>
        /// Get message history between two users with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET Messages/GetMessageHistory/kub/1/10
        /// </remarks>
        /// <param name="to">To user name</param>
        /// <param name="pageIndex">Page index for pagination</param>
        /// <param name="pageSize">Page size for pagination</param>
        /// <returns>Message history between two users</returns>
        [HttpGet]
        [Route("GetMessageHistory/{to}/{pageIndex:int}/{pageSize:int}")]
        public async Task<IActionResult> GetMessageHistoryAsync([FromRoute] string to, [FromRoute] int pageIndex, [FromRoute] int pageSize)
        {
            if (pageIndex < 1 || pageSize < 1 || string.IsNullOrWhiteSpace(to))
            {
                string errorMessage = string.Format("GetMessageHistoryAsync: Parametres: to:{0}", to);
                Log.Error(errorMessage);
                return BadRequest("Please fill in the required fields!");
            }
            var items = new MessageHistoryItemDto
            {
                To = to,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var response = await _messageService.GetMessageHistoryAsync(items);
            return Ok(response);
        }

    }
}
