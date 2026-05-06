using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers;

public class ReferralCommitteesController: Controller
{
    public IActionResult Index()
    {
        return View();
    }
}