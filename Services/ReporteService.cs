using ApiInventario.Data;
using ApiInventario.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ApiInventario.Services;

public class ReporteService : IReporteService
{
    private readonly AppDbContext _context;

    public ReporteService(AppDbContext context)
    {
        _context = context;
    }

    //==============================================
    // Ventas por fecha
    //==============================================
    public async Task<List<VentaReporteDto>> ObtenerVentasPorFecha(DateTime desde, DateTime hasta)
    {
        return await _context.Ventas
            .Include(v => v.Clientes)
            .Where(v => v.Fecha >= desde && v.Fecha <= hasta)
            .Select(v => new VentaReporteDto
            {
                VentaId = v.VentaId,
                Fecha = v.Fecha,
                Cliente = v.Clientes!.Nombre,
                Total = v.Total
            })
            .OrderBy(v => v.Fecha)
            .ToListAsync();
    }

    //==============================================
    // Compras por fecha
    //==============================================
    public async Task<List<CompraReporteDto>> ObtenerComprasPorFecha(DateTime desde, DateTime hasta)
    {
        return await _context.Compras
            .Include(c => c.Proveedor)
            .Where(c => c.Fecha >= desde && c.Fecha <= hasta)
            .Select(c => new CompraReporteDto
            {
                CompraId = c.CompraId,
                Fecha = c.Fecha,
                Proveedor = c.Proveedor!.Nombre,
                Total = c.Total
            })
            .OrderBy(c => c.Fecha)
            .ToListAsync();
    }

    //==============================================
    // Productos más vendidos
    //==============================================
    public async Task<List<ProductoVentaDto>> ObtenerProductosMasVendidos()
    {
        return await _context.VentaDetalle
            .Include(v => v.Producto)
            .GroupBy(v => v.Producto!.Nombre)
            .Select(g => new ProductoVentaDto
            {
                Producto = g.Key,
                CantidadVendida = g.Sum(x => x.Cantidad)
            })
            .OrderByDescending(x => x.CantidadVendida)
            .ToListAsync();
    }

    //==============================================
    // Productos sin stock
    //==============================================
    public async Task<List<ProductoDto>> ObtenerProductosSinStock()
    {
        return await _context.Productos
            .Where(p => p.Stock == 0)
            .Select(p => new ProductoDto
            {
                //ProductoId = p.ProductoId,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                PrecioCompra = p.PrecioCompra,
                PrecioVenta = p.PrecioVenta,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo,
                //ProveedorId = p.ProveedorId,
                Activo = p.Activo
            })
            .ToListAsync();
    }

    //==============================================
    // Productos bajo stock mínimo
    //==============================================
    public async Task<List<ProductoDto>> ObtenerProductosBajoStock()
    {
        return await _context.Productos
            .Where(p => p.Stock <= p.StockMinimo)
            .Select(p => new ProductoDto
            {
               // ProductoId = p.ProductoId,
                Codigo = p.Codigo,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                PrecioCompra = p.PrecioCompra,
                PrecioVenta = p.PrecioVenta,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo,
                //ProveedorId = p.ProveedorId,
                Activo = p.Activo
            })
            .ToListAsync();
    }
}