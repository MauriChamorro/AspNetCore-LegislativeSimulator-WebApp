using System.Collections.Generic;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface IProjectViewModelService
{
    List<ProjectViewModel> ToProjectsVm(List<Project> projects);
    void UpdateMissingValues(ProjectViewModel projectVm, Project auxProject);
    ProjectViewModel ToProjectVm(Project project);
    void SetCommissionVmsToProjectVm(ProjectViewModel projectVm, List<Referral> referralCommissions);
}