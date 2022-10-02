using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Dtos
{
    public class UserActivityLogDto
    {
        public string UserEmail { get; set; }
        public string ProcessName { get; set; }
        public string Description { get; set; }
        public bool IsSuccess { get; set; }
    }
}
