using PWC.Entities;
using PWC.Entities.DTOs;
using PWC.Entities.VMs.UI.Account;
using PWC.Infrastructure;
using PWC.Business;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Web;
using PWC.UI.Helpers;

namespace PWC.UI.Controllers
{
    public class AccountController : Base.BaseController
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM oLoginVM)
        {
            if (ModelState.IsValid)
            {
                ResponseEntity<AccountSessionInfoDto> oResponseEntity = new ResponseEntity<AccountSessionInfoDto>();
                oResponseEntity = await UserManager.GetAccountForLogin(oLoginVM.Username, oLoginVM.Password);
                if (oResponseEntity != null && oResponseEntity.Status != null && oResponseEntity.Status.IsSuccess && oResponseEntity.Data != null && oResponseEntity.Data.AccountID > 0)
                {
                    HttpContext.Session.SetParameter(Enums.SessionVariablesKeys.UserInfo.ToString(), oResponseEntity.Data);
                    return RedirectToHomePage();
                    
                }
                else
                {
                    ModelState.AddModelError(string.Empty, oResponseEntity.Status.ErrorMessage);
                }
            }
            return View(oLoginVM);
        }


        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpVM oLoginVM)
        {
            if (ModelState.IsValid)
            {
                Error oError = new Error(Error.ErrorCodeEnum.Success);
                oError =  UserManager.AddUser(oLoginVM.Username, oLoginVM.Password, oLoginVM.UserTypeID);
                if (oError.IsSuccess)
                {
                    return RedirectToLoginPage();

                }
                else
                {
                    ModelState.AddModelError(string.Empty, oError.ErrorMessage);
                }
            }
            return View(oLoginVM);
        }

        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToHomePage();
        }
    }
}
