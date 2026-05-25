using Microsoft.AspNetCore.Mvc;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

public class LoginController: Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Index(CredentialVm credentials)
    {
        if (!ModelState.IsValid)
        {
            credentials.Username = "";
            credentials.Password = "";
        }
        
        return View();
    }
}