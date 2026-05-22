using Microsoft.EntityFrameworkCore;
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

    public async Task<List<Model.Project>> GetProjects()
    {
        return await _context.Projects
            .Select(p =>
                new Model.Project
                {
                    ProjectId = p.ProjectId,
                    FileId = p.FileId.Trim(),
                    Title = p.Title.Trim(),
                    Articles = p.Articles.Trim(),
                    Fundaments = p.Fundaments.Trim(),
                    Summary = p.Summary.Trim(),
                    StateHistory = p.ProjectStateHistories.Select(h =>
                        new Model.ProjectStateHistory
                        {
                            Date = h.Date,
                            ProjectState = new Model.ProjectState
                            {
                                Id = h.ProjectState.ProjectStateId,
                                Name = h.ProjectState.Name.Trim(),
                                State = (Model.FileState)h.ProjectState.IntState
                            }
                        }).ToList()
                })
            .ToListAsync();
    }

    public Task<bool> Exists(int projectId) => 
        _context.Projects.AnyAsync(p => p.ProjectId == projectId);

    public async Task<Model.Project> Get(int projectId)
    {
        var p = await _context.Projects.FindAsync(projectId);
        return new Model.Project
        {
            ProjectId = p.ProjectId,
            FileId = p.FileId.Trim(),
            Title = p.Title.Trim(),
            Articles = p.Articles.Trim(),
            Fundaments = p.Fundaments.Trim(),
            Summary = p.Summary.Trim(),
            StateHistory = p.ProjectStateHistories.Select(h =>
                new Model.ProjectStateHistory
                {
                    Date = h.Date,
                    ProjectState = new Model.ProjectState
                    {
                        Id = h.ProjectState.ProjectStateId,
                        Name = h.ProjectState.Name.Trim(),
                        State = (Model.FileState)h.ProjectState.IntState
                    }
                }).ToList()
        };
    }


    public async Task UpdateProject(Model.Project updatedProject)
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
        await _context.SaveChangesAsync();
    }
    
    public async Task AddStateHistory(int projectId,
        Model.ProjectStateHistory projectStateHistory)
    {
        _context.ProjectStateHistories.Add(new ProjectStateHistory
        {
            ProjectId =  projectId,
            ProjectStateId = projectStateHistory.ProjectState.Id,
            Date = projectStateHistory.Date
        });

        await _context.SaveChangesAsync();
    }

    public async Task<Model.ProjectState> GetScratchProjectState() => 
        await GetProjectStateByName("Borrador");
    
    public async Task<Model.ProjectState> GetSentToCommissionProjectState() => 
        await GetProjectStateByName("Enviado a Comisiones");

    public async Task<Model.ProjectState> GetInCommissionProjectState() => 
        await GetProjectStateByName("En Comisiones");

    public async Task<Model.ProjectState> GetRejectedByCommissionProjectState() =>
        await GetProjectStateByName("Rechazado por Comisiones");

    public async Task<Model.ProjectState> GetInSessionProjectState() => 
        await GetProjectStateByName("En Sesión");

    public async Task<Model.ProjectState> GetApprovedInSessionProjectState() => 
        await GetProjectStateByName("Aprobado");

    public async Task<Model.ProjectState> GetRejectedInSessionProjectState() => 
        await GetProjectStateByName("Rechazado en Sesión");

    public async Task<bool> HasReferrals(int projectId) => 
        await _context.Referrals.AnyAsync(r => r.ProjectId == projectId);

    public List<Model.Referral> GetReferralsFor(int projectId) =>
        _context.Referrals
            .Where(r => r.ProjectId == projectId)
            .Select(referral => new Model.Referral
            {
                ProjectId = referral.ProjectId,
                CommissionId = referral.CommissionId,
                DateState = referral.Date,
                Commission = new Model.Commission
                {
                    CommissionId =  referral.CommissionId,
                    Name = referral.Commission.Name
                },
                State = (Model.ReferralState)referral.State
            })
            .ToList();

    public void UpdateReferral(Model.Referral modelReferral)
    {
        var referral = _context.Referrals.Find(modelReferral.ProjectId, modelReferral.CommissionId);
        referral.State = (int)modelReferral.State;
        referral.Date = modelReferral.DateState;
        _context.SaveChanges();
    }

    private async Task<Model.ProjectState> GetProjectStateByName(string commissionName)
    {
        var projectState = await _context.ProjectStates
            .SingleAsync(e => e.Name.Contains(commissionName));
        
        return new Model.ProjectState
        {
            Id = projectState.ProjectStateId,
            Name = projectState.Name,
            State = (Model.FileState)projectState.IntState
        };
    }

    public List<Model.Commission> GetCommissions() =>
        _context.Commissions
            .Select(commission => new Model.Commission
            {
                CommissionId = commission.CommissionId,
                Name =  commission.Name,
                WordsForAssignment = commission.Tags!.Trim().Split().ToList()
            })
            .ToList();

    public void AddReferrals(List<Model.Referral> referralCommissions)
    {
        var entities = new List<Referral>();
        foreach (var modelReferral in referralCommissions)
        {
            entities.Add(new Referral
            {
                ProjectId = modelReferral.ProjectId,
                CommissionId =  modelReferral.CommissionId,
                Date = modelReferral.DateState,
                State = (int)modelReferral.State
            });
        }
        _context.Referrals.AddRange(entities);
        _context.SaveChanges();
    }

    public async Task Add(Model.Project project)
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
        await _context.SaveChangesAsync();
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