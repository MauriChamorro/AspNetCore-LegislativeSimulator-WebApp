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
            ProjectId = project.Id,
            Title = project.Title,
            Articles = project.Articles,
            Fundaments = project.Fundaments,
            Summary = project.Summary,
            StateName = project.State.GetNameState(project.State.CurrentState),
            StateDate = project.State.ChangeDate,
            CanEdit = project.CanEdit(),
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
        if (projectVm.StateName.IsNullOrEmpty())
        {
            projectVm.StateName = auxProject.State.GetNameState(auxProject.State.CurrentState);
            projectVm.CurrentState = auxProject.State.CurrentState;
            projectVm.StateDate = auxProject.State.ChangeDate;
            projectVm.CanEdit = auxProject.CanEdit();
        }
    }

    public void SetVmCommissions(ProjectViewModel projectVm, List<ReferralCommission> referralCommissions)
    {
        projectVm.Commissions = new List<CommissionViewModel>();
        foreach (var referralCommission in referralCommissions)
        {
            projectVm.Commissions.Add(
                new()
                {
                    Name = referralCommission.CommisionName,
                    Color = "bg-info"
                }
            );
        }
    }
}