using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Repositories;

public interface IProjectRepository
{
    List<Project> GetProjects();
    void Add(Project project);
    void Delete(int projectId);
    ProjectState GetScratchProjectStates();
    void UpdateProject(Project updatedProject);
}