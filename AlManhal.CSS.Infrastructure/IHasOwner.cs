using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Infrastructure
{
    /// <summary>
    /// This interface is used to mark the owner of an object.
    /// </summary>
    public interface IHasOwner<T>
    {
        /// <summary>
        /// The Member instance this object belongs to.
        /// </summary>
        T Owner { get; set; }
    }
}

