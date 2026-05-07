using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;
using WebAppMVC.Infrastructure.Interfaces;

namespace WebAppMVC.Controllers;

public class ReferralCommitteesController: Controller
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICommissionService _commissionService;
    private readonly ICommissionsVmService _commissionsVmService;

    public ReferralCommitteesController(IProjectRepository projectRepository,
        ICommissionService commissionService,
        ICommissionsVmService commissionsVmService)
    {
        _projectRepository = projectRepository;
        _commissionService = commissionService;
        _commissionsVmService = commissionsVmService;
    }
    
    public IActionResult Index(int projectId)
    {
        var project = _projectRepository.GetProjectById(projectId);
        var referralCommissions = _commissionService.GetReferralCommissionsFor(projectId);
        var result = _commissionsVmService.CreateReferralCommissionsVMs(project,referralCommissions);
        return View(result);
    }
}