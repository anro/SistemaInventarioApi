namespace ApiInventario.DTOs;

using System.ComponentModel.DataAnnotations;

public class ProductoCreateDto
{
    [Required]
    [MaxLength(30)]
    public string Codigo { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = "";

    [MaxLength(200)]
    public string? Descripcion { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PrecioCompra { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PrecioVenta { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Range(0, int.MaxValue)]
    public int StockMinimo { get; set; }

    [Required]
    public int ProveedorId { get; set; }

    public bool Activo { get; set; } = true;
}