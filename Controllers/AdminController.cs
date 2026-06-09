using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Controllers;

[Authorize(Roles = "admin")]
public class AdminController: Controller
{
    private readonly IProjectService _projectService;

    public AdminController(IProjectService projectService)
    {
        _projectService = projectService;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult EditProject()
    {
        return View();
    }
    
    public async Task<IActionResult> SearchProject(string projectId)
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

        if (!_projectService.ExistProjectAsync(id).Result)
        {
            TempData["SwalTitle"] = $"El Proyecto \"{id}\" no Existe.";
            TempData["searchText"] = projectId;
            return RedirectToAction("EditProject");
        }

        var project = await _projectService.GetProjectByIdAsync(id);
        TempData["SwalTitle"] = $"{project.Title}";
        return RedirectToAction("EditProject");
    }
}