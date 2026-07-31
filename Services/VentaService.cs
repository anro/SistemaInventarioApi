using ApiInventario.Data;
using ApiInventario.DTOs;
using ApiInventario.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiInventario.Services;

public class VentaService : IVentaService
{
    private readonly AppDbContext _context;
	private readonly ILogger<VentaService> _logger;

    public VentaService( AppDbContext context, ILogger<VentaService> logger)
	{
		_context = context;
		_logger = logger;
	}
	
	//CREAR Venta
    public async Task<Venta> CrearVenta(VentaDto dto)
    {
        var cliente = await _context.Clientes
            .FindAsync(dto.ClienteId);

        if(cliente == null)
			throw new InvalidOperationException("Cliente no existe");
           // throw new Exception("cliente no existe");

        var venta = new Venta
        {
            ClienteId = dto.ClienteId,
            Fecha = dto.Fecha,
			UsuarioId = dto.UsuarioId,
            MetodoPago = dto.MetodoPago
        };

		if (dto.Detalles == null || !dto.Detalles.Any())
		{
			//throw new Exception("La Venta debe tener al menos un detalle.");
			throw new InvalidOperationException("La Venta debe tener al menos un detalle.");
		}
		
		// Validar stock antes de guardar
		foreach (var detalle in dto.Detalles)
		{
			var producto = await _context.Productos
				.FirstOrDefaultAsync(p => p.ProductoId == detalle.ProductoId);

			if (producto == null)
				throw new InvalidOperationException("Producto no existe");

			if (producto.Stock < detalle.Cantidad)
				throw new InvalidOperationException(
					$"Stock insuficiente para {producto.Nombre}");
		}
		// Guardar cabecera + detalle
		venta.Total = dto.Detalles.Sum(x => x.Cantidad * x.Precio);
		_context.Ventas.Add(venta);

		await _context.SaveChangesAsync();
		
		//Ventas detalle
        foreach (var item in dto.Detalles)
		{
			var detalle = new VentaDetalle
            {
                VentaId = venta.VentaId,
                ProductoId = item.ProductoId,
                Cantidad = item.Cantidad,
                Precio = item.Precio
            };
			
			if(detalle.Cantidad <= 0)
			{
				
				throw new InvalidOperationException("La cantidad del producto {detalle.ProductoId} debe ser mayor a cero");
				/*throw new Exception(
					$"La cantidad del producto {detalle.ProductoId} debe ser mayor a cero"
				);*/
			}
			

            _context.VentaDetalle.Add(detalle);

		    // Buscar producto
			var producto = await _context.Productos
             .FindAsync(item.ProductoId);

			if(producto != null)
			{
				// Actualizar stock
				// AQUÍ está la lógica de inventario en prodcutos
				producto.Stock -= item.Cantidad;
				//producto.PrecioVenta = item.Precio;
				// Crear movimiento
				var movimiento = new MovimientoStock
				{
					ProductoId = item.ProductoId ,
					TipoMovimiento = "SALIDA",
					Cantidad = item.Cantidad,
					Fecha = DateTime.Now,
					Referencia = 
						"Venta Nro " + venta.VentaId
				};
				_context.MovimientosStock.Add(movimiento);
			}
        }
        await _context.SaveChangesAsync();
        //return "Venta registrada correctamente";
		return venta;
    }
	/*
	Esto trae:
	- La venta
	- Sus detalles
	- El producto vendido
	PARA OBTENER UNA SOLA VENTA*/
	public async Task<IEnumerable<Venta>> ObtenerVentas()
	{
		return await _context.Ventas
			.Include(v => v.Detalles)
			.ThenInclude(d => d.Producto)
			.ToListAsync();
	}
	
	/*
	BUSQUEDA DE UNA VENTA POS ID
	*/
	public async Task<Venta?> ObtenerVentaPorId(int id)
	{
		return await _context.Ventas
			.Include(v => v.Detalles)
			.ThenInclude(d => d.Producto)
			.FirstOrDefaultAsync(v => v.VentaId == id);
	}


}


/*

VENTA
   |
   ↓
VentaService
   |
   ↓
Validar Stock >= cantidad
   |
   ↓
Producto.Stock -= cantidad
   |
   ↓
MovimientoStock SALIDA
*/