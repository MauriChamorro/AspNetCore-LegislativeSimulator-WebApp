using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers;

public class ProjectCommitteesController: Controller
{
    public IActionResult Index()
    {
        return View();
    }
}