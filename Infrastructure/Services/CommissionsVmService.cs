using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Services;

public class CommissionsVmService: ICommissionsVmService
{
    public ProjectReferralCommissionsViewModel CreateReferralCommissionsVMs(
        Project project,
        List<ReferralCommission> referralCommissions)
    {
        return new ProjectReferralCommissionsViewModel
        {
            ProjectId =  project.Id,
            ProjectTitle = project.Title,
            ReferralCommissions = CreateReferralCommissions(referralCommissions)
        };
    }

    private List<ReferralCommissionViewModel> CreateReferralCommissions(List<ReferralCommission> referralCommissions)
    {
        var referralCommissionsVm = new List<ReferralCommissionViewModel>();
        foreach (var referralCommission in referralCommissions)
        {
            referralCommissionsVm.Add(
                new ReferralCommissionViewModel
                {
                    CommissionName = referralCommission.CommisionName,
                    ReferralStateName  = GetReferralStateName(referralCommission.State),
                    ReferralDate =  referralCommission.ReferralDate
                }    
            );
        }
        return referralCommissionsVm;
    }

    private string GetReferralStateName(ReferralCommissionState referralCommissionState)
    {
        switch (referralCommissionState)
        {
            case ReferralCommissionState.Assigned:
                return "Asignado";
            default:
                return "none";
        }
    }
}