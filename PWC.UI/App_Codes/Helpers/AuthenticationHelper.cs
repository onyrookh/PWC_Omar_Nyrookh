using PWC.Entities.DTOs;
using PWC.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Collections.Generic;
using PWC.Common.Helpers;
using System.Web.Configuration;

namespace PWC.UI.Helpers
{
    public static class AuthenticationHelper
    {
        public static AccountSessionInfoDto GetAuthenticatedAccount()
        {
            HttpContext ctx = SessionHelper.GetHttpContext();
            return ctx.Session.GetParameter<AccountSessionInfoDto>(Enums.SessionVariablesKeys.UserInfo.ToString());
        }

    }
}
