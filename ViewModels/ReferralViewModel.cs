namespace WebAppMVC.ViewModels;

public class ReferralViewModel
{
    public int CommissionId { get; set; }
    public string CommissionName { get; set; }
    public string ReferralStateName { get; set; }
    public DateTime ReferralDate { get; set; }
    public string BackgroundColor { get; set; }
    public string Color { get; set; }
}