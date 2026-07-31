using System.ComponentModel.DataAnnotations;

namespace ApiInventario.DTOs;

public class CompraReporteDto
{
    public int CompraId { get; set; }

    public DateTime Fecha { get; set; }

    public string Proveedor { get; set; }

    public decimal Total { get; set; }
}