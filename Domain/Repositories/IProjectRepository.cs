using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Repositories;

public interface IProjectRepository
{
    Task<List<Project>> GetProjects();
    Task<bool> Exists(int projectId);
    Task<Project> Get(int projectId);
    void Add(Project project);
    void Delete(int projectId);
    void UpdateProject(Project updatedProject);
    void AddStateHistory(int projectId, ProjectStateHistory projectStateHistory);
    List<Commission> GetCommissions();
    void AddReferrals(List<Referral> referralCommissions);
    ProjectState GetScratchProjectState();
    ProjectState GetSentToCommissionProjectState();
    ProjectState GetInCommissionProjectState();
    ProjectState GetRejectedByCommissionProjectState();
    ProjectState GetInSessionProjectState();
    ProjectState GetApprovedInSessionProjectState();
    ProjectState GetRejectedInSessionProjectState();
    bool HasReferrals(int projectId);
    List<Referral> GetReferralsFor(int projectId);
    void UpdateReferral(Referral modelReferral);

}