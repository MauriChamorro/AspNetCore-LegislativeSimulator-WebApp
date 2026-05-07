namespace WebAppMVC.ViewModels;

public class ProjectReferralCommissionsViewModel
{
    public int ProjectId {get; set;}
    public string ProjectTitle {get; set;}
    public List<ReferralCommissionViewModel> ReferralCommissions {get; set;}
}