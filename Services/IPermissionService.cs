namespace ApiInventario.Services;

public interface IPermissionService
{
    Task<bool> TienePermisoAsync(int usuarioId, string permiso);
}