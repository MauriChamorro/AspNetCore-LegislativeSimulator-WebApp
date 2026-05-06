using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers;

public class SimulationController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [Route("/AssignCommissions")]
    public IActionResult AssignCommissions()
    {
        return RedirectToAction("Index", "Home");
    }
}