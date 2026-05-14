using System;
using System.Collections.Generic;

namespace WebAppMVC.Infrastructure.Entities;

public partial class Project
{
    public string ProjectId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Articles { get; set; } = null!;

    public string Fundaments { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public int StateId { get; set; }

    public virtual ICollection<Referral> Referrals { get; set; } = new List<Referral>();

    public virtual ProjectState State { get; set; } = null!;
}
