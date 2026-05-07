using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Repositories;

public interface IReferralCommissionRepository
{
    void AddRange(List<ReferralCommission> referralCommissions);
    List<ReferralCommission> GetFor(int projectId);
}