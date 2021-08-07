using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        #region Constants

        private const string REGEX = "[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]";
        #endregion

        #region Methods

        public static bool NotNullAnd(this string s, Func<string, bool> f)
        {
            return s != null && f(s);
        }

        public static bool HasArabicCharacters(this string s)
        {
            Regex regex = new Regex(REGEX);
            return regex.IsMatch(s);
        }

        #endregion
    }

    
}
