using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAppMVC.DbFirstModels;

public partial class Project
{
    [Key]
    [Column("projectId")]
    [StringLength(30)]
    public string ProjectId { get; set; } = null!;

    [Column("title")]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [Column("articles")]
    [StringLength(500)]
    public string Articles { get; set; } = null!;

    [Column("fundaments")]
    [StringLength(500)]
    public string Fundaments { get; set; } = null!;

    [Column("summary")]
    [StringLength(200)]
    public string Summary { get; set; } = null!;

    [Column("state")]
    public int State { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<Referral> Referrals { get; set; } = new List<Referral>();
}
