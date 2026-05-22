using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Services;

public interface IProjectService
{
    Task<List<Project>> GetProjects();
    Task<bool> ExistProjectAsync(int projectId);
    Task<Project> GetProjectByIdAsync(int projectId);
    Project BuildEmptyProject();
    void CreateNewProject(string title, string articles, string fundaments, string summary);
    void UpdateProject(Project project, string title, string articles, string fundaments, string summary);
    void RejectProjectByCommissions(int projectId);
    void SendToSession(Project project);
    bool DoSession(Project project);
    bool CanSendToCommission(Project project);
    void SetPendingForCommissionsFor(Project project);
    void DeleteProject(int projectId);
    Task<bool> CanDelete(int projectId);
    void SetInCommissionFor(int projectId);
    bool CanAssignCommissions(Project project);
    bool IsInCommission(Project project);
    bool IsInSession(Project project);
}