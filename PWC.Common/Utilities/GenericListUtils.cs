using System.Collections.Generic;
using System.Linq;
using System.Data.Objects.DataClasses;
namespace System
{
    public static class GenericListUtils
    {
        #region Methods

        public static bool IsNullOrEmpty<T>(List<T> list) where T : class
        {
            if (list == null)
                return true;

            return !(list.Count > 0);
        }

        public static bool IsNullOrEmpty<T>(EntityCollection<T> list) where T : class
        {
            if (list == null)
                return true;

            return !list.Any();
        }

        #endregion
    }
}
