using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using PWC.Entities.DTOs;
using PWC.UI.Filters;
using PWC.UI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PWC.UI.Base
{
    [TypeFilter(typeof(CustomExceptionFilterAttribute))]
    public class BaseController : Controller
    {
        #region Constant

        private const string INDEX_ACTION = "Index";
        private const string HOME_CONTROLLER = "Home";
        private const string LOGIN_ACTION = "Login";
        private const string ACCOUNT_CONTROLLER = "Account";

        #endregion

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.BundleVersion = "?ver=" + WebConfigurationManager.AppSettings["BundlesVersion"];
            ViewBag.StaticResources = WebConfigurationManager.AppSettings["StaticResources"].ToString().ToLower();
        }

        public RedirectToActionResult RedirectToHomePage()
        {
            return RedirectToAction(INDEX_ACTION, HOME_CONTROLLER);
        }

        public RedirectToActionResult RedirectToLoginPage()
        {
            return RedirectToAction(LOGIN_ACTION, ACCOUNT_CONTROLLER);
        }
    }
}