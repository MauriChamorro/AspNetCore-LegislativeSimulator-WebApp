namespace WebAppMVC.Domain.Models.Projects;

public class ProjectState
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public FileState State { get; set; }
    
    public string GetNameState(FileState fileState) =>
        fileState switch
        {
            FileState.Scratch => "Borrador",
            FileState.PendingForAssignCommissions => "Enviado a Comisiones",
            FileState.InCommission => "En Comisiones",
            FileState.RejectedByCommissions => "Rechazado por Comisiones",
            FileState.InSession => "En Sesión",
            FileState.ApprovedInSession => "Aprobado",
            FileState.RejectedInSession => "Rechazado en Sesión",
            FileState.DeletedByLawmaker => "Eliminado por Legislador",
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