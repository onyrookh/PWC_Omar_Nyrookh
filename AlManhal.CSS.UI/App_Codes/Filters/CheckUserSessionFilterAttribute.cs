using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Web.Security;
using PWC.Common.Helpers;
using PWC.Entities;
using PWC.Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PWC.UI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckUserSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = SessionHelper.GetHttpContext();

            // check if session is supported
            if (ctx.Session != null)
            {
                var oAccountSessionInfo = ctx.Session.GetParameter<AccountSessionInfoDto>(Enums.SessionVariablesKeys.UserInfo.ToString());


                if (oAccountSessionInfo == null)
                {

                    if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        // For AJAX requests, we're overriding the returned JSON result with a simple string,
                        // indicating to the calling JavaScript code that a redirect should be performed.
                        filterContext.Result = new UnauthorizedResult();
                    }
                    else
                    {
                        string redirectOnSuccess = filterContext.HttpContext.Request.Path.Value;
                        string redirectUrl = string.Format("?ReturnUrl={0}", redirectOnSuccess.ToLower());
                        string loginUrl = "/Account/Login" + redirectUrl;
                        RedirectResult rr = new RedirectResult(loginUrl, true);
                        filterContext.Result = rr;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Send the Authentication Challenge request
        /// </summary>
        /// <param name="message"></param>
        /// <param name="actionContext"></param>
        void Challenge(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary {
                                { "Controller", "Account" },
                                { "Action", "Login" }
                                    });
        }
    }
}
