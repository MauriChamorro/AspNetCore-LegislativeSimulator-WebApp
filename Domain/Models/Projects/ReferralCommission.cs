namespace WebAppMVC.Domain.Models.Projects;

public class ReferralCommission
{
    public int ProjectId { get; set; }
    public int CommissionId { get; set; }
    public string CommisionName { get; set; }
    public DateTime ReferralDate { get; set; }
    public ReferralCommissionState State { get; set; }
}