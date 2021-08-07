using PWC.Common.Helpers;
using PWC.Entities;
using PWC.Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace PWC.UI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BasicAuthenticationFilter : Attribute, IAuthorizationFilter
    {
        bool Active = true;

        public BasicAuthenticationFilter() { }

        public BasicAuthenticationFilter(bool active)
        {
            Active = active;
        }

        public void OnAuthorization(AuthorizationFilterContext actionContext)
        {
            if (Active)
            {
                HttpContext ctx = SessionHelper.GetHttpContext();

                // check if session is supported
                if (ctx.Session != null)
                {
                    var oSessionInfo = ctx.Session.GetParameter<AccountSessionInfoDto>(Enums.SessionVariablesKeys.UserInfo.ToString());

                    // If the browser session or authentication session has expired...
                    if (oSessionInfo == null)
                    {
                        Challenge(actionContext);
                        return;
                    }
                }
            }
        }

        protected virtual bool OnAuthorizeUser(string username, string password, AuthorizationFilterContext actionContext)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            return true;
        }

        /// <summary>
        /// Send the Authentication Challenge request
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actionContext"></param>
        void Challenge(AuthorizationFilterContext actionContext)
        {
            actionContext.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
        }

    }
}
