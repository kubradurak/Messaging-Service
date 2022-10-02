using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Dtos
{
    public class AccessTokenDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
