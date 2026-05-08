using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface IProjectViewModelService
{
    ProjectViewModel CreateEmptyProjectVm(Project emptyProject);
    List<ProjectViewModel> ToProjectsVm(List<Project> projects);
    void UpdateMissingValues(ProjectViewModel projectVm, Project auxProject);
    Project ToProject(ProjectViewModel projectVm);
    ProjectViewModel ToProjectVm(Project project);
    void SetCommissions(ProjectViewModel projectViewModel, List<ReferralCommission> assignedCommissions);
}