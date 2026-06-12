using Microsoft.IdentityModel.Tokens;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Services;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Services;

public class ProjectViewModelService : IProjectViewModelService
{
    private readonly IProjectService _projectService;

    public ProjectViewModelService(IProjectService projectService)
    {
        _projectService = projectService;
    }
    
    public List<ProjectViewModel> ToProjectsVm(List<Project> projects)
    {
        var projectsVm = new List<ProjectViewModel>();
        foreach (var project in projects)
            projectsVm.Add(ToProjectVm(project));
        return projectsVm;
    }

    public ProjectViewModel ToProjectVm(Project project) =>
        new()
        {
            ProjectId = project.ProjectId,
            Title = project.Title,
            Articles = project.Articles,
            Fundaments = project.Fundaments,
            Summary = project.Summary,
            StateName = project.GetCurrentState().ProjectState.Name,
            StateDate = project.GetCurrentState().Date,
            CanEdit = project.CanEdit(),
            IsEdit = project.IsEdit(),
            HasCommissions = project.HasCommissions(),
            CanSendToSession = _projectService.CanSendToSession(project).Result
        };

    public void UpdateMissingValues(ProjectViewModel projectVm, Project auxProject)
    {
        if (projectVm.Title.IsNullOrEmpty())
            projectVm.Title = auxProject.Title;
        if (projectVm.Fundaments.IsNullOrEmpty())
            projectVm.Fundaments = auxProject.Fundaments;
        if (projectVm.Articles.IsNullOrEmpty())
            projectVm.Articles = auxProject.Articles;
        if (projectVm.Summary.IsNullOrEmpty())
            projectVm.Summary = auxProject.Summary;

        projectVm.StateName = auxProject.GetCurrentState().ProjectState.Name;
        projectVm.CurrentState = auxProject.GetCurrentState().ProjectState.State;
        projectVm.StateDate = auxProject.GetCurrentState().Date;
        projectVm.HasCommissions = auxProject.HasCommissions();
        projectVm.CanEdit = auxProject.CanEdit();
        projectVm.IsEdit = auxProject.IsEdit();
        projectVm.CanSendToSession = _projectService.CanSendToSession(auxProject).Result;
    }

    public void SetCommissionVmsToProjectVm(ProjectViewModel projectVm, List<Referral> referralCommissions)
    {
        projectVm.Commissions = new List<CommissionViewModel>();
        foreach (var referralCommission in referralCommissions)
        {
            projectVm.Commissions.Add(
                new()
                {
                    Name = referralCommission.Commission.Name,
                    Color = "bg-info"
                }
            );
        }
    }
}