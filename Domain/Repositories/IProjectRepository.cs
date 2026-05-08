using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Repositories;

public interface IProjectRepository
{
    List<Project> GetProjects();
    Project GetProjectById(int id);
    void Edit(Project project);
    void Add(Project project);
    int GetLastId();
}