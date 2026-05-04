using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;

namespace WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;

public class InMemoryProjectRepository : IProjectRepository
{
    private readonly List<Project> _projects;

    public InMemoryProjectRepository()
    {
        _projects = new List<Project>();
        _projects.Add(
            new Project
            {
                Id = 1,
                Title = "Ley de Glaciares",
                Articles = "Art.2 ... Art.2 ...",
                Tags = new List<ProjectTag> { ProjectTag.PublicBuildings, ProjectTag.Emvironment },
                State = ProjectState.Scratch
            });
        _projects.Add(
            new Project
            {
                Id = 2,
                Title = "Ley de Libertad Educativa",
                Articles = "Art.2 ... Art.2 ...",
                Tags = new List<ProjectTag> { ProjectTag.Education },
                State = ProjectState.InCommission
            });
    }

    public List<Project> GetProjects()
    {
        return _projects.ToList();
    }

    public Project GerProjectById(int id)
    {
       return _projects.First(p => p.Id == id);
    }
}