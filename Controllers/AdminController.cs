using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAppMVC.Domain.Services;
using WebAppMVC.ViewModels;

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
    
    [HttpGet]
    public async Task<IActionResult> EditProject([FromQuery] string projectTxtId)
    {
        if (projectTxtId.IsNullOrEmpty())
            return View(new EditProjectAdminVm());

        if (!int.TryParse(projectTxtId, out int projectId))
        {
            TempData["SwalTitle"] = "El número de proyecto no es válido.";
            TempData["searchText"] = projectTxtId;
            return View(new EditProjectAdminVm());
        }

        if (!_projectService.ExistProjectAsync(projectId).Result)
        {
            TempData["SwalTitle"] = $"El Proyecto \"{projectId}\" no Existe.";
            TempData["searchText"] = projectTxtId;
            return View(new EditProjectAdminVm());
        }
        
        if (projectId != 0)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);
            var editProjectVm = new EditProjectAdminVm
            {
                ProjectId = project.ProjectId,
                Title = project.Title,
                Summary = project.Summary,
                StateName = project.GetCurrentState().ProjectState.Name
            };
            return View(editProjectVm);
        }
        
        return View(new EditProjectAdminVm());
    }
}