using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;

namespace WebAppMVC.Controllers;

public class ProjectsController : Controller
{
    private readonly IProjectRepository _projectRepository;

    public ProjectsController(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public IActionResult Index()
    {
        var projects = _projectRepository.GetProjects();
        return View(projects);
    }

    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.Action = "add";
        var newProject = new Project
        {
            State = new ProjectState
            {
                Id = 2, ProjectStateName = "Borrador", ChangeDate = DateTime.Now
            }
        };
        return View(newProject);
    }
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        ViewBag.Action = "edit";
        var project = _projectRepository.GerProjectById(id);
        return View(project);
    }
    
    [HttpPost]
    public IActionResult Edit(Project project)
    {
        ViewBag.Action = "edit";
        if (!ModelState.IsValid)
        {
            var auxProject = _projectRepository.GerProjectById(project.Id);
            if (project.Title.IsNullOrEmpty())
                project.Title = auxProject.Title;
            if (project.Fundaments.IsNullOrEmpty())
                project.Fundaments = auxProject.Fundaments;
            if (project.Articles.IsNullOrEmpty())
                project.Articles = auxProject.Articles;
            if (project.Summary.IsNullOrEmpty())
                project.Summary = auxProject.Summary;
            if (project.State == null)
            {
                project.State = auxProject.State;
            }
            return View(project);
        }
        
        _projectRepository.Update(project);
        return RedirectToAction(nameof(Index));
    }
}