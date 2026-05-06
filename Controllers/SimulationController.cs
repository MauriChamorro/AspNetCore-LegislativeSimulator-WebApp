using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimulationController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICommissionService _commissionService;

    public SimulationController(IProjectRepository projectRepository, ICommissionService commissionService)
    {
        _projectRepository = projectRepository;
        _commissionService = commissionService;
    }
    
    [HttpPost("AssignCommissions/{projectId}")]
    public IActionResult AssignCommissions([FromRoute] int projectId)
    {
        //search project
        var project = _projectRepository.GetProjectById(projectId);
        var result = _commissionService.EvaluateCommissionFor(project.Articles);
        //assign commissions logic (project properties)
        //assign commissions to project
        //change project state
        return Ok(result);;
    }
}