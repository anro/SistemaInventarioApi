
using ApiInventario.DTOs;

namespace ApiInventario.Services;

public interface IReporteService
{
    Task<List<VentaReporteDto>> ObtenerVentasPorFecha(DateTime desde, DateTime hasta);

    Task<List<CompraReporteDto>> ObtenerComprasPorFecha(DateTime desde, DateTime hasta);

    Task<List<ProductoVentaDto>> ObtenerProductosMasVendidos();

    Task<List<ProductoDto>> ObtenerProductosSinStock();

    Task<List<ProductoDto>> ObtenerProductosBajoStock();
}

