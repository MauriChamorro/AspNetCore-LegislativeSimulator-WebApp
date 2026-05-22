using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public Task<List<Project>> GetProjects() => _projectRepository.GetProjects();

    public async Task<Project> BuildEmptyProject()
    {
        return new Project
        {
            StateHistory =
            [
                new ProjectStateHistory
                {
                    Date = DateTime.Now,
                    ProjectState = await GetScratchProjectState()
                }
            ]
        };
    }

    private async Task<ProjectState> GetScratchProjectState() => 
        await _projectRepository.GetScratchProjectState();

    public async Task CreateNewProject(string title, string articles, string fundaments, string summary)
    {

        var history = new List<ProjectStateHistory>();
        history.Add( new ProjectStateHistory
            {
                Date = DateTime.Now,
                ProjectState = await GetScratchProjectState()
            });
            
        var newProject = new Project
        {
            FileId = "alphabetic-id",
            Title = title,
            Articles = articles,
            Fundaments = fundaments,
            Summary = summary,
            StateHistory = history
        };
        
       await _projectRepository.Add(newProject);
    }

    public async Task<Project> GetProjectByIdAsync(int projectId)
    {
        var projects = await _projectRepository.GetProjects();
        //TODO: _projectRepository.Get(projectId);
        return projects.First(p => p.ProjectId == projectId);
    }

    public Task<bool> ExistProjectAsync(int projectId) =>
        _projectRepository.Exists(projectId);

    public async Task UpdateProject(Project project, string title, string articles, string fundaments, string summary)
    {
        project.Title = title;
        project.Articles = articles;
        project.Fundaments = fundaments;
        project.Summary = summary;

        await _projectRepository.UpdateProject(project);
    }

    public async Task RejectProjectByCommissions(int projectId)
    {
        var rejectedState = new ProjectStateHistory
        {
            Date = DateTime.Now,
            ProjectState = await _projectRepository.GetRejectedByCommissionProjectState()
        };
        await _projectRepository.AddStateHistory(projectId, rejectedState);
    }

    public async Task SendToSession(Project project)
    {
        var projectStateHistory = new ProjectStateHistory
        {
            ProjectState = await _projectRepository.GetInSessionProjectState(),
            Date = DateTime.Now,
        };

        await _projectRepository.AddStateHistory(project.ProjectId, projectStateHistory);
    }

    public async Task<bool> DoSession(Project project)
    {
        var rnd = new Random();
        var success = rnd.Next(2) == 0;

        var projectStateHistory = new ProjectStateHistory();
        
        if (success)
        {
            projectStateHistory.ProjectState = await _projectRepository.GetApprovedInSessionProjectState();
            projectStateHistory.Date = DateTime.Now;
        }
        else
        {
            projectStateHistory.ProjectState = await _projectRepository.GetRejectedInSessionProjectState();
            projectStateHistory.Date = DateTime.Now;
        }

        await _projectRepository.AddStateHistory(project.ProjectId, projectStateHistory);
        
        return projectStateHistory.ProjectState.State == FileState.ApprovedInSession;
    }

    public bool CanSendToCommission(Project project) =>
        project.GetCurrentState().ProjectState.State == FileState.Scratch;

    public async Task SetPendingForCommissionsFor(Project project)
    {
        var projectStateHistory = new ProjectStateHistory
        {
            ProjectState = await _projectRepository.GetSentToCommissionProjectState(),
            Date = DateTime.Now,
        };

        await _projectRepository.AddStateHistory(project.ProjectId, projectStateHistory);
    }

    public void DeleteProject(int projectId) => 
        _projectRepository.Delete(projectId);

    public async Task<bool> CanDelete(int projectId)
    {
        //TODO: no usarlo en excepciones
        var project = await GetProjectByIdAsync(projectId);
        return project.GetCurrentState().ProjectState.State == FileState.Scratch;
    }

    public async Task SetInCommissionFor(int projectId)
    {
        var projectStateHistory = new ProjectStateHistory
        {
            ProjectState = await _projectRepository.GetInCommissionProjectState(),
            Date = DateTime.Now
        };

        await _projectRepository.AddStateHistory(projectId, projectStateHistory);
    }

    public bool CanAssignCommissions(Project project) => 
        project.GetCurrentState().ProjectState.State == FileState.PendingForAssignCommissions;

    public bool IsInCommission(Project project) => 
        project.GetCurrentState().ProjectState.State == FileState.InCommission;

    public bool IsInSession(Project project) => 
        project.GetCurrentState().ProjectState.State == FileState.InSession;
}