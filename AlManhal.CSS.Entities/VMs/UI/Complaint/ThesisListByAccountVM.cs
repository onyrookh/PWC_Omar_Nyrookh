using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Entities.VMs.UI.Thesis
{
  public class ThesisListByAccountVM
    {
        public int ThesisID { get; set; }
        public string ThesisTitle { get; set; }
        public string SubmitDate { get; set; }
        public string Status { get; set; }
    }
}
