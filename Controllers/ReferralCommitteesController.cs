using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;
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
    
    public IActionResult Index(int projectId)
    {
        var project = _projectService.GetProjectById(projectId);
        var referralCommissions = _commissionService.GetReferralCommissionsFor(projectId);
        var result = _commissionsVmService.CreateReferralCommissionsVMs(project,referralCommissions);
        return View(result);
    }
}