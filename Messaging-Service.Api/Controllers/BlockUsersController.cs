using AutoMapper;
using Messaging_Service.Api.Services;
using MessagingService.Entities.Dtos;
using MessagingService.Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog.Fluent;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Controllers
{


    /// <summary>
    /// Block Users Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BlockUsersController : ControllerBase
    {
        private readonly IBlockUserService _blockUserService;
        private readonly IMapper _mapper;

        /// ctor
        public BlockUsersController(IBlockUserService blockUserService, IMapper mapper)
        {
            _blockUserService = blockUserService;
            _mapper = mapper;

        }
        /// <summary>
        /// Block User
        /// </summary>
        /// <remarks>
        ///  Sample request:
        ///     POST BlockUsers/BlockUser
        ///     {
        ///         "BlockerUserName" : "kub",
        ///         "BlockedUserName": "beyza"
        ///      }
        /// </remarks>
        /// <param name="blockUser"></param>
        /// <returns>reponse</returns>
        [HttpPost]
        [Route("BlockUser")]
        public async Task<IActionResult> BlockUserAsync(BlockUserDto blockUser)
        {
            if (blockUser == null || string.IsNullOrWhiteSpace(blockUser.BlockedUserName))
            {
                string errorMessage = string.Format("BlockUserAsync: Please fill in the required fields!Parametres: BlockedUserName:{0}", blockUser.BlockedUserName);
                Log.Error(errorMessage);
                return BadRequest("Please fill in the required fields!");
            }
            var _blockUser = _mapper.Map<BlockUser>(blockUser);
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            _blockUser.BlockerUserName = identity?.FindFirst("unique_name")?.Value;
            var response = await _blockUserService.BlockUserAsync(_blockUser);
            return Ok(response);
        }
        /// <summary>
        /// Remove Block User
        /// </summary>
        /// <remarks>
        ///  Sample request:
        ///     DELETE BlockUsers/RemoveBlockUser
        ///     {
        ///         "BlockerUserName" : "kub",
        ///         "BlockedUserName": "beyza"
        ///      }
        /// </remarks>
        /// <param name="blockUser"></param>
        /// <returns>responce</returns>
        [HttpDelete]
        [Route("RemoveBlockUser")]
        public async Task<IActionResult> RemoveBlockUserAsync(BlockUserDto blockUser)
        {
            if (blockUser == null || string.IsNullOrWhiteSpace(blockUser.BlockedUserName))
            {
                string errorMessage = string.Format("BlockUserAsync: Please fill in the required fields!Parametres: BlockedUserName:{0}}", blockUser.BlockedUserName);
                Log.Error(errorMessage);
                return BadRequest("Please fill in the required fields!");
            }
            var _blockUser = _mapper.Map<BlockUser>(blockUser);
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            _blockUser.BlockerUserName = identity?.FindFirst("unique_name")?.Value;
            var response = await _blockUserService.RemoveBlockUserAsync(_blockUser);
            return Ok(response);
        }
    }
}
