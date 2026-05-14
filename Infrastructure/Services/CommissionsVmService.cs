using WebAppMVC.Domain.Models.Projects;
using WebAppMVC.Infrastructure.Interfaces;
using WebAppMVC.ViewModels;

namespace WebAppMVC.Infrastructure.Services;

public class CommissionsVmService: ICommissionsVmService
{
    public ProjectReferralCommissionsViewModel CreateReferralCommissionsVMs(
        Project project,
        List<Referral> referralCommissions)
    {
        return new ProjectReferralCommissionsViewModel
        {
            ProjectId =  project.ProjectId,
            ProjectTitle = project.Title,
            ReferralCommissions = CreateReferralCommissions(referralCommissions),
        };
    }

    private List<ReferralCommissionViewModel> CreateReferralCommissions(List<Referral> referralCommissions)
    {
        var referralCommissionsVm = new List<ReferralCommissionViewModel>();
        foreach (var referralCommission in referralCommissions)
        {
            referralCommissionsVm.Add(
                new ReferralCommissionViewModel
                {
                    CommissionName = referralCommission.CommissionName,
                    ReferralStateName  = GetReferralStateName(referralCommission.State),
                    ReferralDate =  referralCommission.Date,
                    BackgroundColor = GetBackgroundColorForCommission(referralCommission.CommissionId),
                    Color = GetColorForCommission(referralCommission.CommissionId)
                }    
            );
        }
        return referralCommissionsVm;
    }

    private string GetColorForCommission(int commissionId)
    {
        switch (commissionId)
        {
            default:
                return "#000000";
        }
    }
        

    private string GetBackgroundColorForCommission(int commissionId)
    {
        switch (commissionId)
        {
            case 1:
                return "#9cbff7";
            case 2:
                return "#b0e8f5";
            case 3:
                return "#b0f5e1";
            case 4:
                return "#b0f5cd";
            case 5:
                return "#b0f5b6";
            case 6:
                return "#bcf5b0";
            case 7:
                return "#ddf5b0";
            default:
                return "none";
        }
    }
        

    private string GetReferralStateName(ReferralCommissionState referralCommissionState)
    {
        switch (referralCommissionState)
        {
            case ReferralCommissionState.Assigned:
                return "Asignado";
            case ReferralCommissionState.Evaluating:
                return "Evaluando";
            case ReferralCommissionState.Accepted:
                return "Aceptado";
            case ReferralCommissionState.Rejected:
                return "Rechazado";
            default:
                return "none";
        }
    }
}