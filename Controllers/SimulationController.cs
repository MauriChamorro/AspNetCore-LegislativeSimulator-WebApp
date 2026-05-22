using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Services;
using WebAppMVC.Filters.ActionFilters;
using WebAppMVC.Filters.ActionFilters.Async;
using WebAppMVC.Infrastructure.Interfaces;

namespace WebAppMVC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimulationController : ControllerBase
{
    private readonly ICommissionService _commissionService;
    private readonly INotificationService _notificationService;
    private readonly IProjectService _projectService;

    public SimulationController(IProjectService projectService,
        ICommissionService commissionService,
        INotificationService notificationService)
    {
        _projectService = projectService;
        _commissionService = commissionService;
        _notificationService = notificationService;
    }

    [HttpPost("AssignCommissions/{projectId}")]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public async Task<IActionResult> AssignCommissions([FromRoute] int projectId)
    {
        var project = await _projectService.GetProjectByIdAsync(projectId);
        
        if (!_projectService.CanAssignCommissions(project))
            return BadRequest("No es posible para el estado en que se encuentra.");

        var commissions = _commissionService.EvaluateCommissionFor(project.Articles);
        if (commissions.Count == 0)
        {
            _projectService.RejectProjectByCommissions(projectId);
            _notificationService.AddProjectStateChangedNotification(projectId);
            return BadRequest("No se encontraron comisiones adecuadas.");
        }

        var result = _commissionService.AssignCommissionTo(commissions, project.ProjectId);
        _projectService.SetInCommissionFor(projectId);
        _notificationService.AddCommissionAssignedNotification(projectId);
        return Ok(result);
    }

    [HttpGet("AssignedCommissions/{projectId}")]
    public IActionResult AssignedCommissions([FromRoute] int projectId)
    {
        if (!_commissionService.HasBeenAssigned(projectId))
            return BadRequest("El proyecto no tiene comisiones asignadas.");
        var result = _commissionService.GetReferralsFor(projectId);
        return Ok(result);
    }

    [HttpPost("DoReferring/{projectId}")]
    public IActionResult DoReferring(int projectId)
    {
        if (!_commissionService.HasBeenAssigned(projectId))
            return BadRequest("El proyecto no tiene comisiones asignadas.");

        var referrals = _commissionService.GetReferralsFor(projectId);
        if (_commissionService.AllCommissionEvaluated(referrals))
            return BadRequest("Todas la comisiones ya evaluaron.");
        
        if (_commissionService.AlreadyRejected(referrals))
            return BadRequest("El proyecto ya fue rechazado.");
        
        var actualReferral = _commissionService.GetActualReferralFor(referrals);
        _commissionService.DoNextReferralPhase(actualReferral);
        if (_commissionService.IsRejectedReferral(actualReferral))
            _projectService.RejectProjectByCommissions(projectId);

        _notificationService.AddReferralChangeNotification(projectId);

        return Ok(actualReferral);
    }

    [HttpPost("SendToSession/{projectId}")]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public async Task<IActionResult> SendToSession(int projectId)
    {
        if (!_commissionService.HasBeenAssigned(projectId))
            return BadRequest("El proyecto no tiene comisiones asignadas.");
        
        var project = await _projectService.GetProjectByIdAsync(projectId);
        if(!_projectService.IsInCommission(project))
            return BadRequest("El proyecto debe estar en Comisión.");

        var referrals = _commissionService.GetReferralsFor(projectId);

        if (!_commissionService.AllCommissionEvaluated(referrals))
            return BadRequest("Todas las comisiones deben terminar de evaluar.");

        if (!_commissionService.AcceptedByAllCommission(referrals))
            return BadRequest("El proyecto debe ser aprobado por todas las commisiones.");

        _projectService.SendToSession(project);
        _notificationService.AddSendToSessionNotification(project);
        return Ok("Proyecto en Sesión");
    }

    [HttpPost("DoSession/{projectId}")]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public async Task<IActionResult> DoSession(int projectId)
    {
        var project = await _projectService.GetProjectByIdAsync(projectId);

        if(!_projectService.IsInSession(project))
            return BadRequest("No es posible finalizar el proyecto.");

        var result = _projectService.DoSession(project);

        _notificationService.AddSessionResultNotification(project, result);

        return Ok($"Resultado de Sesión: {project.GetCurrentState().ProjectState.Name}");
    }
}