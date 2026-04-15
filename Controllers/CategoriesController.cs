using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers;

public class CategoriesController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}