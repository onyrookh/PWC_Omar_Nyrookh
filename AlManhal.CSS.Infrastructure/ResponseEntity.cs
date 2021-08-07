using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Infrastructure
{
    public class ResponseEntity<T> where T: class
    {
        public ResponseEntity(){}
        public ResponseEntity(Error oError)
        {
            Status = oError;
        }

        public T Data { get; set; }
        public Error Status { get; set; }
    }
}
