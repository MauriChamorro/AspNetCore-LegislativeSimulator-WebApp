using System;
using System.Collections.Generic;

namespace WebAppMVC.Infrastructure.Entities;

public partial class Referral
{
    public int ProjectId { get; set; }

    public int CommissionId { get; set; }

    public string? FileId { get; set; }

    public DateTime Date { get; set; }

    public int State { get; set; }

    public virtual Commission Commission { get; set; } = null!;
}
