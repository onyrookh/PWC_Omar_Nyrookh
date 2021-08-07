using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Entities.DTOs
{
   public class ComplaintDto
    {
        public int StatusID { get; set; }
        public int ComplaintID { get; set; }
        public string Message { get; set; }
    }
}
