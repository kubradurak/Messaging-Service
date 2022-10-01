using MessagingService.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Models
{
    public class BlockUser : BaseModel
    {
        public string BlockerUserName { get; set; }
        public string BlockedUserName { get; set; }
        public bool IsLocked { get; set; }
    }
}

