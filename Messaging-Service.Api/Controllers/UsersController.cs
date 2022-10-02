using Messaging_Service.Api.Services;
using MessagingService.Entities.Dtos;
using MessagingService.Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog.Fluent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messaging_Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;

       }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync(UserDto userDto)
        {
            if (userDto == null || string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
            {
                string errorMessage = string.Format("RegisterAsync: Please fill in the required fields! Parametres: userName:{0}, password:{1}", userDto.UserName, userDto.Password);
                Log.Error(errorMessage);
                return BadRequest("Please fill in the required fields!");
            }

            var response = await _userService.RegisterAsync(userDto);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(UserLoginDto userLoginDto)
        {
            if (userLoginDto == null || string.IsNullOrEmpty(userLoginDto.UserName) || string.IsNullOrEmpty(userLoginDto.Password))
            {
                string errorMessage = string.Format("LoginAsync: Please fill in the required fields! Parametres: userName:{0}, password:{1}", userLoginDto.UserName, userLoginDto.Password);
                Log.Error(errorMessage);
                return BadRequest("Please fill in the required fields!");
            }

            var response = await _userService.LoginAsync(userLoginDto);



            return Ok(response);
        }
    }
}
