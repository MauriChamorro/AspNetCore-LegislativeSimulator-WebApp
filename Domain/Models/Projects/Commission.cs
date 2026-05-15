namespace WebAppMVC.Domain.Models.Projects;

public class Commission
{
    public int CommissionId { get; set; }
    public string Name { get; set; }
    public string? Tags { get; set; }
    public List<string>? WordsForAssignment { get; set; }
    
    public virtual ICollection<Referral> Referrals { get; set; } = new List<Referral>();
}