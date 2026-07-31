namespace ApiInventario.Services;

public interface ITokenService
{
    string CrearToken(
        int usuarioId,
        string nombre,
        string email,
        string rol);
}