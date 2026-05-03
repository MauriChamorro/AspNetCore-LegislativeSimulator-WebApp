namespace WebAppMVC.Domain.Models.Projects;

public class ProjectCommissions
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public List<Commission> Commissions { get; set; }
}