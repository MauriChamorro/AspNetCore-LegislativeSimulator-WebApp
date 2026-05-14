using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Services;

public interface ICommissionService
{
    List<Commission> EvaluateCommissionFor(string projectArticles);
    List<Referral> AssignCommissionTo(List<Commission> commissions, int projectId);
    bool HasBeenAssigned(int projectId);
    List<Referral> GetReferralCommissionsFor(int projectId);
    Referral GetActualReferral(List<Referral> referralCommissions);
    void DoNextReferralPhase(Referral actualReferral);
    bool ThereAreNotPendingReferral(List<Referral> referralCommissions);
    bool ReferralIsRejected(Referral actualReferral);
    bool AcceptedByAllCommission(int projectId);
}