using ApiInventario.Data;
using ApiInventario.DTOs;
using ApiInventario.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiInventario.Services;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _context;

    public ProductoService(AppDbContext context)
    {
        _context = context;
    }

    // aquí irán los métodos
	public async Task<IEnumerable<ProductoDto>> ObtenerTodosAsync() //Listar todos los productos
	{
		return await _context.Productos
			.Include(p => p.ProveedorId )
			.Select(p => new ProductoDto
			{
				ProductoId = p.ProductoId,
				Codigo = p.Codigo,
				Nombre = p.Nombre,
				Descripcion = p.Descripcion,
				PrecioCompra = p.PrecioCompra,
				PrecioVenta = p.PrecioVenta,
				Stock = p.Stock,
				StockMinimo = p.StockMinimo,
				Activo = p.Activo,
				//ProveedorId = p.ProveedorId,
				//Proveedor = p.Proveedor.Nombre
			})
			.ToListAsync();
	}
	
	public async Task<ProductoDto?> ObtenerPorIdAsync(int id) //Obtener 1 producto por Id
	{
		return await _context.Productos
			.Include(p => p.ProductoId)
			.Where(p => p.ProductoId == id)
			.Select(p => new ProductoDto
			{
				ProductoId = p.ProductoId,
				Codigo = p.Codigo,
				Nombre = p.Nombre,
				Descripcion = p.Descripcion,
				PrecioCompra = p.PrecioCompra,
				PrecioVenta = p.PrecioVenta,
				Stock = p.Stock,
				StockMinimo = p.StockMinimo,
				Activo = p.Activo,
				//ProveedorId = p.ProveedorId,
				//Proveedor = p.Proveedor.Nombre
			})
			.FirstOrDefaultAsync();
	}
	
	
	
}