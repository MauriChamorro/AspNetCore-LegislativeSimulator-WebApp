using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Services;

public interface IProjectService
{
    Task<List<Project>> GetProjects();
    Task<bool> ExistProjectAsync(int projectId);
    Task<Project> GetProjectByIdAsync(int projectId);
    Task<Project> BuildEmptyProject();
    Task CreateNewProject(string title, string articles, string fundaments, string summary);
    Task UpdateProject(Project project, string title, string articles, string fundaments, string summary);
    Task RejectProjectByCommissions(int projectId);
    Task SendToSession(int projectId);
    Task<bool> DoSessionRandomly(Project project);
    bool CanSendToCommission(Project project);
    Task SetPendingForCommissionsFor(Project project);
    Task DeleteProject(int projectId);
    Task<bool> CanDelete(int projectId);
    Task SetInCommissionFor(int projectId);
    bool CanAssignCommissions(Project project);
    bool IsInSession(Project project);
    Task<bool> CanSendToSession(Project project);
    Task<bool> DoSessionWithResult(Project project, int result);
}