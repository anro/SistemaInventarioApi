using System.ComponentModel.DataAnnotations;

namespace ApiInventario.DTOs;

public class CompraDetalleResponseDto
{
    public int ProductoId { get; set; }
    public string Producto { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
    public decimal Subtotal { get; set; }
}