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
    public IActionResult Add(int id)
    {
        var project = _projectRepository.GerProjectById(id);
        return View(project);
    }
    
    [HttpPost]
    public IActionResult Add(Project project)
    {
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
                project.ProjectStateId = auxProject.ProjectStateId;
                project.State = auxProject.State;
            }
            return View(project);
        }
        
        _projectRepository.Update(project);
        return RedirectToAction(nameof(Index));
    }
}