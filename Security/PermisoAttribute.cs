using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ApiInventario.Services;
using System.Security.Claims;

namespace ApiInventario.Security;

public class PermisoAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _permiso;


    public PermisoAttribute(string permiso)
    {
        _permiso = permiso;
    }


    public async Task OnAuthorizationAsync(
        AuthorizationFilterContext context)
    {
        var permissionService =
            context.HttpContext.RequestServices
            .GetRequiredService<IPermissionService>();


        var usuarioIdClaim =
            context.HttpContext.User
            .FindFirst(ClaimTypes.NameIdentifier);


        if (usuarioIdClaim == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }


        int usuarioId =
            int.Parse(usuarioIdClaim.Value);


        bool tienePermiso =
            await permissionService
            .TienePermisoAsync(usuarioId, _permiso);


        if (!tienePermiso)
        {
            context.Result = new ForbidResult();
        }
    }
}