using MessagingService.Base.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessagingService.Entities.Models
{
    public class User : BaseModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
