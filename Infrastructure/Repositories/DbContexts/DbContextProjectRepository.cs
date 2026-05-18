using Model = WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;
using WebAppMVC.Infrastructure.DbContexts;
using WebAppMVC.Infrastructure.Entities;

namespace WebAppMVC.Infrastructure.Repositories.DbContexts;

public class DbContextProjectRepository : IProjectRepository
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
                    FileId = p.FileId,
                    Title = p.Title,
                    Articles = p.Articles,
                    Fundaments = p.Fundaments,
                    Summary = p.Summary,
                    StateHistory = p.ProjectStateHistories.Select(h =>
                        new Model.ProjectStateHistory
                        {
                            Date = h.Date,
                            ProjectState = new Model.ProjectState
                            {
                                Id = h.ProjectState.ProjectStateId,
                                Name = h.ProjectState.Name,
                                State = (Model.FileState)h.ProjectState.IntState
                            }
                        }).ToList()
                }).ToList();
    }


    public void UpdateProject(Model.Project updatedProject)
    {
        var entityProject = new Project
        {
            ProjectId = updatedProject.ProjectId,
            FileId = updatedProject.FileId,
            Title = updatedProject.Title,
            Articles = updatedProject.Articles,
            Fundaments = updatedProject.Fundaments,
            Summary = updatedProject.Summary
        };
        _context.Update(entityProject);
        _context.SaveChanges();
    }
    
    public void AddStateHistory(int projectId,
        Model.ProjectStateHistory projectStateHistory)
    {
        _context.ProjectStateHistories.Add(new ProjectStateHistory
        {
            ProjectId =  projectId,
            ProjectStateId = projectStateHistory.ProjectState.Id,
            Date = projectStateHistory.Date
        });

        _context.SaveChanges();
    }

    public Model.ProjectState GetSentToCommissionProjectStates()
    {
        //todo: refactor with GetScratchProjectStates()a
        return _context.ProjectStates
            //todo: check out how .where works
            .Where(s => s.Name == "Enviado a Comisiones")
            .Select(e => new Model.ProjectState
            {
                Id = e.ProjectStateId,
                Name = e.Name,
                State = (Model.FileState)e.IntState
            })
            .First();
    }

    public Model.ProjectState GetScratchProjectStates() =>
        _context.ProjectStates
            //todo: check out how .where works
            .Where(s => s.Name == "Borrador")
            .Select(e => new Model.ProjectState
            {
                Id = e.ProjectStateId,
                Name = e.Name,
                State = (Model.FileState)e.IntState
            })
            .First(); // todo: change for another strategy to get only one

    public void Add(Model.Project project)
    {
        var newHistory = new ProjectStateHistory
        {
            ProjectStateId = project.GetCurrentState().ProjectState.Id,
            Date = DateTime.Now,
        };

        var entityProject = new Project
        {
            FileId = project.FileId,
            Title = project.Title,
            Articles = project.Articles,
            Fundaments = project.Fundaments,
            Summary = project.Summary,
            ProjectStateHistories = new List<ProjectStateHistory> { newHistory }
        };
        _context.Projects.Add(entityProject);
        _context.SaveChanges();
    }


    public void Delete(int projectId)
    {
        var entityProject = new Project { ProjectId = projectId };
        var history = _context.ProjectStateHistories
            .Where(h => h.ProjectId == projectId);

        foreach (var stateHistory in history)
            _context.Remove(stateHistory);

        _context.Projects.Remove(entityProject);

        _context.SaveChanges();
    }
}