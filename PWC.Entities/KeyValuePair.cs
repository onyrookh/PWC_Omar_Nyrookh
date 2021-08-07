using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Entities
{
    public class KeyValuePairEntity<K, V>
    {
        public KeyValuePairEntity() { }
        public KeyValuePairEntity(K key, V value)
        {
            Key = key;
            Value = value;
        }

        public K Key { set; get; }
        public V Value { set; get; }
    }
}
