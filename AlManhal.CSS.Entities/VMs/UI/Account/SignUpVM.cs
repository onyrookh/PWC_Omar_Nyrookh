using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Entities.VMs.UI.Account
{
   public class SignUpVM
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmePassword { get; set; }

        [Required]
        public int UserTypeID { get; set; }
    }
}
