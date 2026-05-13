using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAppMVC.DbFirstModels;

[PrimaryKey("ProjectId", "CommissionId")]
public partial class Referral
{
    [Key]
    [Column("projectId")]
    [StringLength(30)]
    public string ProjectId { get; set; } = null!;

    [Key]
    [Column("commissionId")]
    public int CommissionId { get; set; }

    [Column("date", TypeName = "datetime")]
    public DateTime? Date { get; set; }

    [ForeignKey("CommissionId")]
    [InverseProperty("Referrals")]
    public virtual Commission Commission { get; set; } = null!;

    [ForeignKey("ProjectId")]
    [InverseProperty("Referrals")]
    public virtual Project Project { get; set; } = null!;
}
