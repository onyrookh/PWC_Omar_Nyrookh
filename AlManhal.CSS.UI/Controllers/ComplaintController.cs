using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PWC.Business;
using PWC.Common.Helpers;
using PWC.Entities;
using PWC.Entities.DTOs;
using PWC.Entities.VMs.UI.Thesis;
using PWC.UI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PWC.UI.Controllers
{
    public class ComplaintController : Base.SecureController
    {
        #region Log

        private static ILogger log = ApplicationLogging.CreateLogger("ComplaintController");

        #endregion

        #region Actions

        public IActionResult Index()
        {
            try
            {
                AccountSessionInfoDto oAccountSessionInfoDto = AuthenticationHelper.GetAuthenticatedAccount();
                if (oAccountSessionInfoDto != null)
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                log.LogTrace(ex, "Exception on Index Action: ");
            }

            return RedirectToHomePage();
        }

        public IActionResult AddEdit(int? id)
        {
            try
            {
                AccountSessionInfoDto oAccountSessionInfoDto = AuthenticationHelper.GetAuthenticatedAccount();
                if (oAccountSessionInfoDto != null)
                {
                    if (id.HasValue && id >0)
                    {
                       var oComplaint= ComplaintManager.GetComplaintByID(id);
                        if (oComplaint !=null)
                        {
                            ViewBag.Message = oComplaint.Message.Trim();
                            ViewBag.StatusID = oComplaint.StatusID;
                        }
                       
                    }

                    ViewBag.ComplaintID = id;

                    return View();
                }
            }
            catch (Exception ex)
            {
                log.LogTrace(ex, "Exception on Edit Action: ");
            }

            return RedirectToHomePage();
        }
        #endregion
    }
}