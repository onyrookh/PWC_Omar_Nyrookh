using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Entities.Attributes
{
    public class PropertyKey : Attribute
    {
        public int fieldKey;

        public PropertyKey(int fieldKey)
        {
            this.fieldKey = fieldKey;
        }
    }
}
