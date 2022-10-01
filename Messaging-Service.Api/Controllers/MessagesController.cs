using Messaging_Service.Api.Services;
using MessagingService.Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog.Fluent;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SendMail")]
        public async Task<IActionResult> SendMessageAsync(SendMessageDto sendMessage)
        {
            if (sendMessage == null || string.IsNullOrWhiteSpace(sendMessage.To) || string.IsNullOrWhiteSpace(sendMessage.Content))
            {
                string errorMessage = string.Format("User parametres is not valid: userName:{0}, password:{1}", sendMessage.Content, sendMessage.To);
                Log.Error(errorMessage);
                return BadRequest("Lütfen zorunlu alanları doldurunuz.");
            }

            // parameters.SenderUserId = int.Parse(User.FindFirst("UserId")?.Value); OTURUMDAN KULLANICININ ID DEĞERİNİ GETİRİYOR.
            var response = await _messageService.SendMessageAsync(sendMessage);
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("GetMessageHistory/{to}/{pageIndex:int}/{pageSize:int}")]
        public async Task<IActionResult> GetMessageHistoryAsync([FromRoute] string to, [FromRoute] int pageIndex, [FromRoute] int pageSize)
        {
            if (pageIndex < 1 || pageSize < 1 || string.IsNullOrWhiteSpace(to))
            {
                string errorMessage = string.Format("User parametres is not valid: userName:{0}, password:{1}", to);
                Log.Error(errorMessage);
                return BadRequest("Lütfen zorunlu alanları doldurunuz.");
            }
            var items = new MessageHistoryItemDto
            {
                SenderUserName = "veli",
                To = to,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var response = await _messageService.GetMessageHistoryAsync(items);
            return Ok(response);
        }

    }
}
