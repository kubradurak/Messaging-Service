using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Dtos
{
    public class BlockUserDto
    {
        public string BlockerUserName { get; set; }
        public string BlockedUserName { get; set; }
    }
}
