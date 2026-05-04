using Microsoft.AspNetCore.Mvc;
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
            return View(auxProject);
        }

        return Index();
    }
}