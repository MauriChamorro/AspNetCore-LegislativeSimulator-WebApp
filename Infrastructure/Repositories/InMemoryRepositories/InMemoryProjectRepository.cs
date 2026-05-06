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
                Articles = "Educación en el Hogar (Homeschooling): La normativa permite que las familias capaciten a sus hijos en el hogar, ya sea por cuenta propia o contratando maestros particulares, con el fin de educar según sus valores." +
                           "Autonomía Escolar: Las instituciones educativas tendrán mayor libertad para definir sus propios métodos de enseñanza, currículos y planes de estudio dentro de una base común.",
                State = new ProjectState
                {
                    CurrentState = FileState.InPendingCommissions, ChangeDate = DateTime.Now.Subtract(TimeSpan.FromDays(10))
                }
            });
    }

    public List<Project> GetProjects() => 
        _projects.ToList();

    public Project GetProjectById(int id) => 
        _projects.First(p => p.Id == id);

    public void UpdateByEdit(Project project)
    {
        var savedProject = GetProjectById(project.Id);
        savedProject.Title = project.Title;
        savedProject.Articles = project.Articles;
        savedProject.Fundaments = project.Fundaments;
        savedProject.Summary = project.Summary;
    }

    public void AddNewProject(Project project)
    {
        project.Id = _projects.Max(p => p.Id) + 1;
        _projects.Add(project);
    }
}