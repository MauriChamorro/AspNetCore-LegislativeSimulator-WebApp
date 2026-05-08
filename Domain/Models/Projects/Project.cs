namespace WebAppMVC.Domain.Models.Projects;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Articles { get; set; } = string.Empty;
    public string Fundaments { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public ProjectState State { get; set; }

    public bool CanEdit() => State.CurrentState == FileState.Scratch;

    public bool AreCommissionsAssigned() => State.CurrentState == FileState.InCommission;
}