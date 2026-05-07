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
        if (project.State.CurrentState != FileState.InPendingCommissions)
            return BadRequest("No es posible para el estado en que se encuentra");
        
        var commissions = _commissionService.EvaluateCommissionFor(project.Articles);
        if (commissions.Count == 0)
            return BadRequest("No se encontraron comisiones adecuadas");
        
        var result = _commissionService.AssignCommissionTo(commissions, project.Id);
        project.State.CurrentState = FileState.InCommission;
        project.State.ChangeDate = DateTime.Now;
        _projectRepository.UpdateByEdit(project);
        return Ok(result);
    }
    
    [HttpGet("AssignedCommissions/{projectId}")]
    public IActionResult AssignedCommissions([FromRoute] int projectId)
    {
        if (!_commissionService.HasBeenAssigned(projectId))
            return BadRequest("El projecto no tiene commisiones asignadas");
        var result = _commissionService.GetAssignedCommissionsFor(projectId);
        return Ok(result);
    }

    [HttpPost("StartReferring/{projectId}")]
    public IActionResult StartReferring(int projectId)
    {
        if (!_commissionService.HasBeenAssigned(projectId))
            return BadRequest("El projecto no tiene commisiones asignadas");
        var referralCommissions = _commissionService.GetReferralCommissionsFor(projectId);
        var actualReferral = _commissionService.GetActualReferral(referralCommissions);
        _commissionService.DoNextReferralPhase(actualReferral);
        return Ok(actualReferral);
    } 
}