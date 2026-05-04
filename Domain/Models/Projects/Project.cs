namespace WebAppMVC.Domain.Models.Projects;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Articles { get; set; }
    public string Fundaments { get; set; }
    public string Summary { get; set; }
    public List<ProjectTag> Tags { get; set; }
    public int ProjectStateId { get; set; }
    public ProjectState State { get; set; }
    public int LawmakerId { get; set; }
    public Lawmaker Lawmaker { get; set; }
}