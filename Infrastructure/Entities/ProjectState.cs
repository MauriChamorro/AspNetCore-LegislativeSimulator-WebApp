using System;
using System.Collections.Generic;

namespace WebAppMVC.Infrastructure.Entities;

public partial class ProjectState
{
    public int ProjectStateId { get; set; }

    public string Name { get; set; } = null!;

    public int IntState { get; set; }

    public virtual ICollection<ProjectStateHistory> ProjectStateHistories { get; set; } = new List<ProjectStateHistory>();
}
