namespace WebAppMVC.Domain.Models.Projects;

public class ProjectState
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public FileState State { get; set; }
}

public enum FileState
{
    Scratch,
    PendingForAssignCommissions,
    InCommission,
    RejectedByCommissions,
    InSession,
    ApprovedInSession,
    RejectedInSession,
    DeletedByLawmaker
}