using ApiInventario.Data;
using ApiInventario.DTOs;
using ApiInventario.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ApiInventario.Services;

public class UsuarioService : IUsuarioService
{
    private readonly AppDbContext _context;

    public UsuarioService(AppDbContext context) //Constructor
    {
        _context = context;
    }

    public async Task<List<UsuarioDto>> ObtenerUsuarios()  //1-Listar Usuarios
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                Nombre = u.Nombre,
                Email = u.Email,
                Rol = u.Rol.Nombre,
                Activo = u.Activo,
                FechaCreacion =  u.FechaCreacion ?? DateTime.Now
            })
            .ToListAsync();
    }

    public async Task<UsuarioDto?> ObtenerUsuario(int id)  //2-Buscar usuario por ID
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .Where(u => u.UsuarioId == id)
            .Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                Nombre = u.Nombre,
                Email = u.Email,
                Rol = u.Rol.Nombre,
                Activo = u.Activo,
				FechaCreacion = u.FechaCreacion ?? DateTime.Now
            })
            .FirstOrDefaultAsync();
    }

    public async Task<UsuarioDto> CrearUsuario(RegistroUsuarioDto dto) //3-Crear usuario por DTO
    {
        if (await ExisteEmail(dto.Email))
            throw new Exception("Ya existe un usuario con ese correo.");

        var rol = await _context.Roles
            .FirstOrDefaultAsync(r => r.RolId == dto.RolId);

        if (rol == null)
            throw new Exception("El rol no existe.");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RolId = dto.RolId,
            Activo = true,
            FechaCreacion = DateTime.Now
        };

        _context.Usuarios.Add(usuario);

        await _context.SaveChangesAsync();

        return new UsuarioDto
        {
            UsuarioId = usuario.UsuarioId,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = rol.Nombre,
            Activo = usuario.Activo,
            //FechaCreacion = usuario.FechaCreacion
			FechaCreacion = usuario.FechaCreacion ?? DateTime.Now
        };
    }

    public async Task<bool> ExisteEmail(string email) //4-Saber si el usurio tiene Email
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Email == email);
    }
	public async Task<Usuario?> ObtenerPorEmail(string email) //// 5-Obtener por Email el rol
	{
		return await _context.Usuarios
			.Include(u => u.Rol)
			.FirstOrDefaultAsync(u => u.Email == email);
	}
	
	public async Task<Usuario> Crear(Usuario usuario) //6- Crear usuario par ABM
	{
		_context.Usuarios.Add(usuario);

		await _context.SaveChangesAsync();

		return usuario;
	}
}