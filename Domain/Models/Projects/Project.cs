namespace WebAppMVC.Domain.Models.Projects;

public class Project
{
    public int ProjectId { get; set; }
    public string FileId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Articles { get; set; } = string.Empty;
    public string Fundaments { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public int StateId { get; set; }
    public List<ProjectStateHistory> StateHistory { get; set; } = null!;
    public ICollection<Referral> Referrals { get; set; } = new List<Referral>();

    public ProjectStateHistory GetCurrentState() => StateHistory.Last();

    public bool CanEdit() => GetCurrentState().ProjectState.State == FileState.Scratch;

    public bool AreCommissionsAssigned() => GetCurrentState().ProjectState.State != FileState.Scratch && 
                                            GetCurrentState().ProjectState.State != FileState.PendingForAssignCommissions;
}