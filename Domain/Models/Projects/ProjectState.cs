namespace WebAppMVC.Domain.Models.Projects;

public class ProjectState
{
    public int Id { get; private set; }
    public string ProjectStateName { get; private set; }
    public DateTime ChangeDate { get; private set; }

    public static readonly ProjectState Scratch = new(1, "Borrador");
    public static readonly ProjectState InCommission =  new(2, "En Comisión");
    public static readonly ProjectState InSession =  new(3,"En Sesión");
    public static readonly ProjectState Sanctioned =  new(4,"Sancionado");
    public static readonly ProjectState Rejected =  new(5,"Rechazado");
    public static readonly ProjectState Removed =  new(6,"Removido");

    private ProjectState(int id, string name)
    {
        Id = id;
        ProjectStateName = name;
        ChangeDate = DateTime.Now;
    }
}