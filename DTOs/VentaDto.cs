using System.ComponentModel.DataAnnotations;

namespace ApiInventario.DTOs;

public class VentaDto
{
    [Required]
    public int ClienteId { get; set; }

    public DateTime Fecha{ get; set; }

    public string? MetodoPago { get; set; }
	
	public int UsuarioId { get; set; }

    [Required]
    public List<VentaDetalleDto> Detalles { get; set; } = new();
}