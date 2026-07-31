using ApiInventario.DTOs;
using ApiInventario.Models;

namespace ApiInventario.Services;

public interface IVentaService
{
    Task<Venta> CrearVenta(VentaDto dto);
	Task<IEnumerable<Venta>> ObtenerVentas();
    Task<Venta?> ObtenerVentaPorId(int id);
}