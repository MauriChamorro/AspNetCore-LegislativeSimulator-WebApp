namespace WebAppMVC.Domain.Models.Projects;

public class Commission
{
    public int CommissionId { get; set; }
    public string Name { get; set; }
    public List<string> WordsForAssingment { get; set; }
}