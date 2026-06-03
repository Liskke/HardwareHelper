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
        // 1. Jeśli użytkownik NIE jest zalogowany, przekieruj go bezpośrednio do strony logowania ASP.NET Core Identity
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }

        // 2. Jeśli zalogowany użytkownik ma rolę Admina lub Serwisanta, przekieruj go do Twojego panelu serwisanta
        if (User.IsInRole("Admin") || User.IsInRole("Serwisant"))
        {
            return RedirectToAction("Index", "Serwisant");
        }

        // 3. Jeśli zalogowany użytkownik ma rolę Klienta, przekieruj go do panelu klienta
        if (User.IsInRole("Klient"))
        {
            return RedirectToAction("MojeZgloszenia", "Klient");
        }

        // Domyślny widok awaryjny (np. jeśli użytkownik nie ma przypisanej żadnej roli)
        return View();
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
}