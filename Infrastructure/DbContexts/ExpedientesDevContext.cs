using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebAppMVC.Infrastructure.Entities;

namespace WebAppMVC.Infrastructure.DbContexts;

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

    public virtual DbSet<ProjectState> ProjectStates { get; set; }

    public virtual DbSet<Referral> Referrals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DbConnection");

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

            entity.Property(e => e.ProjectId)
                .ValueGeneratedNever()
                .HasColumnName("projectId");
            entity.Property(e => e.Articles).HasColumnName("articles");
            entity.Property(e => e.Fundaments)
                .HasMaxLength(500)
                .IsFixedLength()
                .HasColumnName("fundaments");
            entity.Property(e => e.NumExpediente)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("numExpediente");
            entity.Property(e => e.StateId).HasColumnName("stateId");
            entity.Property(e => e.Summary)
                .HasMaxLength(200)
                .IsFixedLength()
                .HasColumnName("summary");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("title");

            entity.HasOne(d => d.State).WithMany(p => p.Projects)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProjectState");
        });

        modelBuilder.Entity<ProjectState>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK_ProjectState");

            entity.Property(e => e.StateId)
                .ValueGeneratedNever()
                .HasColumnName("stateId");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("name");
            entity.Property(e => e.State).HasColumnName("state");
        });

        modelBuilder.Entity<Referral>(entity =>
        {
            entity.HasKey(e => new { e.ProjectId, e.CommissionId }).HasName("PK_ProjectCommission");

            entity.Property(e => e.ProjectId)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("projectId");
            entity.Property(e => e.CommissionId).HasColumnName("commissionId");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
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
