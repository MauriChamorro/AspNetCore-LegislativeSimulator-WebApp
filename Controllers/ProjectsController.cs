using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Services;
using WebAppMVC.Filters.ActionFilters;
using WebAppMVC.Filters.ExceptionFilters;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

[UnexpectedHandleException]
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

    public IActionResult Index()
    {
        CheckNotifications();
        var projects = _projectService.GetProjects();
        var projectVms = _projectViewModelService.ToProjectsVm(projects);
        return View(projectVms);
    }

    private void CheckNotifications()
    {
        if (_notificationService.ThereAreNotification())
        {
            var noti = _notificationService.GetNextNotification();
            TempData["SwalTitle"] = noti.Title;
            TempData["SwalMessage"] = noti.Message;
            TempData["SwalIcon"] = "info"; // success, error, warning, info
        }
    }

    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.Action = "add";
        var emptyProject = _projectService.CreatEmptyProject();
        var newProjectVm = _projectViewModelService.ToProjectVm(emptyProject);
        return View(newProjectVm);
    }
    
    [HttpPost]
    public IActionResult Add(ProjectViewModel projectVm)
    {
        ViewBag.Action = "add";
        
        if (!ModelState.IsValid)
        {
            var emptyProject = _projectService.CreatEmptyProject();
            _projectViewModelService.UpdateMissingValues(projectVm, emptyProject);
            return View(projectVm);
        }

        _projectService.CreateNewProject(projectVm.Title, projectVm.Articles, projectVm.Fundaments,projectVm.Summary);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet("Projects/Edit/{projectId}")]
    [ProjectIdNotFoundFilter]
    public IActionResult Edit([FromRoute] int projectId)
    {
        ViewBag.Action = "edit";
        
        var project = _projectService.GetProjectById(projectId);
        
        var projectVm = _projectViewModelService.ToProjectVm(project);
        if (_commissionService.HasBeenAssigned(projectId))
        {
            var referralCommissions = _commissionService.GetReferralCommissionsFor(projectId);
            _projectViewModelService.SetVmCommissions(projectVm, referralCommissions);
        }
        return View(projectVm);
    }

    [HttpPost]
    [ProjectVmFilter]
    public IActionResult Edit(ProjectViewModel projectVm)
    {
        ViewBag.Action = "edit";
        
        var savedProject = _projectService.GetProjectById(projectVm.ProjectId);
        
        if (!ModelState.IsValid)
        {
            //re fill fields
            _projectViewModelService.UpdateMissingValues(projectVm, savedProject);
            return View(projectVm);
        }

        _projectService.EditProject(
            savedProject,
            projectVm.Title, 
            projectVm.Articles,
            projectVm.Fundaments,
            projectVm.Summary);
        
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ProjectVmFilter]
    public IActionResult SendToCommission(ProjectViewModel projectVm)
    {
        var savedProject = _projectService.GetProjectById(projectVm.ProjectId);
        
        //en el mundo real, habrían más validaciones
        if (!ModelState.IsValid)
        {
            _projectViewModelService.UpdateMissingValues(projectVm, savedProject);
            ViewBag.Action = "edit";
            return View("Edit", projectVm); //doesnt clear data for on back validation
        }
        
        _projectService.SendToCommissions(savedProject);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost("Projects/Delete/{projectId}")]
    [ProjectIdNotFoundFilter]
    [CanDeleteProjectFilter]
    public IActionResult Delete(int projectId)
    {
        _projectService.DeleteProject(projectId);
        return RedirectToAction(nameof(Index));
    }
}