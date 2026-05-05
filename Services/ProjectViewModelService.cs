using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Services;

public class ProjectViewModelService: IProjectViewModelService
{
    public List<ProjectViewModel> ToProjectsVm(List<Project> projects)
    {
        var projectsVm = new List<ProjectViewModel>();
        foreach (var project in projects)
            projectsVm.Add(ToProjectVm(project));
        return projectsVm;
    }

    public Project ToProject(ProjectViewModel projectVm)
    {
        return new Project
        {
            Id =  projectVm.ProjectId,
            Title = projectVm.Title,
            Articles = projectVm.Articles,
            Fundaments = projectVm.Fundaments,
            Summary = projectVm.Summary,
            //TODO: builder para State
            State = new ProjectState
            {
                Id = GetStateIdByName(projectVm.StateName),
                ProjectStateName =  projectVm.StateName,
                ChangeDate =  projectVm.StateDate
            }
        };
    }

    public ProjectViewModel ToProjectVm(Project project)
    {
        return new ProjectViewModel
        {
            ProjectId = project.Id,
            Title = project.Title,
            Articles = project.Articles,
            Fundaments = project.Fundaments,
            Summary = project.Summary,
            StateName = project.State.ProjectStateName,
            StateDate = project.State.ChangeDate
        };
    }

    private int GetStateIdByName(string stateName)
    {
        switch (stateName)
        {
            case "Borrador":
                return 1;
            case "En Comissión":
                return 2;
            default:
                return 1;
        }
    }
}