namespace ApiInventario.DTOs;

public class CompraDetalleDto
{
    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal Precio { get; set; }
}