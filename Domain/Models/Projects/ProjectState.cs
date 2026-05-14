namespace WebAppMVC.Domain.Models.Projects;

public partial class ProjectState
{
    public int StateId { get; set; }

    public string Name { get; set; } = null!;
    public FileState State { get; set; }
    public DateTime Date { get; set; }
    
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    
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
            FileState.DeletedByLawmaker => "Elimado por Legislador",
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