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

    [HttpPost("assignCommissions/{projectId}")]
    public IActionResult AssignCommissions([FromRoute] int projectId)
    {
        var project = _projectRepository.GetProjectById(projectId);
        if (project.State.CurrentState != FileState.PendingForAssignCommissions)
            return BadRequest("No es posible para el estado en que se encuentra.");

        var commissions = _commissionService.EvaluateCommissionFor(project.Articles);
        if (commissions.Count == 0)
            return BadRequest("No se encontraron comisiones adecuadas.");

        var result = _commissionService.AssignCommissionTo(commissions, project.Id);
        project.State.CurrentState = FileState.InCommission;
        project.State.ChangeDate = DateTime.Now;
        _projectRepository.Edit(project);
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
        {
            // TODO: service ... check update method needed
            var project = _projectRepository.GetProjectById(projectId);
            project.State.CurrentState = FileState.RejectedByCommissions;
            project.State.ChangeDate = DateTime.Now;
        }

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

        // TODO: service ... check update method needed
        var project = _projectRepository.GetProjectById(projectId);

        //One validation alternative
        if (project.State.CurrentState != FileState.InCommission &&
            !_commissionService.AcceptedByAllCommission(projectId))
            return BadRequest("No es posible enviar a Sesión.");

        //TODO: do it in a service
        project.State.CurrentState = FileState.InSession;
        project.State.ChangeDate = DateTime.Now;

        return Ok("Projecto en Sesión");
    }

    [HttpPost("DoSession/{projectId}")]
    public IActionResult DoSession(int projectId)
    {
        var project = _projectRepository.GetProjectById(projectId);
        
        if (project.State.CurrentState != FileState.InSession)
            return BadRequest("No es posible finalizar el projecto.");
        
        GetRandomSessionResultFor(project);
        return Ok($"The project has been finalized");
    }

    private void GetRandomSessionResultFor(Project project)
    {
        //todo: do it in a service
        var rnd = new Random();
        var success = rnd.Next(2) == 0;
        if (success)
        {
            project.State.CurrentState = FileState.ApprovedInSession;
            project.State.ChangeDate = DateTime.Now;
        }
        else
        {
            project.State.CurrentState = FileState.RejectedInSession;
            project.State.ChangeDate = DateTime.Now;
        }
    }
}