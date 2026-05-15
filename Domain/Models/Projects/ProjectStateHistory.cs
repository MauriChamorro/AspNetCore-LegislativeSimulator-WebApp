namespace WebAppMVC.Domain.Models.Projects;

public class ProjectStateHistory
{
    public DateTime Date { get; set; }
    public ProjectState ProjectState { get; set; }
}