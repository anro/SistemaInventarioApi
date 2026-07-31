using ApiInventario.Data;
using ApiInventario.DTOs;
using ApiInventario.Models;
using ApiInventario.Security;
using Microsoft.EntityFrameworkCore;

namespace ApiInventario.Services;

public class AuthService : IAuthService
{
	private readonly IUsuarioService _usuarioService;
    private readonly AppDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly ITokenService _tokenService;

	public AuthService(
		AppDbContext context,
		PasswordService passwordService,
		ITokenService tokenService,
		IUsuarioService usuarioService)
	{
		_context = context;
		_passwordService = passwordService;
		_tokenService = tokenService;
		_usuarioService = usuarioService;
	}

    public async Task<TokenDto?> LoginAsync(LoginDto dto) //TOKEN
    {
        // Buscar usuario por email
		//var usuario = await _usuarioService.ObtenerPorEmail(dto.Email);
		var usuario = await _context.Usuarios
					.Include(u => u.Rol)
					.FirstOrDefaultAsync(u => u.Email == dto.Email);
		
		
		//Console.WriteLine("====== 1-DATOS LOGIN ======");
		//Console.WriteLine($"Usuario: {usuario.Nombre}");
		//Console.WriteLine($"RolId: {usuario.RolId}");
		//Console.WriteLine($"Rol cargado: {usuario.Rol != null}");
		//Console.WriteLine($"Nombre Rol: {usuario.Rol?.Nombre}");
		//Console.WriteLine("=========================");

        if (usuario == null)
            return null;

        // Verificar si está activo
        if (!usuario.Activo)
            return null;

        // Verificar contraseña
		bool passwordValido = _passwordService.Verificar(
			dto.Password,
			usuario.PasswordHash);

        if (!passwordValido)
            return null;
		


        // Generar token
        //string token = _tokenService.CrearToken(usuario);
		string token = _tokenService.CrearToken(
			usuario.UsuarioId,
			usuario.Nombre,
			usuario.Email ?? "",
			usuario.Rol?.Nombre ?? ""
		);

        return new TokenDto
        {
            Token = token,
            Nombre = usuario.Nombre,
            //Rol = usuario.Rol
			Rol = usuario.Rol?.Nombre ?? ""
        };
    }

    public async Task<bool> RegistrarAsync(RegistroUsuarioDto dto) //REGISTRAR USUARIO por DTO
    {
        // Verificar si el email ya existe
        if (await _usuarioService.ExisteEmail(dto.Email))
            return false;

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            PasswordHash = _passwordService.Encriptar(dto.Password),
            RolId = dto.RolId,
            Activo = true,
            FechaCreacion = DateTime.Now
        };

        await _usuarioService.Crear(usuario);

        return true;
    }



}