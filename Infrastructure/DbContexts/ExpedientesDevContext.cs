using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebAppMVC.Infrastructure.Entities;

namespace WebAppMVC.Infrastructure.DbContexts;

public partial class ExpedientesDevContext : DbContext
{
    public ExpedientesDevContext(DbContextOptions<ExpedientesDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Commission> Commissions { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectState> ProjectStates { get; set; }

    public virtual DbSet<ProjectStateHistory> ProjectStateHistories { get; set; }

    public virtual DbSet<Referral> Referrals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Commission>(entity =>
        {
            entity.HasKey(e => e.CommissionId).HasName("PK_Commission");

            entity.Property(e => e.CommissionId)
                .ValueGeneratedNever()
                .HasColumnName("commissionId");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("name");
            entity.Property(e => e.Tags)
                .HasMaxLength(500)
                .IsFixedLength()
                .HasColumnName("tags");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK_Project");

            entity.Property(e => e.ProjectId).HasColumnName("projectId");
            entity.Property(e => e.Articles).HasColumnName("articles");
            entity.Property(e => e.FileId)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("fileId");
            entity.Property(e => e.Fundaments)
                .HasMaxLength(500)
                .IsFixedLength()
                .HasColumnName("fundaments");
            entity.Property(e => e.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Summary)
                .HasMaxLength(200)
                .IsFixedLength()
                .HasColumnName("summary");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("title");
        });

        modelBuilder.Entity<ProjectState>(entity =>
        {
            entity.HasKey(e => e.ProjectStateId).HasName("PK_ProjectState");

            entity.Property(e => e.ProjectStateId)
                .ValueGeneratedNever()
                .HasColumnName("projectStateId");
            entity.Property(e => e.IntState).HasColumnName("intState");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("name");
        });

        modelBuilder.Entity<ProjectStateHistory>(entity =>
        {
            entity.HasKey(e => new { e.ProjectId, e.ProjectStateId }).HasName("PK_ProjectStateHistory");

            entity.Property(e => e.ProjectId).HasColumnName("projectId");
            entity.Property(e => e.ProjectStateId).HasColumnName("projectStateId");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectStateHistories)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectStateHistory_Project");

            entity.HasOne(d => d.ProjectState).WithMany(p => p.ProjectStateHistories)
                .HasForeignKey(d => d.ProjectStateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectStateHistories_ProjectState");
        });

        modelBuilder.Entity<Referral>(entity =>
        {
            entity.HasKey(e => new { e.ProjectId, e.CommissionId }).HasName("PK_ProjectCommission");

            entity.Property(e => e.ProjectId).HasColumnName("projectId");
            entity.Property(e => e.CommissionId).HasColumnName("commissionId");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.FileId)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("fileId");
            entity.Property(e => e.State).HasColumnName("state");

            entity.HasOne(d => d.Commission).WithMany(p => p.Referrals)
                .HasForeignKey(d => d.CommissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectCommissions_Commission");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
