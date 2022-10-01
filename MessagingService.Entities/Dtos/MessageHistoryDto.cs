using MessagingService.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Dtos
{
    public class MessageHistoryDto
    {
        public List<Message> MessageHistoryList { get; set; }
    }
}
