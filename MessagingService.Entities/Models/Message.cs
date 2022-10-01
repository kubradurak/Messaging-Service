using MessagingService.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Models
{
    public class Message : BaseModel
    {
        public string SenderUserName { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
    }
}
