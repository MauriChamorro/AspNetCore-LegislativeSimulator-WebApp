using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Services;
using WebAppMVC.Infrastructure.Interfaces;

namespace WebAppMVC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimulationController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ICommissionService _commissionService;
    private readonly INotificationService _notificationService;

    public SimulationController(IProjectService projectService,
        ICommissionService commissionService,
        INotificationService notificationService)
    {
        _projectService = projectService;
        _commissionService = commissionService;
        _notificationService = notificationService;
    }

    [HttpPost("assignCommissions/{projectId}")]
    public IActionResult AssignCommissions([FromRoute] int projectId)
    {
        var project = _projectService.GetProjectById(projectId);
        if (project.State.CurrentState != FileState.PendingForAssignCommissions)
            return BadRequest("No es posible para el estado en que se encuentra.");

        var commissions = _commissionService.EvaluateCommissionFor(project.Articles);
        if (commissions.Count == 0)
            return BadRequest("No se encontraron comisiones adecuadas.");

        var result = _commissionService.AssignCommissionTo(commissions, project.Id);
        project.State.CurrentState = FileState.InCommission;
        project.State.ChangeDate = DateTime.Now;
        _notificationService.AddCommissionAssignedNotification(projectId);
        return Ok(result);
    }

    [HttpGet("assignedCommissions/{projectId}")]
    public IActionResult AssignedCommissions([FromRoute] int projectId)
    {
        if (!_commissionService.HasBeenAssigned(projectId))
            return BadRequest("El projecto no tiene comisiones asignadas.");
        var result = _commissionService.GetReferralCommissionsFor(projectId);
        return Ok(result);
    }

    [HttpPost("doReferring/{projectId}")]
    public IActionResult DoReferring(int projectId)
    {
        if (!_commissionService.HasBeenAssigned(projectId))
            return BadRequest("El projecto no tiene comisiones asignadas.");
        
        var referralCommissions = _commissionService.GetReferralCommissionsFor(projectId);
        if (_commissionService.ThereAreNotPendingReferral(referralCommissions))
            return BadRequest("Todas la comisiones ya evaluaron.");
        var actualReferral = _commissionService.GetActualReferral(referralCommissions);
        _commissionService.DoNextReferralPhase(actualReferral);
        
        if (_commissionService.ReferralIsRejected(actualReferral))
            _projectService.RejectProjectByCommissions(projectId);

        return Ok(actualReferral);
    }

    [HttpPost("sendToSession/{projectId}")]
    public IActionResult SendToSession(int projectId)
    {
        if (!_commissionService.HasBeenAssigned(projectId))
            return BadRequest("El projecto no tiene comisiones asignadas.");
        var referralCommissions = _commissionService.GetReferralCommissionsFor(projectId);

        if (!_commissionService.ThereAreNotPendingReferral(referralCommissions))
            return BadRequest("Todas las comisiones deben terminar de evaluar.");

        if (!_commissionService.AcceptedByAllCommission(projectId))
            return BadRequest("El projecto debe ser aprobado por todas las commisiones.");

        var project = _projectService.GetProjectById(projectId);

        //One validation alternative
        if (project.State.CurrentState != FileState.InCommission &&
            !_commissionService.AcceptedByAllCommission(projectId))
            return BadRequest("No es posible enviar a Sesión.");

        _projectService.SendToSession(projectId);

        return Ok("Projecto en Sesión");
    }

    [HttpPost("DoSession/{projectId}")]
    public IActionResult DoSession(int projectId)
    {
        var project = _projectService.GetProjectById(projectId);
        
        if (project.State.CurrentState != FileState.InSession)
            return BadRequest("No es posible finalizar el projecto.");

        _projectService.SimulateSessionResult(project);
        return Ok($"Resultado de Sesión: {project.State.GetNameState(project.State.CurrentState)}");
    }
}