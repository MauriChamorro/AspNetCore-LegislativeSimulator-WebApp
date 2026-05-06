using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Services.Interfaces;

public interface IProjectStateService
{
    bool CanEdit(ProjectState state);
    string GetNameState(FileState projectVmCurrentState);
    ProjectState EmptyProject();
}