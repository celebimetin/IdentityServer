using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Client1.Controllers
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
    }
}