using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public List<Project> GetProjects() => _projectRepository.GetProjects();
    
    public Project CreatEmptyProject()
    {
        return new Project
        {
            State = new()
            {
                CurrentState = FileState.Scratch,
                ChangeDate =  DateTime.Now
            }
        };
    }

    public void CreateNewProject(string title, string articles, string fundaments, string summary)
    {
        var lastId = _projectRepository.GetLastId();
        var newProject = new Project
        {
            Id = lastId + 1,
            Title = title,
            Articles = articles,
            Fundaments = fundaments,
            State = new()
            {
                CurrentState = FileState.Scratch,
                ChangeDate =  DateTime.Now
            }
        };
        _projectRepository.Add(newProject);
    }
}