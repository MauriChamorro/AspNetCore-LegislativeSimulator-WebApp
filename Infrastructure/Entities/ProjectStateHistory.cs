using System;
using System.Collections.Generic;

namespace WebAppMVC.Infrastructure.Entities;

public partial class ProjectStateHistory
{
    public int ProjectId { get; set; }

    public int ProjectStateId { get; set; }

    public DateTime Date { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ProjectState ProjectState { get; set; } = null!;
}
