using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Client2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewBag.url = ReturnUrl;
            return View();
        }

        [Authorize]
        public IActionResult Users()
        {
            return View();
        }
    }
}