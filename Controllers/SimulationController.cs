using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Models.Projects;
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
        var project = _projectRepository.GetProjectById(projectId);
        var commissions = _commissionService.EvaluateCommissionFor(project.Articles);
        var result = _commissionService.AssignCommissionTo(commissions, project.Id);
        project.State.CurrentState = FileState.InCommission;
        project.State.ChangeDate = DateTime.Now;
        _projectRepository.Update(project);
        return Ok(result);;
    }
    
    [HttpGet("AssignedCommissions/{projectId}")]
    public IActionResult AssignedCommissions([FromRoute] int projectId)
    {
        var result = _commissionService.GetAssignedCommissionsFor(projectId);
        //change project state
        return Ok(result);;
    }
}