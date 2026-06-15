using WebAppMVC.Domain.Models.Projects;

namespace WebAppMVC.ViewModels;

public class ProjectReferralCommissionsViewModel
{
    public int ProjectId { get; set; }
    public string ProjectTitle { get; set; }
    public string ProjectStateName { get; set; }
    public List<ReferralViewModel> ReferralCommissions { get; set; }
    public FileState CurrentProjectState { get; set; }
}