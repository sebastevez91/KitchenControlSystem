using CocinaManager.API.CocinaManager.Domain;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.API.CocinaManager.Infrastructure.Data;

public class CocinaDbContext : DbContext
{
    public CocinaDbContext(DbContextOptions<CocinaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Personal> Personal { get; set; }
    public DbSet<Producto> Productos { get; set; }
}