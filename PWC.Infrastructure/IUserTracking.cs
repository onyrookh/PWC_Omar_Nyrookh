using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWC.Infrastructure
{
    /// <summary>
    /// Defines an interface for objects whose creation and modified dates are kept track of.
    /// </summary>
    public interface IUserTracking
    {
        /// <summary>
        /// Gets or sets the user id who created the object.
        /// </summary>
        int? CreatedByID { get; set; }

        /// <summary>
        /// Gets or sets the user id who modified the object.
        /// </summary>
        int? ModifiedByID { get; set; }
    }
}

