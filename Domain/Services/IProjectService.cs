using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Services;

public interface IProjectService
{
    List<Project> GetProjects();
    Project BuildEmptyProject();
    void CreateNewProject(string title, string articles, string fundaments, string summary);
    Project GetProjectById(int projectId);
    bool ExistProject(int projectId);
    void UpdateProject(Project project, string title, string articles, string fundaments, string summary);
    void RejectProjectByCommissions(int projectId);
    void SendToSession(int projectId);
    bool SimulateSessionResult(Project project);
    bool CanSendToCommission(Project project);
    void SetPendingForCommissionsFor(Project project);
    void DeleteProject(int projectId);
    bool CanDelete(int projectId);
    void SetInCommissionFor(int projectId);
}