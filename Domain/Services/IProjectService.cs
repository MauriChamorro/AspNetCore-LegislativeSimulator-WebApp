using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Services;

public interface IProjectService
{
    List<Project> GetProjects();
    Project CreatEmptyProject();
    void CreateNewProject(string title, string articles, string fundaments, string summary);
    Project GetProjectById(int projectId);
    bool ExistProject(int projectId);
    void EditProject(int projectId, string title, string articles, string fundaments, string summary);
}