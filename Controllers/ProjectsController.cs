using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Services;
using WebAppMVC.Filters.ActionFilters;
using WebAppMVC.Filters.ActionFilters.Async;
using WebAppMVC.Filters.ExceptionFilters;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

[UnexpectedExceptionHandler]
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

    public async Task<IActionResult> Index()
    {
        _notificationService.SendNotification(TempData);
        var projects = await _projectService.GetProjects();
        var projectVms = _projectViewModelService.ToProjectsVm(projects);
        return View(projectVms);
    }

    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.Action = "add";
        var emptyProject = _projectService.BuildEmptyProject();
        var newProjectVm = _projectViewModelService.ToProjectVm(emptyProject);
        return View(newProjectVm);
    }
    
    [HttpPost]
    public IActionResult Add(ProjectViewModel projectVm)
    {
        ViewBag.Action = "add";
        
        if (!ModelState.IsValid)
        {
            var emptyProject = _projectService.BuildEmptyProject();
            _projectViewModelService.UpdateMissingValues(projectVm, emptyProject);
            return View(projectVm);
        }

        _projectService.CreateNewProject(projectVm.Title, projectVm.Articles, projectVm.Fundaments,projectVm.Summary);
        _notificationService.AddProjectCreatedNotification();
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet("Projects/Edit/{projectId}")]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public async Task<IActionResult> Edit([FromRoute] int projectId)
    {
        ViewBag.Action = "edit";
        
        var project = await _projectService.GetProjectByIdAsync(projectId);
        
        var projectVm = _projectViewModelService.ToProjectVm(project);
        if (_commissionService.HasBeenAssigned(projectId))
        {
            var referrals = _commissionService.GetReferralsFor(projectId);
            _projectViewModelService.SetCommissionVmsToProjectVm(projectVm, referrals);
        }
        return View(projectVm);
    }

    [HttpPost]
    [ServiceFilter(typeof(ProjectVmAsyncFilterAttribute))]
    public async Task<IActionResult> Edit(ProjectViewModel projectVm)
    {
        ViewBag.Action = "edit";
        
        var savedProject = await _projectService.GetProjectByIdAsync(projectVm.ProjectId);
        
        if (!ModelState.IsValid)
        {
            //re fill fields for validation by data annotations
            _projectViewModelService.UpdateMissingValues(projectVm, savedProject);
            return View(projectVm);
        }

        UpdateProject(projectVm, savedProject);
        _notificationService.AddProjectUpdatedNotification();
        _notificationService.SendNotification(TempData);
        return View(projectVm);
    }

    [HttpPost]
    [ServiceFilter(typeof(ProjectVmAsyncFilterAttribute))]
    public async Task<IActionResult> SendToCommission(ProjectViewModel projectVm)
    {
        var savedProject = await _projectService.GetProjectByIdAsync(projectVm.ProjectId);
        
        //en el mundo real, habrían más validaciones
        if (!ModelState.IsValid)
        {
            _projectViewModelService.UpdateMissingValues(projectVm, savedProject);
            ViewBag.Action = "edit";
            return View("Edit", projectVm); //doesnt clear data for on back validation
        }
        
        _projectService.SetPendingForCommissionsFor(savedProject);
        UpdateProject(projectVm, savedProject);
        
        _notificationService.AddSentToCommissionsNotification();
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost("Projects/Delete/{projectId}")]
    [ServiceFilter(typeof(CanDeleteProjectAsyncFilterAttribute))]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public IActionResult Delete(int projectId)
    {
        _projectService.DeleteProject(projectId);
        _notificationService.AddProjectDeletedNotification();
        return RedirectToAction(nameof(Index));
    }
    
    private void UpdateProject(ProjectViewModel projectVm, Project savedProject)
    {
        _projectService.UpdateProject(
            savedProject,
            projectVm.Title, 
            projectVm.Articles,
            projectVm.Fundaments,
            projectVm.Summary);
    }
}