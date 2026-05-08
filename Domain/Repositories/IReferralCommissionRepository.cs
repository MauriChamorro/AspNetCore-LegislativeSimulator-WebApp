using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Repositories;

public interface IReferralCommissionRepository
{
    bool ExistProjectId(int projectId);
    void AddRange(List<ReferralCommission> referralCommissions);
    List<ReferralCommission> GetFor(int projectId);
}