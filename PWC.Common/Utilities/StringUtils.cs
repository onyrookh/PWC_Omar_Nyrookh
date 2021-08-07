using System;
using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class StringUtils
    {

        #region Methods

        public static int TryParse(string u, int defaultValue)
        {
            try
            {
                return int.Parse(u);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int TryParse(string u)
        {
            return TryParse(u, 0);
        }

        /// <summary>
        /// Like null coalescing operator (??) but including empty strings
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string IfNullOrEmpty(string a, string b)
        {
            return string.IsNullOrEmpty(a) ? b : a;
        }

        /// <summary>
        /// If <paramref name="a"/> is empty, returns null
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string EmptyToNull(string a)
        {
            return string.IsNullOrEmpty(a) ? null : a;
        }

        public static List<T> ConvertSplitedStringToList<T>(string splitedString, string separater = ",")
        {
            List<T> lst = null;
            if (!string.IsNullOrEmpty(splitedString))
            {
                lst = splitedString.Split(separater.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(value =>
                      Convert.ChangeType(value.ToString(), typeof(T))
                 ).OfType<T>().ToList();
            }
            return lst;
        }

        #endregion
    }
}
