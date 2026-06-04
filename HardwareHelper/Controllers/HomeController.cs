using HardwareHelper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HardwareHelper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // 1. Jeśli użytkownik nie jest zalogowany, automatycznie wyrzuć go na stronę logowania
            if (!User.Identity.IsAuthenticated)
            {
                return LocalRedirect("/Identity/Account/Login");
            }

            // 2. Jeśli jest zalogowany, sprawdzamy jego rolę
            if (User.IsInRole("Admin") || User.IsInRole("Serwisant"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Zlecenia");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult BrakDostepu()
        {
            return View();
        }
    }
}
