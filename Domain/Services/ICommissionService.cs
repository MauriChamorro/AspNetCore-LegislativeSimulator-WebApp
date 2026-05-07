using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Services;

public interface ICommissionService
{
    List<Commission> EvaluateCommissionFor(string projectArticles);
    AssignedCommissions AssignCommissionTo(List<Commission> commissions, int projectId);
    AssignedCommissions GetAssignedCommissionsFor(int projectId);
    bool HasBeenAssigned(int projectId);
    List<ReferralCommission> GetReferralCommissionsFor(int projectId);
    ReferralCommission GetActualReferral(List<ReferralCommission> referralCommissions);
    void DoNextReferralPhase(ReferralCommission actualReferral);
    bool ThereAreNotPendingReferral(List<ReferralCommission> referralCommissions);
    bool ReferralIsRejected(ReferralCommission actualReferral);
    bool AcceptedByAllCommission(int projectId);
}