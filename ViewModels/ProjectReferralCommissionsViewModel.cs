namespace WebAppMVC.ViewModels;

public class ProjectReferralCommissionsViewModel
{
    public int ProjectId { get; set; }
    public string ProjectTitle { get; set; }
    public List<ReferralViewModel> ReferralCommissions { get; set; }
}