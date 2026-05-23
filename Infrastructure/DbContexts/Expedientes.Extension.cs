using Microsoft.EntityFrameworkCore;
using WebAppMVC.Infrastructure.Entities;

namespace WebAppMVC.Infrastructure.DbContexts;

public partial class ExpedientesDevContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        // se puede reescribir lo que genera EF
        modelBuilder.Entity<Project>()
            .Property(p => p.RowVersion)
            .IsRowVersion();
    }
}