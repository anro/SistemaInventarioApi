using ApiInventario.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ApiInventario.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportesController : ControllerBase
{
    private readonly IReporteService _service;

    public ReportesController(IReporteService service)
    {
        _service = service;
    }

    //==========================================
    // Ventas por fecha
    //==========================================
    [HttpGet("ventas")]
    public async Task<IActionResult> VentasPorFecha(
        DateTime desde,
        DateTime hasta)
    {
        var resultado = await _service.ObtenerVentasPorFecha(desde, hasta);

        return Ok(resultado);
    }

    //==========================================
    // Compras por fecha
    //==========================================
    [HttpGet("compras")]
    public async Task<IActionResult> ComprasPorFecha(
        DateTime desde,
        DateTime hasta)
    {
        var resultado = await _service.ObtenerComprasPorFecha(desde, hasta);

        return Ok(resultado);
    }

    //==========================================
    // Productos más vendidos
    //==========================================
    [HttpGet("productos-mas-vendidos")]
    public async Task<IActionResult> ProductosMasVendidos()
    {
        var resultado = await _service.ObtenerProductosMasVendidos();

        return Ok(resultado);
    }

    //==========================================
    // Productos sin stock
    //==========================================
    [HttpGet("productos-sin-stock")]
    public async Task<IActionResult> ProductosSinStock()
    {
        var resultado = await _service.ObtenerProductosSinStock();

        return Ok(resultado);
    }

    //==========================================
    // Productos bajo stock mínimo
    //==========================================
    [HttpGet("productos-bajo-stock")]
    public async Task<IActionResult> ProductosBajoStock()
    {
        var resultado = await _service.ObtenerProductosBajoStock();

        return Ok(resultado);
    }
}