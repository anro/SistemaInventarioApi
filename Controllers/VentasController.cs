using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ApiInventario.Models;
//using ApiInventario.Data;
using ApiInventario.DTOs;
using ApiInventario.Services;
using Microsoft.AspNetCore.Authorization;

namespace ApiInventario.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VentasController : ControllerBase
{
	//private readonly AppDbContext _context;
	/*public VentasController(AppDbContext context)
	{
		_context = context;
	}*/
	
	private readonly IVentaService _service;
	
	public VentasController(IVentaService service)
	{
		_service = service;
	}
	
	[HttpPost]
	public async Task<IActionResult> Crear(VentaDto dto)
	{
		dto.Fecha = DateTime.Now;
		//_context.Ventas.Add(venta);
		//await _context.SaveChangesAsync();

		//return Ok(venta);
		var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

		if (claim == null)
		{
			return Unauthorized();
		}

		dto.UsuarioId = int.Parse(claim.Value);
		
		var venta = await _service.CrearVenta(dto);

		 return Ok(new
		{
			mensaje = "Venta registrada correctamente",
			VentaId = venta.VentaId,
			clienteId = venta.ClienteId,
			usuarioId = venta.UsuarioId,
			fecha = venta.Fecha
		});
	}


	[HttpGet]
	public async Task<IActionResult> ObtenerVentas()
	{
		var ventas = await _service.ObtenerVentas();

		return Ok(ventas);
	}
	
	[HttpGet("{id}")]
	public async Task<IActionResult> ObtenerVenta(int id)
	{
		var venta = await _service.ObtenerVentaPorId(id);

		if (venta == null)
			return NotFound("Venta no encontrada");

		return Ok(venta);
	}
	
}


