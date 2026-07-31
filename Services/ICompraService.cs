using ApiInventario.DTOs;
using ApiInventario.Models;

namespace ApiInventario.Services;

public interface ICompraService
{
    Task<Compra> CrearCompra(CompraDto dto);
	Task<List<CompraResponseDto>> ObtenerCompras();
    Task<CompraResponseDto?> ObtenerCompra(int id);
}