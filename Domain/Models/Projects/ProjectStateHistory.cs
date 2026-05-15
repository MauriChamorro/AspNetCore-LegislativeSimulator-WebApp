namespace WebAppMVC.Domain.Models.Projects;

public class ProjectStateHistory
{
    public int HistoryId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
    public ProjectState ProjectState { get; set; }
}