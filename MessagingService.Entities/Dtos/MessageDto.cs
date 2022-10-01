using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Dtos
{
    public class MessageDto
    {
        public string SenderUserName { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
