using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Repositories;

public interface IProjectRepository
{
    List<Project> GetProjects();
    Project GetProjectById(int id);
    void UpdateByEdit(Project project);
    void AddNewProject(Project project);
}