using System;
using System.Collections.Generic;

namespace WebAppMVC.Infrastructure.Entities;

public partial class ProjectStateHistory
{
    public int HistoryId { get; set; }

    public DateTime Date { get; set; }

    public int ProjectStateId { get; set; }

    public virtual ProjectState ProjectState { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
