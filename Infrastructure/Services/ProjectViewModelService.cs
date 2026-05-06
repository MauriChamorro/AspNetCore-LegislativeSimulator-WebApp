using Microsoft.IdentityModel.Tokens;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Services;

public class ProjectViewModelService: IProjectViewModelService
{
    private readonly IProjectStateService _projectStateService;

    public ProjectViewModelService(IProjectStateService projectStateService)
    {
        _projectStateService = projectStateService;
    }

    public ProjectViewModel NewProjectViewModel() =>
        new()
        {
            CurrentState = FileState.Scratch,
            StateName = _projectStateService.GetNameState(FileState.Scratch),
            StateDate = DateTime.Now,
            CanEdit =  _projectStateService.CanEdit(_projectStateService.EmptyProject())
        };

    public List<ProjectViewModel> ToProjectsVm(List<Project> projects)
    {
        var projectsVm = new List<ProjectViewModel>();
        foreach (var project in projects)
            projectsVm.Add(ToProjectVm(project));
        return projectsVm;
    }
    
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
            projectVm.StateName = _projectStateService.GetNameState(auxProject.State.CurrentState);
            projectVm.CurrentState = auxProject.State.CurrentState;
            projectVm.StateDate = auxProject.State.ChangeDate;
            projectVm.CanEdit = _projectStateService.CanEdit(auxProject.State);
        }
    }

    public Project ToProject(ProjectViewModel projectVm) =>
        new()
        {
            Id =  projectVm.ProjectId,
            Title = projectVm.Title,
            Articles = projectVm.Articles,
            Fundaments = projectVm.Fundaments,
            Summary = projectVm.Summary,
            //TODO: builder para State??
            State = new ProjectState
            {
                CurrentState = projectVm.CurrentState,
                ChangeDate =  projectVm.StateDate
            }
        };

    public ProjectViewModel ToProjectVm(Project project) =>
        new()
        {
            ProjectId = project.Id,
            Title = project.Title,
            Articles = project.Articles,
            Fundaments = project.Fundaments,
            Summary = project.Summary,
            StateName = _projectStateService.GetNameState(project.State.CurrentState),
            StateDate = project.State.ChangeDate,
            CanEdit = _projectStateService.CanEdit(project.State)
        };

    public void SetCommissions(ProjectViewModel projectViewModel, AssignedCommissions assignedCommissions)
    {
        projectViewModel.Commissions = new List<CommissionViewModel>();
        foreach (var assignedCommission in assignedCommissions.Commissions)
        {
            projectViewModel.Commissions.Add(
                new()
                {
                    Name = assignedCommission.Name,
                    Color = "bg-info"
                }
            );
        }
    }
}