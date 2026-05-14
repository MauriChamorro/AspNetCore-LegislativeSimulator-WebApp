using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.Domain.Repositories;

public interface IReferralCommissionRepository
{
    bool ExistProjectId(int projectId);
    void AddRange(List<Referral> referralCommissions);
    List<Referral> GetFor(int projectId);
}