using MessagingService.Entities.Dtos;

namespace Messaging_Service.Api.Services
{
    public interface ITokenServices
    {
        AccessTokenDto CreateToken(CreateTokenDto parameters);

    }
}
