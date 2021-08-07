using Microsoft.AspNetCore.Mvc;

namespace PWC.UI.Controllers
{
    public class HomeController : Base.BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
