using CocinaManager.Application.Interfaces;
using CocinaManager.Domain.Entities;
using CocinaManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CocinaManager.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly CocinaDbContext _context;

    public UsuarioRepository(CocinaDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByCredencialesAsync(string nombreUsuario, string passwordHash)
        => await _context.Usuarios
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario
                                   && u.PasswordHash == passwordHash
                                   && u.Activo);
}