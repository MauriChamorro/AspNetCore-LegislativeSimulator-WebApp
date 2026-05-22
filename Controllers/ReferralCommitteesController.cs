using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Services;
using WebAppMVC.Filters.ActionFilters;
using WebAppMVC.Filters.ActionFilters.Async;
using WebAppMVC.Infrastructure.Interfaces;

namespace WebAppMVC.Controllers;

public class ReferralCommitteesController: Controller
{
    private readonly IProjectService _projectService;
    private readonly ICommissionService _commissionService;
    private readonly ICommissionsVmService _commissionsVmService;

    public ReferralCommitteesController(IProjectService projectService,
        ICommissionService commissionService,
        ICommissionsVmService commissionsVmService)
    {
        _projectService = projectService;
        _commissionService = commissionService;
        _commissionsVmService = commissionsVmService;
    }
    
    [HttpGet("ReferralCommittees/{projectId}")]
    [ServiceFilter(typeof(ProjectIdNotFoundAsyncFilterAttribute))]
    [ServiceFilter(typeof(ReferralCommitteesAsyncFilterAttribute))]
    public async Task<IActionResult> Index(int projectId)
    {
        var project = await _projectService.GetProjectByIdAsync(projectId);
        var referralCommissions = await _commissionService.GetReferralsFor(projectId);
        var result = _commissionsVmService.CreateReferralCommissionsVMs(project,referralCommissions);
        return View(result);
    }
}