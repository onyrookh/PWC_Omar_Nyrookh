using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    /// <summary>
    /// Query string processing extensions
    /// </summary>
    public static class UrlUtils
    {
        #region Methods
        /// <summary>
        /// Sets/changes an url's query string parameter.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="url">URL to process</param>
        /// <param name="key">Query string parameter key to set/change</param>
        /// <param name="value">Query string parameter value</param>
        /// <returns>Resulting URL</returns>
        public static string SetParameter(string url, string key, string value)
        {
            return SetParameters(url, new Dictionary<string, object> {
                {key, value}
            });
        }

        /// <summary>
        /// Sets/changes an url's query string parameters.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="url">URL to process</param>
        /// <param name="parameters">Paramteres to set/change</param>
        /// <returns>Resulting URL</returns>
        public static string SetParameters(string url, IDictionary<string, object> parameters)
        {
            var parts = url.Split('?');
            IDictionary<string, string> qs = new Dictionary<string, string>();
            if (parts.Length > 1)
                qs = ParseQueryString(parts[1]);
            foreach (var p in parameters)
                qs[p.Key] = p.Value.ToNullOrString();
            return parts[0] + "?" + DictToQuerystring(qs);
        }

        /// <summary>
        /// Removes parameters from an url's query string
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="url">URL to process</param>
        /// <param name="parameters">Query string parameter keys to remove</param>
        /// <returns>Resulting URL</returns>
        public static string RemoveParametersUrl(string url, params string[] parameters)
        {
            var parts = url.Split('?');
            IDictionary<string, string> qs = new Dictionary<string, string>();
            if (parts.Length > 1)
                qs = ParseQueryString(parts[1]);
            foreach (var p in parameters)
                qs.Remove(p);
            return parts[0] + "?" + DictToQuerystring(qs);
        }

        public static string RemoveParameters(params string[] parameters)
        {
            return RemoveParametersUrl(HttpContext.Current.Request.RawUrl, parameters);
        }

        public static string DictToQuerystring(IDictionary<string, string> qs)
        {
            return string.Join("&", qs
                .Where(k => !string.IsNullOrEmpty(k.Key))
                .Select(k => string.Format("{0}={1}", HttpUtility.UrlEncode(k.Key), HttpUtility.UrlEncode(k.Value))).ToArray());
        }

        /// <summary>
        /// Sets/changes a single parameter from the current query string.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="key">Parameter key</param>
        /// <param name="value">Parameter value</param>
        /// <returns>Resulting URL</returns>
        public static string SetParameter(string key, object value)
        {
            return SetParameter(HttpContext.Current.Request.RawUrl, key, value.ToNullOrString());
        }

        /// <summary>
        /// Sets/changes the current query string's parameters, using <paramref name="parameterDictionary"/> as dictionary
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="parameterDictionary">Parameters to set/change</param>
        /// <returns>Resulting URL</returns>
        public static string SetParameters(object parameterDictionary)
        {
            return SetParameters(HttpContext.Current.Request.RawUrl, parameterDictionary.ToPropertyDictionary());
        }

        /// <summary>
        /// Parses a query string. If duplicates are present, the last key/value is kept.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ParseQueryString(string s)
        {
            var d = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            if (s == null)
                return d;
            if (s.StartsWith("?"))
                s = s.Substring(1);
            foreach (var kv in s.Split('&'))
            {
                var v = kv.Split('=');
                if (string.IsNullOrEmpty(v[0]))
                    continue;
                d[HttpUtility.UrlDecode(v[0])] = HttpUtility.UrlDecode(v[1]);
            }
            return d;
        }

        public static string ForQuery(string url, string solrQuery, int type)
        {
            string query = SetParameter(url, "q", solrQuery);

            switch (type)
            {
                case 1:
                    query = SetParameter(url, "sf_2_0_2", solrQuery);
                    break;
                case 2:
                    query = SetParameter(url, "sf_8_0_2", solrQuery);
                    break;
                case 3:
                    query = SetParameter(url, "sf_23_0_2", solrQuery);
                    break;
            }
            
            return query;
        }

        public static string ForQuery(string url, string solrQuery)
        {
            return SetParameter(url, "q", solrQuery);
        }

        public static IDictionary<string, string> ToKeyValue(this object metaToken)
        {
            if (metaToken == null)
            {
                return null;
            }

            JToken token = metaToken as JToken;
            if (token == null)
            {
                return ToKeyValue(JObject.FromObject(metaToken));
            }

            if (token.HasValues)
            {
                var contentData = new Dictionary<string, string>();
                foreach (var child in token.Children().ToList())
                {
                    var childContent = child.ToKeyValue();
                    if (childContent != null)
                    {
                        contentData = contentData.Concat(childContent)
                                                 .ToDictionary(k => k.Key, v => v.Value);
                    }
                }

                return contentData;
            }

            var jValue = token as JValue;
            if (jValue?.Value == null)
            {
                return null;
            }

            var value = jValue?.Type == JTokenType.Date ?
                            jValue?.ToString("o", CultureInfo.InvariantCulture) :
                            jValue?.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string> { { token.Path, value } };
        }

        #endregion
    }
}
