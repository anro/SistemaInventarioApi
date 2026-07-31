using ApiInventario.Models;
using ApiInventario.DTOs;

namespace ApiInventario.Services;

public interface IUsuarioService
{
	Task<List<UsuarioDto>> ObtenerUsuarios(); ////1-Listar Usuarios

    Task<UsuarioDto?> ObtenerUsuario(int id); // 2-Buscar Usuario

    Task<UsuarioDto> CrearUsuario(RegistroUsuarioDto dto); // 3-Crear usuarios
	
	Task<bool> ExisteEmail(string email); //4-Preguntar si tiene Email
	
	Task<Usuario?> ObtenerPorEmail(string email);// 5-Obtener por Email el rol
	
	Task<Usuario> Crear(Usuario usuario);// 6- Crear usuario
}