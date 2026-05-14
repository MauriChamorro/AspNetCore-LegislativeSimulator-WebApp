using Model = WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Infrastructure.DbContexts;

namespace WebAppMVC.Infrastructure.Repositories.DbContexts;

public class DbContextProjectRepository: IProjectRepository
{
    private readonly ExpedientesDevContext _context;

    public DbContextProjectRepository(ExpedientesDevContext context)
    {
        _context = context;
    }

    public List<Model.Project> GetProjects()
    {
        return _context.Projects
            .Select(p =>
                new Model.Project
                {
                    ProjectId = int.Parse(p.ProjectId),
                    Title = p.Title,
                    Articles = p.Articles,
                    Fundaments = p.Fundaments,
                    Summary = p.Summary
                
            }).ToList();
    }

    public Model.Project GetProjectById(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(Model.Project project)
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