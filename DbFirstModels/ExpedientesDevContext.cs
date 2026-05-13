using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAppMVC.DbFirstModels;

public partial class ExpedientesDevContext : DbContext
{
    public ExpedientesDevContext()
    {
    }

    public ExpedientesDevContext(DbContextOptions<ExpedientesDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Commission> Commissions { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Referral> Referrals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DbConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Commission>(entity =>
        {
            entity.HasKey(e => e.CommissionId).HasName("PK_Commission");

            entity.Property(e => e.CommissionId).ValueGeneratedNever();
            entity.Property(e => e.Name).IsFixedLength();
            entity.Property(e => e.Tags).IsFixedLength();
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK_Project");

            entity.Property(e => e.ProjectId).IsFixedLength();
            entity.Property(e => e.Articles).IsFixedLength();
            entity.Property(e => e.Fundaments).IsFixedLength();
            entity.Property(e => e.Summary).IsFixedLength();
            entity.Property(e => e.Title).IsFixedLength();
        });

        modelBuilder.Entity<Referral>(entity =>
        {
            entity.HasKey(e => new { e.ProjectId, e.CommissionId }).HasName("PK_ProjectCommission");

            entity.Property(e => e.ProjectId).IsFixedLength();

            entity.HasOne(d => d.Commission).WithMany(p => p.Referrals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectCommissions_Commission");

            entity.HasOne(d => d.Project).WithMany(p => p.Referrals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectCommissions_Project");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
