using System.ComponentModel.DataAnnotations;

namespace ApiInventario.DTOs;

public class VentaReporteDto
{
    public int VentaId { get; set; }

    public DateTime Fecha { get; set; }

    public string Cliente { get; set; }

    public decimal Total { get; set; }
}