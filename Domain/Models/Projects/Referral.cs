namespace WebAppMVC.Domain.Models.Projects;

public class Referral
{
    public int ProjectId { get; set; }
    public int CommissionId { get; set; }
    public DateTime DateState { get; set; }
    public ReferralState State { get; set; }
    
    public virtual Commission Commission { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}

public enum ReferralState
{
    Assigned,
    Evaluating,
    Accepted,
    Rejected
}