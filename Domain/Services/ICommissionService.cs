using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Services;

public interface ICommissionService
{
    List<Commission> EvaluateCommissionFor(string projectArticles);
    List<Referral> AssignCommissionTo(List<Commission> commissions, int projectId);
    bool HasBeenAssigned(int projectId);
    List<Referral> GetReferralsFor(int projectId);
    Referral GetActualReferralFor(List<Referral> referralCommissions);
    void DoNextReferralPhase(Referral actualReferral);
    bool AllCommissionEvaluated(List<Referral> referralCommissions);
    bool IsRejectedReferral(Referral actualReferral);
    bool AcceptedByAllCommission(List<Referral> referrals);
}