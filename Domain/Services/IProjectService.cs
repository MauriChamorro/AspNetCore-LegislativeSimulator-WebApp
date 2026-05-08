using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Services;

public interface IProjectService
{
    List<Project> GetProjects();
    Project CreatEmptyProject();
}