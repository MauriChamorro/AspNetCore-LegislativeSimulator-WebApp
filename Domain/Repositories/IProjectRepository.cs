using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetProjects();
    Task<bool> Exists(int projectId);
    Task<Project> Get(int projectId);
    Task Add(Project project);
    void Delete(int projectId);
    Task UpdateProject(Project updatedProject);
    Task AddStateHistory(int projectId, ProjectStateHistory projectStateHistory);
    List<Commission> GetCommissions();
    void AddReferrals(List<Referral> referralCommissions);
    Task<ProjectState> GetScratchProjectState();
    Task<ProjectState> GetSentToCommissionProjectState();
    Task<ProjectState> GetInCommissionProjectState();
    Task<ProjectState> GetRejectedByCommissionProjectState();
    Task<ProjectState> GetInSessionProjectState();
    Task<ProjectState> GetApprovedInSessionProjectState();
    Task<ProjectState> GetRejectedInSessionProjectState();
    bool HasReferrals(int projectId);
    List<Referral> GetReferralsFor(int projectId);
    void UpdateReferral(Referral modelReferral);

}