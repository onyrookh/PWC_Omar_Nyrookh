using PWC.Entities.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Entities.Filters
{
   public class ThesisListFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public List<FieldSortVM> FieldSort { get; set; }
    }
}
