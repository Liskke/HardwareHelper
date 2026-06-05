using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HardwareHelper.Models;

namespace HardwareHelper.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

        public IActionResult Index()
{
    if (!User.Identity.IsAuthenticated)
    {
        return LocalRedirect("/Identity/Account/Login");
    }

    if (User.IsInRole("Admin") || User.IsInRole("Serwisant"))
    {
        return RedirectToAction("Index", "Serwisant");
    }
    
    return RedirectToAction("Index", "Zlecenia");
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