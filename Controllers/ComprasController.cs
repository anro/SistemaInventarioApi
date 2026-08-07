using ApiInventario.DTOs;
using ApiInventario.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiInventario.Security;

namespace ApiInventario.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ComprasController : ControllerBase
{
    private readonly ICompraService _service;

	public ComprasController(ICompraService service)
	{
		_service = service;
	}
	
	[Permiso("COMPRAS_CREAR")]  //CREAR COMPRA
	[HttpPost]
	public async Task<IActionResult> Crear(CompraDto dto)
	{
		var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

		if (claim == null)
		{
			return Unauthorized();
		}

		dto.UsuarioId = int.Parse(claim.Value);

		var compra = await _service.CrearCompra(dto);

		return Ok(new
		{
			mensaje = "Compra registrada correctamente",
			compraId = compra.CompraId,
			proveedorId = compra.ProveedorId,
			usuarioId = compra.UsuarioId,
			fecha = compra.Fecha
		});
	}
	
	[Permiso("COMPRAS_VER")] // VER TODAS LAS COMPRAS
	[HttpGet]
	public async Task<IActionResult> ObtenerCompras()
	{
		var compras = await _service.ObtenerCompras();
		return Ok(compras);
	}

	[Permiso("COMPRAS_VER")] //BUSCAR COMPRA	
	[HttpGet("{id}")]
	public async Task<IActionResult> ObtenerCompra(int id)
	{
		var compra = await _service.ObtenerCompra(id);

		if (compra == null)
			return NotFound("Compra no encontrada");

		return Ok(compra);
	}
}