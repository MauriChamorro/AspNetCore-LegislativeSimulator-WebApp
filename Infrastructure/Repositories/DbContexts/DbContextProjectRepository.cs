using Microsoft.EntityFrameworkCore;
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
                    ProjectId = p.ProjectId,
                    Title = p.Title,
                    Articles = p.Articles,
                    Fundaments = p.Fundaments,
                    Summary = p.Summary,
                    State = new Model.ProjectState
                    {
                        StateId =  p.StateId,
                        Name =  p.State.Name,
                        Date =   p.State.Date.Value,
                        State = (Model.FileState)p.State.State
                    }
                
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