namespace WebAppMVC.Domain.Models.Projects;

public class ProjectState
{
    public FileState CurrentState { get; set; }
    public DateTime ChangeDate { get; set; }
    
    public string GetNameState(FileState fileState) =>
        fileState switch
        {
            FileState.Scratch => "Borrador",
            FileState.PendingForAssignCommissions => "Asignando Comisiones",
            FileState.InCommission => "En Comisiones",
            FileState.RejectedByCommissions => "Rechazado por Comisiones",
            FileState.InSession => "En Sesión",
            FileState.ApprovedInSession => "Aprobado en Sesión",
            FileState.RejectedInSession => "Recahzado en Sesión",
            FileState.DeletedByLawmaker => "Eliminado",
            _ => "none"
        };
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