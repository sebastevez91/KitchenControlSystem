using Microsoft.EntityFrameworkCore;
using CocinaManager.Domain.Entities;

namespace CocinaManager.Infrastructure.Data;

public class CocinaDbContext : DbContext
{
    public CocinaDbContext(DbContextOptions<CocinaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Personal> Personal { get; set; }
    public DbSet<Turno> Turnos { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<MovimientoStock> MovimientosStock { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Personal
        modelBuilder.Entity<Personal>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(p => p.Documento).IsRequired().HasMaxLength(20);
            entity.Property(p => p.Cargo).IsRequired().HasMaxLength(100);
        });

        // Turno
        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.HasOne(t => t.Personal)
                  .WithMany()
                  .HasForeignKey(t => t.PersonalId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Producto
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(p => p.UnidadMedida).IsRequired().HasMaxLength(50);
            entity.Property(p => p.StockActual).HasColumnType("decimal(18,2)");
            entity.Property(p => p.StockMinimo).HasColumnType("decimal(18,2)");
        });

        // MovimientoStock
        modelBuilder.Entity<MovimientoStock>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.HasOne(m => m.Producto)
                  .WithMany()
                  .HasForeignKey(m => m.ProductoId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.Property(m => m.Cantidad).HasColumnType("decimal(18,2)");
        });
    }
}
