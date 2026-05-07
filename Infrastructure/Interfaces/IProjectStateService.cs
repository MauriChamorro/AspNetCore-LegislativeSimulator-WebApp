using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface IProjectStateService
{
    bool CanEdit(ProjectState state);
    string GetNameState(FileState projectVmCurrentState);
    ProjectState EmptyProject();
    bool CommissionsAssigned(ProjectState projectState);
}