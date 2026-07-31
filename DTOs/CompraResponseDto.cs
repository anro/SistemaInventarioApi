using System.ComponentModel.DataAnnotations;

namespace ApiInventario.DTOs;

public class CompraResponseDto
{
    public int CompraId { get; set; }
    public DateTime Fecha { get; set; }
    public string Proveedor { get; set; }
    public decimal Total { get; set; }

    public List<CompraDetalleResponseDto> Detalles { get; set; }
}