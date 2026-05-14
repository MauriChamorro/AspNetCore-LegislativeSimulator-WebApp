namespace WebAppMVC.Domain.Models.Projects;

public partial class Project
{
    public int ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Articles { get; set; } = string.Empty;
    public string Fundaments { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public int StateId { get; set; }
    public virtual ProjectState State { get; set; } = null!;
    public virtual ICollection<Referral> Referrals { get; set; } = new List<Referral>();

    public bool CanEdit() => State.State == FileState.Scratch;

    public bool AreCommissionsAssigned() => State.State == FileState.InCommission;
}