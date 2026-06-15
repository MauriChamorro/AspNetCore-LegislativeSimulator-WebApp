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
            ProjectStateName = project.GetCurrentState().ProjectState.Name,
            CurrentProjectState =  project.GetCurrentState().ProjectState.State,
            ReferralCommissions = CreateReferralVms(referralCommissions),
        };
    }

    private List<ReferralViewModel> CreateReferralVms(List<Referral> referrals)
    {
        var referralCommissionsVm = new List<ReferralViewModel>();
        foreach (var referral in referrals)
        {
            referralCommissionsVm.Add(
                new ReferralViewModel
                {
                    CommissionId = referral.CommissionId,
                    CommissionName = referral.Commission.Name,
                    ReferralStateName  = GetReferralStateName(referral.State),
                    StateId = (int)referral.State,
                    ReferralDate =  referral.DateState,
                    BackgroundColor = GetBackgroundColorForCommission(referral.CommissionId),
                    Color = GetColorForCommission(referral.CommissionId)
                }    
            );
        }
        return referralCommissionsVm;
    }

    private string GetColorForCommission(int commissionId) => "#000000";


    private string GetBackgroundColorForCommission(int commissionId)
    {
        //todo: select by name or from bd
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
        

    private string GetReferralStateName(ReferralState referralState)
    {
        switch (referralState)
        {
            case ReferralState.Assigned:
                return "Asignado";
            case ReferralState.Evaluating:
                return "Evaluando";
            case ReferralState.Accepted:
                return "Aceptado";
            case ReferralState.Rejected:
                return "Rechazado";
            default:
                return "none";
        }
    }
}