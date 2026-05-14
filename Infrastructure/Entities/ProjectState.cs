using System;
using System.Collections.Generic;

namespace WebAppMVC.Infrastructure.Entities;

public partial class ProjectState
{
    public int StateId { get; set; }

    public string Name { get; set; } = null!;

    public int State { get; set; }

    public DateTime? Date { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
