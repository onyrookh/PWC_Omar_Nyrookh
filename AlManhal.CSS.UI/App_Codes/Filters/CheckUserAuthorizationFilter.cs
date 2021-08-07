using PWC.Common.Helpers;
using PWC.Entities;
using PWC.Entities.DTOs;
using PWC.UI.Filters;
using PWC.UI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PWC.UI
{
    [NoCache]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CheckUserAuthorizationFilter : ActionFilterAttribute
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
                        filterContext.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                    }
                    else
                    {
                        string homeUrl = "/";
                        RedirectResult rr = new RedirectResult(homeUrl, true);
                        filterContext.Result = rr;
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
