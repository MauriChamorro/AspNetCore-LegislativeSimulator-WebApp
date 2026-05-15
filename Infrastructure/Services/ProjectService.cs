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

    public List<Project> GetProjects() => _projectRepository.GetProjects();

    public Project CreatEmptyProject()
    {
        return new Project
        {
            StateHistory = new()
            {
                ProjectState = new ProjectState
                {
                    State =  FileState.Scratch
                },
                Date = DateTime.Now
            }
        };
    }

    public void CreateNewProject(string title, string articles, string fundaments, string summary)
    {
        var lastId = _projectRepository.GetLastId();
        var newProject = new Project
        {
            ProjectId = lastId + 1,
            Title = title,
            Articles = articles,
            Fundaments = fundaments,
            Summary = summary,
            StateHistory = new()
            {
                ProjectState = new ProjectState
                {
                    State =  FileState.Scratch
                },
                Date = DateTime.Now
            }
        };
        _projectRepository.Add(newProject);
    }

    public Project GetProjectById(int projectId) =>
        _projectRepository.GetProjects().First(p => p.ProjectId == projectId);

    public bool ExistProject(int projectId) =>
        _projectRepository.GetProjects().Exists(p => p.ProjectId == projectId);

    public void EditProject(Project project, string title, string articles, string fundaments, string summary)
    {
        project.Title = title;
        project.Articles = articles;
        project.Fundaments = fundaments;
        project.Summary = summary;
    }

    public void RejectProjectByCommissions(int projectId)
    {
        var project = GetProjectById(projectId);
        project.StateHistory.ProjectState.State = FileState.RejectedByCommissions;
        project.StateHistory.Date = DateTime.Now;
    }

    public void SendToSession(int projectId)
    {
        var project =  GetProjectById(projectId);
        project.StateHistory.ProjectState.State = FileState.InSession;
        project.StateHistory.Date = DateTime.Now;
    }

    public bool SimulateSessionResult(Project project)
    {
        var rnd = new Random();
        var success = rnd.Next(2) == 0;
        if (success)
        {
            project.StateHistory.ProjectState.State = FileState.ApprovedInSession;
            project.StateHistory.Date = DateTime.Now;
        }
        else
        {
            project.StateHistory.ProjectState.State = FileState.RejectedInSession;
            project.StateHistory.Date = DateTime.Now;
        }

        return project.StateHistory.ProjectState.State == FileState.ApprovedInSession;
    }

    public bool CanSendToCommission(Project project) =>
        project.StateHistory.ProjectState.State == FileState.Scratch;

    public void PendingForCommissions(Project project)
    {
        project.StateHistory.ProjectState.State = FileState.PendingForAssignCommissions;
        project.StateHistory.Date = DateTime.Now;
    }

    public void DeleteProject(int projectId) => 
        _projectRepository.Delete(projectId);

    public bool CanDelete(int projectId) => 
        GetProjectById(projectId).StateHistory.ProjectState.State == FileState.Scratch;

    public void SendToCommissions(int projectId)
    {
        var project = GetProjectById(projectId);
        project.StateHistory.ProjectState.State = FileState.InCommission;
        project.StateHistory.Date = DateTime.Now;
    }
}