using Model = WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Infrastructure.DbContextss;

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
                    StateHistory = p.ProjectStateHistories.Select( h=> new Model.ProjectStateHistory
                    {
                        Date = h.Date,
                        ProjectState = new Model.ProjectState
                        {
                            Id = h.ProjectState.ProjectStateId,
                            Name = h.ProjectState.Name,
                            State =  (Model.FileState)h.ProjectState.IntState
                        }
                    }).ToList()
            }).ToList();
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