using MessagingService.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Models
{
    public class UserActivityLog: BaseModel
    {
        public string UserEmail { get; set; }
        public string ProcessName { get; set; }
        public string Description { get; set; }
        public bool IsSuccess { get; set; }
    }
}
