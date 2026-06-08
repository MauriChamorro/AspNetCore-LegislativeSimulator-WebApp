using Microsoft.IdentityModel.Tokens;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Services;

public class ProjectViewModelService: IProjectViewModelService
{

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
            CommissionsAssigned = project.AreCommissionsAssigned()
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
        projectVm.CanEdit = auxProject.CanEdit();
        projectVm.IsEdit =  auxProject.IsEdit();
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