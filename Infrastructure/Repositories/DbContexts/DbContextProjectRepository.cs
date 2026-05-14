using WebAppMVC.DbFirstModels;
using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;

namespace WebAppMVC.Infrastructure.Repositories.DbContexts;

public class DbContextProjectRepository: IProjectRepository
{
    private readonly ExpedientesDevContext _context;

    public DbContextProjectRepository(ExpedientesDevContext context)
    {
        _context = context;
    }
    
    public List<Project> GetProjects() => 
        _context.Projects.ToList();

    public Project GetProjectById(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(Project project)
    {
        throw new NotImplementedException();
    }

    public int GetLastId()
    {
        throw new NotImplementedException();
    }

    public void Delete(int projectId)
    {
        throw new NotImplementedException();
    }
}