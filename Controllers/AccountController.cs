using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

public class AccountController: Controller
{
    public IActionResult Login()
    {
        return View("Login");
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(CredentialVm credentials)
    {
        if (!ModelState.IsValid) return View(credentials);
        
        //validation service
        if (credentials.Username == "admin" && credentials.Password == "admin")
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, credentials.Username),
                new(ClaimTypes.Country, "ar"),
                new("Legislador","true"),
                new("ProbationDate","2026-5-20")
            };

            //Establece (set) la Autenticación; mediante Cookies (web)
            //IsAuthenticated pasa a ser true.
            var identity = new ClaimsIdentity(claims, "MyAppCookies");
            
            //user-like
            var principal = new ClaimsPrincipal(identity);
            
            var authProps = new AuthenticationProperties
            {
               IsPersistent = credentials.RememberMe
            };
            
            //hace el inicio de sesion usando las implementaciones injectadas
            //serialize claimsPrincipal into a string and it is saved as a cookie in the http context
            await HttpContext.SignInAsync("MyAppCookies", principal, authProps);
            
            return RedirectToAction("Index", "Home");
        }
        
        //TODO: auth error message
        return View(credentials);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyAppCookies");
        return RedirectToAction("Login", "Account");
    }
}