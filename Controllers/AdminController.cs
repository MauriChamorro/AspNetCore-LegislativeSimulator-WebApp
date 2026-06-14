using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Services;
using WebAppMVC.Filters.ActionFilters.Async;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    private readonly IProjectService _projectService;
    private readonly ISimulationServices _simulationServices;
    private readonly INotificationService _notificationService;

    public AdminController(IProjectService projectService, 
        ISimulationServices simulationServices,
        INotificationService notificationService)
    {
        _projectService = projectService;
        _simulationServices = simulationServices;
        _notificationService = notificationService;
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
                StateName = project.GetCurrentState().ProjectState.Name,
                EnumState = project.GetCurrentState().ProjectState.State
            };
            return View(editProjectVm);
        }

        return View(new EditProjectAdminVm());
    }

    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    [HttpPost]
    public async Task<IActionResult> AssignCommissions(int projectId)
    {
        var result = await _simulationServices.AssignCommissionsFor(projectId);
        TempData["searchText"] = projectId;
        TempData["adminNoti"] = result;
        TempData["userNoti"] = "result";
        return RedirectToAction("EditProject", new { projectTxtId = projectId });
    }

    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    [HttpPost]
    public async Task<IActionResult> DoNextReferringRandomly(int projectId)
    {
        var result = await _simulationServices.DoNextReferringRandomly(projectId);
        TempData["SwalTitle"] = result;
        TempData["searchText"] = projectId;
        return RedirectToAction("EditProject", new { projectTxtId = projectId });
    }

    [HttpPost("EditReferral/{projectId}")]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public async Task<IActionResult> EditReferral(int projectId, int commissionId, ReferralState state)
    {
        var result = await _simulationServices.EditReferral(projectId, commissionId, state);
        TempData["adminNoti"] = result;
        TempData["userNoti"] = "hi";
        //await _notificationService.AddReferralChangeNotification(projectId);
        return RedirectToAction("Index", "ReferralCommittees", new { projectId });
    }

    [HttpPost]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public async Task<IActionResult> DoSessionRandomly(int projectId)
    {
        var result = await _simulationServices.DoSessionRandomly(projectId);
        TempData["adminNoti"] = result;
        TempData["userNoti"] = "result";
        return RedirectToAction("EditProject", new { projectTxtId = projectId });
    }
    
    [HttpPost]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public async Task<IActionResult> DoSession(int projectId, int sessionResult)
    {
        var result = await _simulationServices.DoSession(projectId, sessionResult);
        TempData["adminNoti"] = result;
        return RedirectToAction("EditProject", new { projectTxtId = projectId });
    }
}