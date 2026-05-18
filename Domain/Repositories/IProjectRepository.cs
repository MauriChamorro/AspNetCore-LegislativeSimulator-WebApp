using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Repositories;

public interface IProjectRepository
{
    List<Project> GetProjects();
    void Add(Project project);
    void Delete(int projectId);
    void UpdateProject(Project updatedProject);
    void AddStateHistory(int projectId, ProjectStateHistory projectStateHistory);
    ProjectState GetScratchProjectStates();
    ProjectState GetSentToCommissionProjectStates();
}