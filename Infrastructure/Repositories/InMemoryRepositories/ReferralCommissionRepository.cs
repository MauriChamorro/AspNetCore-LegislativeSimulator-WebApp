using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;

namespace WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;

public class ReferralCommissionRepository: IReferralCommissionRepository
{
    private readonly List<Referral> _referrals;

    public ReferralCommissionRepository()
    {
        _referrals = new List<Referral>();
        _referrals.Add(
            new Referral
            {
                ProjectId = 4,
                CommissionId = 3,
                DateState =  DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                State = ReferralState.Accepted
            });
        _referrals.Add(
            new Referral
            {
                ProjectId = 4,
                CommissionId = 4,
                DateState =  DateTime.Now.Subtract(TimeSpan.FromDays(5)),
                State = ReferralState.Accepted
            });
        _referrals.Add(
            new Referral
            {
                ProjectId = 5,
                CommissionId = 2,
                DateState =  DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                State = ReferralState.Accepted
            });
        _referrals.Add(
            new Referral
            {
                ProjectId = 5,
                CommissionId = 4,
                DateState =  DateTime.Now.Subtract(TimeSpan.FromDays(5)),
                State = ReferralState.Accepted
            });
    } 
    
    public void AddRange(List<Referral> referralCommissions) => 
        _referrals.AddRange(referralCommissions);
    
    public bool ExistProjectId(int projectId) => 
        _referrals.Any(ac => ac.ProjectId == projectId);
    
    public List<Referral> GetFor(int projectId) => 
        _referrals.FindAll(referralCommission => referralCommission.ProjectId == projectId);
}