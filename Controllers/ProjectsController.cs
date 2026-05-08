using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectViewModelService _projectViewModelService;
    private readonly ICommissionService _commissionService;

    public ProjectsController(IProjectService projectService,
        IProjectRepository projectRepository,
        IProjectViewModelService projectViewModelService,
        ICommissionService commissionService)
    {
        _projectService = projectService;
        _projectRepository = projectRepository;
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
        var newProjectVm = _projectViewModelService.NewProjectViewModel();
        return View(newProjectVm);
    }
    
    [HttpPost]
    public IActionResult Add(ProjectViewModel projectVm)
    {
        ViewBag.Action = "add";
        
        if (!ModelState.IsValid)
        {
            // TODO: services empty project
            var emptyProject = new Project
            {
                Articles = "Art.2 ... Art.2 ...",
                State = new()
                {
                    CurrentState = FileState.Scratch,
                    ChangeDate = DateTime.Now
                }
            };
            _projectViewModelService.UpdateMissingValues(projectVm, emptyProject);
            return View(projectVm);
        }

        //TODO: projectService.CreateProject()
        var newProject = _projectViewModelService.ToProject(projectVm);
        _projectRepository.AddNewProject(newProject);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public IActionResult Edit(int projectId)
    {
        ViewBag.Action = "edit";
        var project = _projectRepository.GetProjectById(projectId);
        
        var projectViewModel = _projectViewModelService.ToProjectVm(project);
        if (_commissionService.HasBeenAssigned(projectId))
        {
            var assignedCommissions = _commissionService.GetReferralCommissionsFor(projectId);
            _projectViewModelService.SetCommissions(projectViewModel, assignedCommissions);
        }
        return View(projectViewModel);
    }
    
    [HttpPost]
    public IActionResult Edit(ProjectViewModel projectVm)
    {
        ViewBag.Action = "edit";
        if (!ModelState.IsValid)
        {
            var auxProject = _projectRepository.GetProjectById(projectVm.ProjectId);
            _projectViewModelService.UpdateMissingValues(projectVm, auxProject);
            return View(projectVm);
        }
        var editedProject = _projectViewModelService.ToProject(projectVm);
        //add new state with validation
        _projectRepository.Edit(editedProject);
        return RedirectToAction(nameof(Index));
    }
}