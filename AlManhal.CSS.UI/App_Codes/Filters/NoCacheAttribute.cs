using System;
using Microsoft.AspNetCore.Mvc.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PWC.UI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                filterContext.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                filterContext.HttpContext.Response.Headers["Expires"] = "-1";
                filterContext.HttpContext.Response.Headers["Pragma"] = "no-cache";
            }
            catch
            {
                
            }

            base.OnActionExecuted(filterContext);
        }
    }
}
