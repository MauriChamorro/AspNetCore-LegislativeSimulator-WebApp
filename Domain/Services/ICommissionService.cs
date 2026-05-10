using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Domain.Services;

public interface ICommissionService
{
    List<Commission> EvaluateCommissionFor(string projectArticles);
    List<ReferralCommission> AssignCommissionTo(List<Commission> commissions, int projectId);
    bool HasBeenAssigned(int projectId);
    List<ReferralCommission> GetReferralCommissionsFor(int projectId);
    ReferralCommission GetActualReferral(List<ReferralCommission> referralCommissions);
    void DoNextReferralPhase(ReferralCommission actualReferral);
    bool ThereAreNotPendingReferral(List<ReferralCommission> referralCommissions);
    bool ReferralIsRejected(ReferralCommission actualReferral);
    bool AcceptedByAllCommission(int projectId);
}