using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Dtos
{
    public class MessageHistoryItemDto
    {
        public string SenderUserName { get; set; }
        public string To { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
