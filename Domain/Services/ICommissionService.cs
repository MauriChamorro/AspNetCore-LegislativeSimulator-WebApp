using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Services;

public interface ICommissionService
{
    Task<List<Commission>> EvaluateCommissionFor(string projectArticles);
    Task<List<Referral>> AssignCommissionTo(List<Commission> commissions, int projectId);
    Task<bool> HasBeenAssigned(int projectId);
    Task<List<Referral>> GetReferralsFor(int projectId);
    Referral GetActualReferralFor(List<Referral> referrals);
    Task DoNextReferralPhase(Referral actualReferral);
    bool AllCommissionEvaluated(List<Referral> referrals);
    bool IsRejectedReferral(Referral actualReferral);
    bool AcceptedByAllCommission(List<Referral> referrals);
    bool AlreadyRejected(List<Referral> referrals);
}