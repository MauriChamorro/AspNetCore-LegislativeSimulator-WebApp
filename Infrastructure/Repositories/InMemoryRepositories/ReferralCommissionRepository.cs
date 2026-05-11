using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Domain.Repositories;

namespace WebAppMVC.Infrastructure.Repositories.InMemoryRepositories;

public class ReferralCommissionRepository: IReferralCommissionRepository
{
    private readonly List<ReferralCommission> _referrals;

    public ReferralCommissionRepository()
    {
        _referrals = new List<ReferralCommission>();
        _referrals.Add(
            new ReferralCommission
            {
                ProjectId = 4,
                CommissionId = 3,
                CommisionName = "Comisión de Legislación General",
                ReferralDate =  DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                State = ReferralCommissionState.Accepted
            });
        _referrals.Add(
            new ReferralCommission
            {
                ProjectId = 4,
                CommissionId = 4,
                CommisionName = "Comisión de Legislación del Trabajo",
                ReferralDate =  DateTime.Now.Subtract(TimeSpan.FromDays(5)),
                State = ReferralCommissionState.Accepted
            });
        _referrals.Add(
            new ReferralCommission
            {
                ProjectId = 5,
                CommissionId = 2,
                CommisionName = "Comisión de Presupuesto y Hacienda",
                ReferralDate =  DateTime.Now.Subtract(TimeSpan.FromDays(10)),
                State = ReferralCommissionState.Accepted
            });
        _referrals.Add(
            new ReferralCommission
            {
                ProjectId = 5,
                CommissionId = 4,
                CommisionName = "Comisión de Legislación del Trabajo",
                ReferralDate =  DateTime.Now.Subtract(TimeSpan.FromDays(5)),
                State = ReferralCommissionState.Accepted
            });
    } 
    
    public void AddRange(List<ReferralCommission> referralCommissions) => 
        _referrals.AddRange(referralCommissions);
    
    public bool ExistProjectId(int projectId) => 
        _referrals.Any(ac => ac.ProjectId == projectId);
    
    public List<ReferralCommission> GetFor(int projectId) => 
        _referrals.FindAll(referralCommission => referralCommission.ProjectId == projectId);
}