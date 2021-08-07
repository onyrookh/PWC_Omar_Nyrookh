using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Entities.DTOs
{
    public class AccountSessionInfoDto
    {
        public int AccountID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int UserTypeID { get; set; }
        public int PublishingOwnerID { get; set; }
    }
}
