using System;
using System.Collections.Generic;

namespace WebAppMVC.Infrastructure.Entities;

public partial class Project
{
    public int ProjectId { get; set; }

    public string NumExpediente { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Articles { get; set; } = null!;

    public string Fundaments { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public int StateId { get; set; }

    public virtual ProjectState State { get; set; } = null!;
}
