using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAppMVC.DbFirstModels;

public partial class Commission
{
    [Key]
    [Column("commissionId")]
    public int CommissionId { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("tags")]
    [StringLength(500)]
    public string? Tags { get; set; }

    [InverseProperty("Commission")]
    public virtual ICollection<Referral> Referrals { get; set; } = new List<Referral>();
}
