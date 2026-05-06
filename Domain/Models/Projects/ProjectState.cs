namespace WebAppMVC.Domain.Models.Projects;

public class ProjectState
{
    public FileState CurrentState { get; set; }
    public DateTime ChangeDate { get; set; }
}