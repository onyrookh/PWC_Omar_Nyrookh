using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    public static class SolrUtils
    {
        #region Constants

        private const string F_0_FORMAT = "f_{0}";
        private const string O_P_F_0_FORMAT = "opf_{0}";
        private const string PAGE = "page";
        private const string ONE = "1";
        private const string S_F_0_FORMAT = "sf_{0}";
        private const string O_P_S_F_0_FORMAT = "opsf_{0}";
        private const string S_F_0_1_2_FORMAT = "sf_{0}_{1}_{2}";
        private const string ZERO_ONE_FORMAT = "{0}_{1}";
        #endregion

        #region Methods

        public static string SetFacet(string url, string key, string value, string op = "")
        {
            string newUrl = UrlUtils.SetParameters(url, new Dictionary<string, object> {
                {string.Format(F_0_FORMAT, key), value}
            });
            if (!string.IsNullOrEmpty(op))
            {
                newUrl = SetFacetOperator(newUrl, key, op);
            }
            return newUrl;
        }

        public static string SetFacetOperator(string url, string key, string value)
        {
            return UrlUtils.SetParameters(url, new Dictionary<string, object> {
                {string.Format(O_P_F_0_FORMAT, key), value}
            });
        }

        public static string RemoveFacet(string url, string facet)
        {
            var noFacet = UrlUtils.RemoveParametersUrl(url, string.Format(F_0_FORMAT, facet));
            return UrlUtils.SetParameter(noFacet, PAGE, ONE);
        }

        public static string SetField(string url, string field,string value, string matchType,string op = "")
        {
            int count = Regex.Matches(url, string.Format(S_F_0_FORMAT, field)).Count;
            string newUrl = UrlUtils.SetParameters(url, new Dictionary<string, object> {
                {string.Format(S_F_0_1_2_FORMAT, field,count > 0 ? count/2 : 0,matchType), value}
            });
            if (!string.IsNullOrEmpty(op))
            {
                newUrl = SetFieldOperator(newUrl, string.Format(ZERO_ONE_FORMAT, field, count > 0 ? count / 2 : 0), op);
            }
            return newUrl;
        }

        public static string SetFieldOperator(string url, string field, string value)
        {
            return UrlUtils.SetParameters(url, new Dictionary<string, object> {
                {string.Format(O_P_S_F_0_FORMAT, field), value}
            });
        }

        public static string RemoveField(string url, string field)
        {
            var noFacet = UrlUtils.RemoveParametersUrl(url, string.Format(S_F_0_FORMAT, field));
            return UrlUtils.SetParameter(noFacet, PAGE, ONE);
        }

        #endregion
    }
}
