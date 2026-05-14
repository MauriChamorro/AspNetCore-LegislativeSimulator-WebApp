using System;
using System.Collections.Generic;

namespace WebAppMVC.Infrastructure.Entities;

public partial class Commission
{
    public int CommissionId { get; set; }

    public string Name { get; set; } = null!;

    public string? Tags { get; set; }

    public virtual ICollection<Referral> Referrals { get; set; } = new List<Referral>();
}
