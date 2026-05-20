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

    public Project BuildEmptyProject()
    {
        return new Project
        {
            StateHistory =
            [
                new ProjectStateHistory
                {
                    Date = DateTime.Now,
                    ProjectState = GetScratchProjectState()
                }
            ]
        };
    }

    private ProjectState GetScratchProjectState() => 
        _projectRepository.GetScratchProjectStates();

    public void CreateNewProject(string title, string articles, string fundaments, string summary)
    {

        var history = new List<ProjectStateHistory>();
        history.Add( new ProjectStateHistory
            {
                Date = DateTime.Now,
                ProjectState = GetScratchProjectState()
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
        
        _projectRepository.Add(newProject);
    }

    public Project GetProjectById(int projectId) =>
        _projectRepository.GetProjects().First(p => p.ProjectId == projectId);

    public bool ExistProject(int projectId) =>
        _projectRepository.GetProjects().Exists(p => p.ProjectId == projectId);

    public void UpdateProject(Project project, string title, string articles, string fundaments, string summary)
    {
        project.Title = title;
        project.Articles = articles;
        project.Fundaments = fundaments;
        project.Summary = summary;

        _projectRepository.UpdateProject(project);
    }

    public void RejectProjectByCommissions(int projectId)
    {
        var project = GetProjectById(projectId);
        project.GetCurrentState().ProjectState.State = FileState.RejectedByCommissions;
        project.GetCurrentState().Date = DateTime.Now;
    }

    public void SendToSession(int projectId)
    {
        var project =  GetProjectById(projectId);
        project.GetCurrentState().ProjectState.State = FileState.InSession;
        project.GetCurrentState().Date = DateTime.Now;
    }

    public bool SimulateSessionResult(Project project)
    {
        var rnd = new Random();
        var success = rnd.Next(2) == 0;
        if (success)
        {
            project.GetCurrentState().ProjectState.State = FileState.ApprovedInSession;
            project.GetCurrentState().Date = DateTime.Now;
        }
        else
        {
            project.GetCurrentState().ProjectState.State = FileState.RejectedInSession;
            project.GetCurrentState().Date = DateTime.Now;
        }

        return project.GetCurrentState().ProjectState.State == FileState.ApprovedInSession;
    }

    public bool CanSendToCommission(Project project) =>
        project.GetCurrentState().ProjectState.State == FileState.Scratch;

    public void SetPendingForCommissionsFor(Project project)
    {
        var projectStateHistory = new ProjectStateHistory
        {
            ProjectState = _projectRepository.GetSentToCommissionProjectStates(),
            Date = DateTime.Now,
        };

        _projectRepository.AddStateHistory(project.ProjectId, projectStateHistory);
    }

    public void DeleteProject(int projectId) => 
        _projectRepository.Delete(projectId);

    public bool CanDelete(int projectId) => 
        GetProjectById(projectId).GetCurrentState().ProjectState.State == FileState.Scratch;

    public void SetInCommissionFor(int projectId)
    {
        var projectStateHistory = new ProjectStateHistory
        {
            ProjectState = _projectRepository.GetInCommissionProjectStates(),
            Date = DateTime.Now
        };

        _projectRepository.AddStateHistory(projectId, projectStateHistory);
    }
}