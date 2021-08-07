using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Entities.VMs.UI.Thesis
{
  public class ComplaintVM
    {
        public int ComplaintID { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }
        public string StatusName { get; set; }
        public int StatusID { get; set; }

    }
}
