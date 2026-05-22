using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAppMVC.Filters.ExceptionFilters;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

[UnexpectedExceptionHandler]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Index", "Projects");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string errorMessage = "", string bodyMessage = "")
    {
        _logger.LogError("En Error view");
        _logger.LogError(errorMessage,  bodyMessage);
        if (errorMessage.IsNullOrEmpty())
            errorMessage = "Contáctese con el equipo de soporte";

        ViewBag.ErrorMessage = errorMessage;
        ViewBag.bodyMessage = bodyMessage;
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}