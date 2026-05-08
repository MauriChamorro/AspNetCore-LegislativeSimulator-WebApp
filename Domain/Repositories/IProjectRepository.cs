using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Repositories;

public interface IProjectRepository
{
    List<Project> GetProjects();
    Project GetProjectById(int id);
    void Add(Project project);
    int GetLastId();
    void Delete(int projectId);
}