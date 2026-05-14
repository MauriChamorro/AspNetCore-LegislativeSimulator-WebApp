using System;
using System.Collections.Generic;

namespace WebAppMVC.Infrastructure.Entities;

public partial class Referral
{
    public string ProjectId { get; set; } = null!;

    public int CommissionId { get; set; }

    public DateTime Date { get; set; }

    public int State { get; set; }

    public virtual Commission Commission { get; set; } = null!;
}
