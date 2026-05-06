using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Services.Interfaces;

public interface IProjectViewModelService
{
    ProjectViewModel NewProjectViewModel();
    List<ProjectViewModel> ToProjectsVm(List<Project> projects);
    void UpdateMissingValues(ProjectViewModel projectVm, Project auxProject);
    Project ToProject(ProjectViewModel projectVm);
    ProjectViewModel ToProjectVm(Project project);
}