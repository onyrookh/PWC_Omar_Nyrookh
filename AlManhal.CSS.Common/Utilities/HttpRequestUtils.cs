using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace PWC.Common.Utilities
{
    public static class HttpRequestUtils
    {
        private readonly static List<string> BOTS_LOOKUP = WebConfigurationManager.AppSettings["Bots"].Split(',').Select(s => s.Trim().ToLower()).ToList();
        private readonly static List<string> ALLOWED_BOTS_LOOKUP = WebConfigurationManager.AppSettings["AllowdBots"].Split(',').Select(s => s.Trim().ToLower()).ToList();
        public static bool IsCrawler(System.Web.HttpRequest oHttpRequest)
        {
            bool isCrwler = false;
            try
            {
                isCrwler = oHttpRequest.Browser.Crawler;
                if (!isCrwler)
                {
                    string userAgent = oHttpRequest.UserAgent.Trim().ToLower();
                    isCrwler = BOTS_LOOKUP.Any(b => userAgent.Contains(b));
                }
            }
            catch (Exception)
            {

                isCrwler = false;
            }


            return isCrwler;
        }

        public static bool IsAllowedCrawler(System.Web.HttpRequest oHttpRequest)
        {
            bool isAllowedCrwler = true;
            try
            {
                isAllowedCrwler = ALLOWED_BOTS_LOOKUP.Any(b => oHttpRequest.UserAgent.Trim().ToLower().Contains(b));
            }
            catch (Exception)
            {
                isAllowedCrwler = true;
            }

            return isAllowedCrwler;
        }

        public static string GetServerVariable(string key)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string variable = string.Empty;

            try
            {
                variable = context.Request.ServerVariables[key];
            }
            catch (Exception)
            {
                variable = string.Empty;
            }
            return variable;
        }

        public static bool CheckServerVariableExistance(string key)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            bool isExists = false;

            try
            {
                isExists = context.Request.ServerVariables[key] != null;
            }
            catch (Exception)
            {
                isExists = false;
            }
            return isExists;
        }
    }
}
