using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Services;

public interface IProjectViewModelService
{
    List<ProjectViewModel> ToProjectsVm(List<Project> projects);
    Project ToProject(ProjectViewModel projectVm);
    ProjectViewModel ToProjectVm(Project project);
}