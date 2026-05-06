namespace WebAppMVC.Domain.Models.Projects;

public class AssignedCommissions
{
    public int ProjectId { get; set; }
    public List<Commission> Commissions { get; set; }
}