namespace WebAppMVC.Domain.Models.Projects;

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