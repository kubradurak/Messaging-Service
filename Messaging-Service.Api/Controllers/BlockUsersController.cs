using Messaging_Service.Api.Services;
using MessagingService.Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog.Fluent;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockUsersController : ControllerBase
    {
        private readonly IBlockUserService _blockUserService;
        public BlockUsersController(IBlockUserService blockUserService)
        {
            _blockUserService = blockUserService;

        }

        [HttpPost]
        [Route("BlockUser")]
        public async Task<IActionResult> BlockUserAsync(BlockUserDto blockUser)
        {
            if (blockUser == null || string.IsNullOrWhiteSpace(blockUser.BlockedUserName))
            {
                string errorMessage = string.Format("", blockUser.BlockedUserName);
                Log.Error(errorMessage);
                return BadRequest("Lütfen zorunlu alanı doldurunuz.");
            }

            // parameters.SenderUserId = int.Parse(User.FindFirst("UserId")?.Value); OTURUMDAN KULLANICININ ID DEĞERİNİ GETİRİYOR.
            var response = await _blockUserService.BlockUserAsync(blockUser);
            return Ok(response);
        }
        [HttpDelete]
        [Route("RemoveBlockUser")]
        public async Task<IActionResult> RemoveBlockUserAsync(BlockUserDto blockUser)
        {
            if (blockUser == null || string.IsNullOrWhiteSpace(blockUser.BlockedUserName))
            {
                string errorMessage = string.Format("", blockUser.BlockedUserName);
                Log.Error(errorMessage);
                return BadRequest("Lütfen zorunlu alanı doldurunuz.");
            }

            // parameters.SenderUserId = int.Parse(User.FindFirst("UserId")?.Value); OTURUMDAN KULLANICININ ID DEĞERİNİ GETİRİYOR.
            var response = await _blockUserService.RemoveBlockUserAsync(blockUser);
            return Ok(response);
        }
    }
}
