using Microsoft.EntityFrameworkCore;
using ApiInventario.Data;

namespace ApiInventario.Services;

public class PermissionService : IPermissionService
{
    private readonly AppDbContext _context;

    public PermissionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> TienePermisoAsync(int usuarioId, string permiso)
    {
        return await _context.Usuarios
            .Where(u => u.UsuarioId == usuarioId && u.Activo)
            .SelectMany(u => u.Rol.RolPermisos)
            .AnyAsync(rp =>
                rp.Permiso.Nombre == permiso &&
                rp.Permiso.Activo);
    }
}

/*
Petición HTTP
      |
      v
[Authorize]  ← valida JWT
      |
      v
[Permiso("PRODUCTOS_CREAR")]
      |
      v
PermissionService
      |
      +---- Tiene permiso → continúa al Controller
      |
      +---- No tiene permiso → 403 Forbidden
*/