using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Dtos
{
    public class SendMessageDto
    {
        public string To { get; set; }
        public string Content { get; set; }

    }
}
