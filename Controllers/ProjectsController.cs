using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IProjectViewModelService _projectViewModelService;
    private readonly ICommissionService _commissionService;

    public ProjectsController(IProjectService projectService,
        IProjectViewModelService projectViewModelService,
        ICommissionService commissionService)
    {
        _projectService = projectService;
        _projectViewModelService = projectViewModelService;
        _commissionService = commissionService;
    }

    public IActionResult Index()
    {
        var projects = _projectService.GetProjects();
        var projectVms = _projectViewModelService.ToProjectsVm(projects);
        return View(projectVms);
    }

    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.Action = "add";
        var emptyProject = _projectService.CreatEmptyProject();
        var newProjectVm = _projectViewModelService.CreateEmptyProjectVm(emptyProject);
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
    
    [HttpGet]
    public IActionResult Edit(int projectId)
    {
        ViewBag.Action = "edit";
        var project = _projectService.GetProjectById(projectId);
        
        var projectVm = _projectViewModelService.ToProjectVm(project);
        if (_commissionService.HasBeenAssigned(projectId))
        {
            var referralCommissions = _commissionService.GetReferralCommissionsFor(projectId);
            _projectViewModelService.SetCommissions(projectVm, referralCommissions);
        }
        return View(projectVm);
    }

    [HttpPost]
    public IActionResult Edit(ProjectViewModel projectVm)
    {
        ViewBag.Action = "edit";
        //TODO: id exist validation
        //TODO: Validation: cannot edit in different to scratch
        if (!ModelState.IsValid)
        {
            if (!_projectService.ExistProject(projectVm.ProjectId))
            {
                ViewBag.ErrorMessage = "El proyecto no existe";
                return View("Error");
            }

            var auxProject = _projectService.GetProjectById(projectVm.ProjectId);
            _projectViewModelService.UpdateMissingValues(projectVm, auxProject);
            return View(projectVm);
        }

        _projectService.EditProject(
            projectVm.ProjectId,
            projectVm.Title, 
            projectVm.Articles,
            projectVm.Fundaments,
            projectVm.Summary);
        
        return RedirectToAction(nameof(Index));
    }
}