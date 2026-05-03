using Microsoft.AspNetCore.Mvc;
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
        return View();
    }
}