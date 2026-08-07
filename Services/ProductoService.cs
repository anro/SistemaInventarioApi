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
	
	public async Task<ProductoDto> CrearAsync(ProductoCreateDto dto) //Insertar Producto
	{
		// Verificar que exista el proveedor
		var proveedorExiste = await _context.Proveedores
			.AnyAsync(p => p.ProveedorId == dto.ProveedorId);

		if (!proveedorExiste)
			throw new Exception("El proveedor no existe.");

		// Verificar que el código no esté repetido
		bool codigoExiste = await _context.Productos
			.AnyAsync(p => p.Codigo == dto.Codigo);

		if (codigoExiste)
			throw new Exception("Ya existe un producto con ese código.");

		var producto = new Producto
		{
			Codigo = dto.Codigo,
			Nombre = dto.Nombre,
			Descripcion = dto.Descripcion,
			PrecioCompra = dto.PrecioCompra,
			PrecioVenta = dto.PrecioVenta,
			Stock = dto.Stock,
			StockMinimo = dto.StockMinimo,
			ProveedorId = dto.ProveedorId,
			Activo = dto.Activo
		};

		_context.Productos.Add(producto);

		await _context.SaveChangesAsync();

		return new ProductoDto
		{
			ProductoId = producto.ProductoId,
			Codigo = producto.Codigo,
			Nombre = producto.Nombre,
			Descripcion = producto.Descripcion,
			PrecioCompra = producto.PrecioCompra,
			PrecioVenta = producto.PrecioVenta,
			Stock = producto.Stock,
			ProveedorId = producto.ProveedorId ?? 0,
			Activo = producto.Activo
		};
	}
	
	public async Task<ProductoDto?> ActualizarAsync( //Modificar producto
    int id,
    ProductoUpdateDto dto)
	{
		var producto = await _context.Productos
			.FirstOrDefaultAsync(p => p.ProductoId == id);

		if (producto == null)
			return null;

		var proveedorExiste = await _context.Proveedores
			.AnyAsync(p => p.ProveedorId == dto.ProveedorId);

		if (!proveedorExiste)
			throw new Exception("El proveedor no existe.");

		producto.Nombre = dto.Nombre;
		producto.Descripcion = dto.Descripcion;
		producto.PrecioVenta = dto.PrecioVenta;
		producto.StockMinimo = dto.StockMinimo;
		producto.ProveedorId = dto.ProveedorId;
		producto.Activo = dto.Activo;

		await _context.SaveChangesAsync();

		return new ProductoDto
		{
			ProductoId = producto.ProductoId,
			Codigo = producto.Codigo,
			Nombre = producto.Nombre,
			Descripcion = producto.Descripcion,
			PrecioCompra = producto.PrecioCompra,
			PrecioVenta = producto.PrecioVenta,
			Stock = producto.Stock,
			StockMinimo = producto.StockMinimo,
			ProveedorId = producto.ProveedorId ?? 0,
			Activo = producto.Activo
		};
	}
	
	public async Task<bool> EliminarAsync(int id) //Eliminar productos
	{
		var producto = await _context.Productos
			.FirstOrDefaultAsync(p => p.ProductoId == id);

		if (producto == null)
			return false;

		_context.Productos.Remove(producto);

		await _context.SaveChangesAsync();

		return true;
	}

}