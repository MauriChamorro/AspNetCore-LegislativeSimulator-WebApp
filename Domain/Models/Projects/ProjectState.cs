namespace WebAppMVC.Domain.Models.Projects;

public class ProjectState
{
    public int Id { get; set; }
    public string ProjectStateName { get; set; }
    public DateTime ChangeDate { get; set; }
}