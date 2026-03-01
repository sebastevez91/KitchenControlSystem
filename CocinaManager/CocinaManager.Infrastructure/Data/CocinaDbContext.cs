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
    public DbSet<Herramienta> Herramientas { get; set; }
    public DbSet<OrdenMantenimiento> OrdenesMantenimiento { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Receta> Recetas { get; set; }
    public DbSet<RecetaIngrediente> RecetaIngredientes { get; set; }
    public DbSet<PlanMenu> PlanesMenu { get; set; }
    public DbSet<Incidente> Incidentes { get; set; }
    public DbSet<Ausencia> Ausencias { get; set; }
    public DbSet<Mensaje> Mensajes { get; set; }

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

        // Herramienta
        modelBuilder.Entity<Herramienta>(entity =>
        {
            entity.HasKey(h => h.Id);
            entity.Property(h => h.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(h => h.Descripcion).HasMaxLength(500);
        });

        // OrdenMantenimiento
        modelBuilder.Entity<OrdenMantenimiento>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.Descripcion).IsRequired().HasMaxLength(500);
            entity.Property(o => o.Observaciones).HasMaxLength(500);
            entity.HasOne(o => o.Herramienta)
                  .WithMany()
                  .HasForeignKey(o => o.HerramientaId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.NombreUsuario).IsRequired().HasMaxLength(50);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.HasIndex(u => u.NombreUsuario).IsUnique();
        });

        // Receta
        modelBuilder.Entity<Receta>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Nombre).IsRequired().HasMaxLength(150);
            entity.Property(r => r.Descripcion).HasMaxLength(1000);
            entity.HasMany(r => r.Ingredientes)
                  .WithOne(i => i.Receta)
                  .HasForeignKey(i => i.RecetaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RecetaIngrediente>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Cantidad).HasColumnType("decimal(18,2)");
            entity.Property(i => i.UnidadMedida).IsRequired().HasMaxLength(50);
        });

        // Menú
        modelBuilder.Entity<PlanMenu>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.TipoComida).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Observaciones).HasMaxLength(500);
            entity.HasOne(p => p.Receta)
                  .WithMany()
                  .HasForeignKey(p => p.RecetaId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Incidentes
        modelBuilder.Entity<Incidente>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Descripcion).IsRequired().HasMaxLength(1000);
            entity.Property(i => i.RegistradoPor).IsRequired().HasMaxLength(100);
            entity.Property(i => i.Observaciones).HasMaxLength(500);
        });

        // Ausencias
        modelBuilder.Entity<Ausencia>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.RegistradoPor).IsRequired().HasMaxLength(100);
            entity.HasOne(a => a.Personal)
                  .WithMany()
                  .HasForeignKey(a => a.PersonalId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.Ignore(a => a.DiasAusencia);
        });

        // Mensajes
        modelBuilder.Entity<Mensaje>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Remitente).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Destinatario).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Asunto).IsRequired().HasMaxLength(200);
            entity.Property(m => m.Cuerpo).IsRequired().HasMaxLength(2000);
            entity.HasOne(m => m.MensajePadre)
                  .WithMany()
                  .HasForeignKey(m => m.MensajePadreId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);
        });
    }
}
