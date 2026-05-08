using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;

namespace WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;

public class ReferralCommissionRepository: IReferralCommissionRepository
{
    private readonly List<ReferralCommission> _referrals;

    public ReferralCommissionRepository() => 
        _referrals = new List<ReferralCommission>();
    
    public void AddRange(List<ReferralCommission> referralCommissions) => 
        _referrals.AddRange(referralCommissions);
    
    public bool ExistProjectId(int projectId) => 
        _referrals.Any(ac => ac.ProjectId == projectId);
    
    public List<ReferralCommission> GetFor(int projectId) => 
        _referrals.FindAll(referralCommission => referralCommission.ProjectId == projectId);
}