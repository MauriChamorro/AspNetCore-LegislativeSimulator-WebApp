using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Services;
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

        var commissions = await _commissionService.EvaluateCommissionFor(project.Articles);
        if (commissions.Count == 0)
        {
            await _projectService.RejectProjectByCommissions(projectId);
            await _notificationService.AddProjectStateChangedNotification(projectId);
            return BadRequest("No se encontraron comisiones adecuadas.");
        }

        //todo: check if can parallelize method
        var assignedReferrals = await _commissionService.AssignCommissionTo(commissions, project.ProjectId);
        await _projectService.SetInCommissionFor(projectId);
        await _notificationService.AddCommissionAssignedNotification(projectId);
        return Ok(assignedReferrals);
    }

    [HttpGet("AssignedCommissions/{projectId}")]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public async Task<IActionResult> AssignedCommissions([FromRoute] int projectId)
    {
        var hasBeenAssigned = await _commissionService.HasBeenAssigned(projectId);

        if (!hasBeenAssigned)
            return BadRequest("El proyecto no tiene comisiones asignadas.");
        var result = await _commissionService.GetReferralsFor(projectId);
        return Ok(result);
    }

    [HttpPost("EditReferral/{projectId}")]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public async Task<IActionResult> EditReferral(int projectId, [FromQuery] int commissionId, [FromQuery] int referralState)
    {
        var hasBeenAssigned = await _commissionService.HasBeenAssigned(projectId);

        if (!hasBeenAssigned)
            return BadRequest("El proyecto no tiene comisiones asignadas.");

        var referrals = await _commissionService.GetReferralsFor(projectId);

        if (!referrals.Exists(r => r.CommissionId == commissionId))
            return BadRequest("El proyecto no tiene la comisión que estas buscando.");

        if (!Enum.IsDefined(typeof(ReferralState), referralState))
            return BadRequest("El Estado de Giro no es válido.");

        var referral = referrals.Find(r => r.ProjectId == projectId && r.CommissionId == commissionId);
        referral!.State = (ReferralState)referralState;
        
        await _commissionService.UpdateReferral(referral);
        await _notificationService.AddReferralChangeNotification(projectId);

        return Ok(referral);
    }
    
    [HttpPost("DoNextReferringRandomly/{projectId}")]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public async Task<IActionResult> DoRandomReferring(int projectId)
    {
        var hasBeenAssigned = await _commissionService.HasBeenAssigned(projectId);
        if (!hasBeenAssigned)
            return BadRequest("El proyecto no tiene comisiones asignadas.");

        var referrals = await _commissionService.GetReferralsFor(projectId);
        if (_commissionService.AllCommissionEvaluated(referrals))
            return BadRequest("Todas la comisiones ya evaluaron.");

        if (_commissionService.AlreadyRejected(referrals))
            return BadRequest("El proyecto ya fue rechazado.");

        var actualReferral = _commissionService.GetActualReferralFor(referrals);
        await _commissionService.DoNextReferralPhase(actualReferral);
        if (_commissionService.IsRejectedReferral(actualReferral))
            await _projectService.RejectProjectByCommissions(projectId);

        await _notificationService.AddReferralChangeNotification(projectId);

        return Ok(actualReferral);
    }

    [HttpPost("DoSession/{projectId}")]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    public async Task<IActionResult> DoSession(int projectId)
    {
        var project = await _projectService.GetProjectByIdAsync(projectId);

        if (!_projectService.IsInSession(project))
            return BadRequest("No es posible finalizar el proyecto.");

        var sessionResult = await _projectService.DoSession(project);

        _notificationService.AddSessionResultNotification(project, sessionResult);

        return Ok($"Resultado de Sesión: {GetResultText(sessionResult)}");
    }

    private string GetResultText(bool sessionResult) =>
        sessionResult ? "Aprobado" : "Rechazado";
}