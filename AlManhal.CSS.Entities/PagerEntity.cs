using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Entities
{
    public class PagerEntity<T> where T : class
    {
        public PagerEntity() { }

        public PagerEntity(int pageSize, int pageNumber)
        {

            PageSize = pageSize;
            PageNumber = pageNumber;
        }

        public IList<T> Data { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalRows { get; set; }
        public int TotalPages
        {
            get
            {
                double value = TotalRows <= 0 ? 0.0 : (double)TotalRows / PageSize;
                return TotalRows <= 0 ? 0 : (int)Math.Ceiling(value);
            }
        }
    }
}
