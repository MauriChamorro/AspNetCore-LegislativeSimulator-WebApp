namespace WebAppMVC.ViewModels;

public class ProjectReferralCommissionsViewModel
{
    public int ProjectId {get; set;}
    public string ProjectTitle {get; set;}
    public List<ReferralCommissionViewModel> ReferralCommissions {get; set;}
    public string BackgroundColor { get; set; }
    public string Color { get; set; }
}