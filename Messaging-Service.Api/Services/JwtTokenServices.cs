using MessagingService.Entities.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using MessagingService.Base.Constants;

namespace Messaging_Service.Api.Services
{
    public class JwtTokenServices : ITokenServices
    {
        public string AccessToken { get; set; }

        public JwtTokenServices(IConfiguration configuration)
        {
            AccessToken = configuration.GetValue<string>("App:JwtSecretKey");
        }

        public AccessTokenDto CreateToken(CreateTokenDto createTokenDto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricKey = Convert.FromBase64String(AccessToken);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, createTokenDto.UserName),
                    new Claim(ClaimTypes.Role, "AppUser")
                }),
                Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(TokenConstants.GetExprationDate())),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return new AccessTokenDto
            {
                Token = token,
                Expiration = (DateTime)tokenDescriptor.Expires
            };
        }
    }
}

