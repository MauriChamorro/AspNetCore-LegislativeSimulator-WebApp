using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Services;

public class ProjectViewModelService: IProjectViewModelService
{
    public List<ProjectViewModel> ToProjectsVm(List<Project> projects)
    {
        var projectsVm = new List<ProjectViewModel>();
        foreach (var project in projects)
        {
            projectsVm.Add(
                new  ProjectViewModel
                {
                    ProjectId = project.Id,
                    Title = project.Title,
                    Articles = project.Articles,
                    Fundaments = project.Fundaments,
                    Summary = project.Summary,
                    StateName = project.State.ProjectStateName,
                    StateDate = project.State.ChangeDate
                }
            );
        }
        
        return projectsVm;
    }
}