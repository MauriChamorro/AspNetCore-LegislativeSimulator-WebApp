using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Services;
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
        var newProject = new ProjectViewModel
        {
            StateName = "Borrador",
            StateDate = DateTime.Now
        };
        return View(newProject);
    }
    
    [HttpPost]
    public IActionResult Add(ProjectViewModel projectVm)
    {
        ViewBag.Action = "add";
        
        if (!ModelState.IsValid)
        {
            var emptyProject = new Project
            {
                Articles = "Art.2 ... Art.2 ...",
                State = new ProjectState { Id = 1, ProjectStateName = "Borrador", ChangeDate = DateTime.Now }
            };
            UpdateMissingValues(projectVm, emptyProject);
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
            UpdateMissingValues(projectVm, auxProject);
            return View(projectVm);
        }
        var editedProject = _projectViewModelService.ToProject(projectVm);
        _projectRepository.Update(editedProject);
        return RedirectToAction(nameof(Index));
    }

    private void UpdateMissingValues(ProjectViewModel projectVm, Project auxProject)
    {
        if (projectVm.Title.IsNullOrEmpty())
            projectVm.Title = auxProject.Title;
        if (projectVm.Fundaments.IsNullOrEmpty())
            projectVm.Fundaments = auxProject.Fundaments;
        if (projectVm.Articles.IsNullOrEmpty())
            projectVm.Articles = auxProject.Articles;
        if (projectVm.Summary.IsNullOrEmpty())
            projectVm.Summary = auxProject.Summary;
        if (projectVm.StateName.IsNullOrEmpty())
        {
            projectVm.StateName = auxProject.State.ProjectStateName;
            projectVm.StateDate = auxProject.State.ChangeDate;
        }
    }
}