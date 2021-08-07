using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace System.Collections.Specialized
{
    public static class NameValueCollectionExtentions
    {
        public static IDictionary<string, string> ToDictionary(this NameValueCollection nv)
        {
            var d = new Dictionary<string, string>();
            foreach (var k in nv.AllKeys)
                d[k] = nv[k];
            return d;
        }
    }
}
