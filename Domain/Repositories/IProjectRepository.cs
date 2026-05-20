using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Repositories;

public interface IProjectRepository
{
    List<Project> GetProjects();
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
    bool HasReferrals(int projectId);
    List<Referral> GetReferralsFor(int projectId);
    void UpdateReferral(Referral modelReferral);
}