using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Services;
using WebAppMVC.Filters.ActionFilters.Async;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IProjectViewModelService _projectViewModelService;
    private readonly ICommissionService _commissionService;
    private readonly INotificationService _notificationService;

    public ProjectsController(IProjectService projectService,
        IProjectViewModelService projectViewModelService,
        ICommissionService commissionService,
        INotificationService notificationService)
    {
        _projectService = projectService;
        _projectViewModelService = projectViewModelService;
        _commissionService = commissionService;
        _notificationService = notificationService;
    }

    [Authorize(Roles = "legislador, admin")]
    public async Task<IActionResult> Index()
    {
        var projects = await _projectService.GetProjects();
        var projectVms = _projectViewModelService.ToProjectsVm(projects);
        return View(projectVms);
    }

    [HttpGet]
    [Authorize(Roles = "legislador")]
    public async Task<IActionResult> Add()
    {
        var emptyProject = await _projectService.BuildEmptyProject();
        var newProjectVm = _projectViewModelService.ToProjectVm(emptyProject);
        return View(newProjectVm);
    }
    
    [Authorize(Roles = "legislador")]
    [HttpPost]
    public async Task<IActionResult> Add(ProjectViewModel projectVm)
    {
        if (!ModelState.IsValid)
        {
            var emptyProject = await _projectService.BuildEmptyProject();
            _projectViewModelService.UpdateMissingValues(projectVm, emptyProject);
            return View(projectVm);
        }

        await _projectService.CreateNewProject(projectVm.Title, projectVm.Articles, projectVm.Fundaments,projectVm.Summary);
        _notificationService.AddProjectCreatedNotification();
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet("Projects/Edit/{projectId}")]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    [Authorize(Roles = "legislador")]
    public async Task<IActionResult> Edit([FromRoute] int projectId)
    {
        var project = await _projectService.GetProjectByIdAsync(projectId);
        
        var projectVm = _projectViewModelService.ToProjectVm(project);
        var hasBeenAssigned = await _commissionService.HasBeenAssigned(projectId);
        if (hasBeenAssigned)
        {
            var referrals = await _commissionService.GetReferralsFor(projectId);
            _projectViewModelService.SetCommissionVmsToProjectVm(projectVm, referrals);
        }
        return View(projectVm);
    }

    [HttpPost]
    [ServiceFilter(typeof(ProjectVmAsyncFilterAttribute))]
    [Authorize(Roles = "legislador")]
    public async Task<IActionResult> Edit(ProjectViewModel projectVm)
    {
        var savedProject = await _projectService.GetProjectByIdAsync(projectVm.ProjectId);
        
        if (!ModelState.IsValid)
        {
            //re fill fields for validation by data annotations
            _projectViewModelService.UpdateMissingValues(projectVm, savedProject);
            return View(projectVm);
        }

        await UpdateProject(projectVm, savedProject);
        _notificationService.AddProjectUpdatedNotification();
        return View(projectVm);
    }

    [HttpPost]
    [ServiceFilter(typeof(ProjectVmAsyncFilterAttribute))]
    [Authorize(Roles = "legislador")]
    public async Task<IActionResult> SendToCommission(ProjectViewModel projectVm)
    {
        var savedProject = await _projectService.GetProjectByIdAsync(projectVm.ProjectId);
        
        //en el mundo real, habrían más validaciones
        if (!ModelState.IsValid)
        {
            _projectViewModelService.UpdateMissingValues(projectVm, savedProject);
            return View("Edit", projectVm); //doesn't clear data for on back validation
        }
        
        await _projectService.SetPendingForCommissionsFor(savedProject);
        await UpdateProject(projectVm, savedProject);
        
        _notificationService.AddSentToCommissionsNotification();
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost("Projects/Delete/{projectId}")]
    [ServiceFilter(typeof(CanDeleteProjectAsyncFilterAttribute))]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    [Authorize(Roles = "legislador")]
    public async Task<IActionResult> Delete(int projectId)
    {
        await _projectService.DeleteProject(projectId);
        //todo: setear estado Eliminado por Legislador solo cuando se decida Marcar y no borrar de BD
        _notificationService.AddProjectDeletedNotification("legislador");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("SendToSession/{projectId}")]
    [Authorize(Roles = "legislador")]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public async Task<IActionResult> SendToSession(int projectId)
    {
        var referrals = await _commissionService.GetReferralsFor(projectId);
        if (referrals.Count == 0)
        {
            TempData["SwalTitle"] = "No es posible enviar a Sesión";
            return RedirectToAction(nameof(Index));
        }

        var project = await _projectService.GetProjectByIdAsync(projectId);
        var canSendToSession = await _projectService.CanSendToSession(project);
        if (!canSendToSession)
        {
            TempData["SwalTitle"] = "El Proyecto debe ser Aprobado por todas las Comisiones";
            return RedirectToAction(nameof(Index));
        }
       
        await _projectService.SendToSession(project.ProjectId);
        _notificationService.AddSentToSessionNotification("legislador");
        return RedirectToAction(nameof(Index));
    }
    
    private async Task UpdateProject(ProjectViewModel projectVm, Project savedProject)
    {
       await _projectService.UpdateProject(
            savedProject,
            projectVm.Title, 
            projectVm.Articles,
            projectVm.Fundaments,
            projectVm.Summary);
    }
}