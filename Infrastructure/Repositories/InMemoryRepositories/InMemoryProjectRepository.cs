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
                Articles = "Art.1: Glaciares \n Art.2 ...",
                State = new ProjectState
                {
                    CurrentState = FileState.Scratch,
                    ChangeDate = DateTime.Now
                }
            });
        _projects.Add(
            new Project
            {
                Id = 2,
                Title = "Ley de Libertad Educativa",
                Articles = "Art.2 ... Art.2 ...",
                State = new ProjectState
                {
                    CurrentState = FileState.InPendingCommissions, ChangeDate = DateTime.Now.Subtract(TimeSpan.FromDays(10))
                }
            });
    }

    public List<Project> GetProjects()
    {
        return _projects.ToList();
    }

    public Project GetProjectById(int id)
    {
        return _projects.First(p => p.Id == id);
    }

    public void Update(Project project)
    {
        var oldProject = GetProjectById(project.Id);
        oldProject.Title = project.Title;
        oldProject.Articles = project.Articles;
        oldProject.Fundaments = project.Fundaments;
        oldProject.Summary = project.Summary;
        oldProject.State = project.State;
    }

    public void AddNewProject(Project project)
    {
        project.Id = _projects.Max(p => p.Id) + 1;
        _projects.Add(project);
    }
}