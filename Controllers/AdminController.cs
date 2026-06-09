using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebAppMVC.Controllers;

[Authorize(Roles = "admin")]
public class AdminController: Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult EditProject()
    {
        return View();
    }
    
    public IActionResult SearchProject(string projectId)
    {
        if (projectId.IsNullOrEmpty())
        {
            TempData["SwalTitle"] = "Debe ingresar un número de proyecto.";
            TempData["searchText"] = projectId;
            return RedirectToAction("EditProject");
        }

        if (!int.TryParse(projectId, out int id))
        {
            TempData["SwalTitle"] = "El número de proyecto no es válido.";
            TempData["searchText"] = projectId;
            return RedirectToAction("EditProject");
        }
        
        TempData["SwalTitle"] = $"Si/No se encontró el proyecto \"{id}\"";
        return RedirectToAction("EditProject");
    }
}