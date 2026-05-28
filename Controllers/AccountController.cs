using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Services;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

public class AccountController: Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    public IActionResult Login()
    {
        return View("Login");
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(CredentialVm credentials)
    {
        if (!ModelState.IsValid) return View(credentials);
        
        if (_accountService.ExistUser(credentials.Username, credentials.Password))
        {
            var claims = _accountService.GetClaims(credentials.Username);
            var principal = _accountService.GetPrincipal(claims, "MyAppCookies");
            var authProps = new AuthenticationProperties
            {
               IsPersistent = credentials.RememberMe
            };
            
            //hace el inicio de sesion usando las implementaciones injectadas
            //serialize claimsPrincipal into a string and it is saved as a cookie in the http context
            await HttpContext.SignInAsync("MyAppCookies", principal, authProps);
            
            return RedirectToAction("Index", "Home");
        }
        
        //TODO: user not exist message
        return View(credentials);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyAppCookies");
        return RedirectToAction("Login", "Account");
    }
}