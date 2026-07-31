using ApiInventario.Data;
using ApiInventario.DTOs;
using ApiInventario.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiInventario.Services;

public class CompraService : ICompraService
{

    private readonly AppDbContext _context;

    public CompraService(AppDbContext context)
    {
        _context = context;
    }
	
	//CREAR COMPRA
    public async Task<Compra> CrearCompra(CompraDto dto)
    {
        var proveedor = await _context.Proveedores
            .FindAsync(dto.ProveedorId);

        if(proveedor == null)
			throw new InvalidOperationException("Proveedor no existe");
           // throw new Exception("Proveedor no existe");
		
		if (dto.Detalles == null || !dto.Detalles.Any())
		{
			//throw new Exception("La compra debe tener al menos un detalle.");
			throw new InvalidOperationException("La compra debe tener al menos un detalle.");
		}

        var compra = new Compra
        {
            ProveedorId = dto.ProveedorId,
            Fecha = dto.Fecha,
			UsuarioId = dto.UsuarioId,
            Observacion = dto.Observacion
        };
	
		// Guardar cabecera + detalle
		compra.Total = dto.Detalles.Sum(x => x.Cantidad * x.Precio);
		_context.Compras.Add(compra);
        await _context.SaveChangesAsync();
		
		//Compras detalle
        foreach (var item in dto.Detalles)
        {
            var producto = await _context.Productos
                .FindAsync(item.ProductoId);

            if(producto == null)
				throw new InvalidOperationException("Producto no existe");
                //throw new Exception("Producto no existe");
			

            var detalle = new CompraDetalle
            {
                CompraId = compra.CompraId,
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
                Precio = item.Precio
            };
			
			if(detalle.Cantidad <= 0)
			{
				/*throw new Exception(
					$"La cantidad del producto {detalle.ProductoId} debe ser mayor a cero"
				);*/
				throw new InvalidOperationException("La cantidad debe ser mayor a cero");
			}
			
			if(detalle.Precio <= 0)
			{
				throw new InvalidOperationException("El precio debe ser mayor a cero");
			}

            _context.CompraDetalle.Add(detalle);

		    // Buscar producto
			var product = await _context.Productos
             .FindAsync(item.ProductoId);

			if(product != null)
			{
				// AQUÍ está la lógica de inventario sin movimientos stock
				producto.Stock += item.Cantidad;
				producto.PrecioCompra = item.Precio;
				// Crear movimiento
				var movimiento = new MovimientoStock
				{
					ProductoId = item.ProductoId ,
					TipoMovimiento = "ENTRADA",
					Cantidad = item.Cantidad,
					Fecha = DateTime.Now,
					Referencia = 
						"Compra Nro " + compra.CompraId
				};
				_context.MovimientosStock.Add(movimiento);
			}
        }
        await _context.SaveChangesAsync();
        //return "Compra registrada correctamente";
		return compra;
    }
	
	//Obtener varias Compras
	public async Task<List<CompraResponseDto>> ObtenerCompras()
	{
		return await _context.Compras
		.Include(c => c.Proveedor)
		.Include(c => c.Detalles)
		.ThenInclude(d => d.Producto)
		.Select(c => new CompraResponseDto
		{
			CompraId = c.CompraId,
			Fecha = c.Fecha,
			Proveedor = c.Proveedor.Nombre,
			Total = c.Total,
			Detalles = c.Detalles.Select(d => new CompraDetalleResponseDto
			{
				ProductoId = d.ProductoId,
				Producto = d.Producto.Nombre,
				Cantidad = d.Cantidad,
				Precio = d.Precio,
				Subtotal = d.Cantidad * d.Precio
				}).ToList()
			})
			.ToListAsync();
	}
	
	//Obtener 1 Compra por medio de ID
	public async Task<CompraResponseDto?> ObtenerCompra(int id)
	{
		return await _context.Compras
			.Include(c => c.Proveedor)
			.Include(c => c.Detalles)
				.ThenInclude(d => d.Producto)
			.Where(c => c.CompraId == id)
			.Select(c => new CompraResponseDto
			{
				CompraId = c.CompraId,
				Fecha = c.Fecha,
				Proveedor = c.Proveedor.Nombre,
				Total = c.Total,

				Detalles = c.Detalles.Select(d => new CompraDetalleResponseDto
				{
					ProductoId = d.ProductoId,
					Producto = d.Producto.Nombre,
					Cantidad = d.Cantidad,
					Precio = d.Precio,
					Subtotal = d.Cantidad * d.Precio
				}).ToList()
			})
			.FirstOrDefaultAsync();
	}

}

/*
JSON
 |
 v
CompraService
 |
 +-- Guarda Compra
 |       |
 |       +-- genera CompraId = 15
 |
 +-- Guarda Detalles con CompraId = 15
 |
 +-- Actualiza Stock
 |
 v
Commit final
*/