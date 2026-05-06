using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Services.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Controllers;

public class ProjectsController : Controller
{
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectViewModelService _projectViewModelService;

    public ProjectsController(IProjectRepository projectRepository, IProjectViewModelService projectViewModelService)
    {
        _projectRepository = projectRepository;
        _projectViewModelService = projectViewModelService;
    }

    public IActionResult Index()
    {
        var projects = _projectRepository.GetProjects();
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

        var newProject = _projectViewModelService.ToProject(projectVm);
        _projectRepository.AddNewProject(newProject);
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public IActionResult Edit(int projectId)
    {
        ViewBag.Action = "edit";
        var project = _projectRepository.GerProjectById(projectId);
        return View(_projectViewModelService.ToProjectVm(project));
    }
    
    [HttpPost]
    public IActionResult Edit(ProjectViewModel projectVm)
    {
        ViewBag.Action = "edit";
        if (!ModelState.IsValid)
        {
            var auxProject = _projectRepository.GerProjectById(projectVm.ProjectId);
            _projectViewModelService.UpdateMissingValues(projectVm, auxProject);
            return View(projectVm);
        }
        var editedProject = _projectViewModelService.ToProject(projectVm);
        //add new state with validation
        _projectRepository.Update(editedProject);
        return RedirectToAction(nameof(Index));
    }
}