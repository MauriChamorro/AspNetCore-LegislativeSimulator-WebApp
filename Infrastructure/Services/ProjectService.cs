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
            State = new()
            {
                CurrentState = FileState.Scratch,
                ChangeDate = DateTime.Now
            }
        };
    }

    public void CreateNewProject(string title, string articles, string fundaments, string summary)
    {
        var lastId = _projectRepository.GetLastId();
        var newProject = new Project
        {
            Id = lastId + 1,
            Title = title,
            Articles = articles,
            Fundaments = fundaments,
            Summary = summary,
            State = new()
            {
                CurrentState = FileState.Scratch,
                ChangeDate = DateTime.Now
            }
        };
        _projectRepository.Add(newProject);
    }

    public Project GetProjectById(int projectId) =>
        _projectRepository.GetProjects().First(p => p.Id == projectId);

    public bool ExistProject(int projectId) =>
        _projectRepository.GetProjects().Exists(p => p.Id == projectId);

    public void EditProject(Project project, string title, string articles, string fundaments, string summary)
    {
        project.Title = title;
        project.Articles = articles;
        project.Fundaments = fundaments;
        project.Summary = summary;
    }

    public void RejectProjectByCommissions(int projectId)
    {
        var project = _projectRepository.GetProjectById(projectId);
        project.State.CurrentState = FileState.RejectedByCommissions;
        project.State.ChangeDate = DateTime.Now;
    }

    public void SendToSession(int projectId)
    {
        var project = _projectRepository.GetProjectById(projectId);
        project.State.CurrentState = FileState.InSession;
        project.State.ChangeDate = DateTime.Now;
    }

    public void SimulateSessionResult(Project project)
    {
        var rnd = new Random();
        var success = rnd.Next(2) == 0;
        if (success)
        {
            project.State.CurrentState = FileState.ApprovedInSession;
            project.State.ChangeDate = DateTime.Now;
        }
        else
        {
            project.State.CurrentState = FileState.RejectedInSession;
            project.State.ChangeDate = DateTime.Now;
        }
    }

    public bool CanSendToCommission(Project project) =>
        project.State.CurrentState == FileState.Scratch;

    public void SendToCommissions(Project project)
    {
        project.State.CurrentState = FileState.PendingForAssignCommissions;
        project.State.ChangeDate = DateTime.Now;
    }
}