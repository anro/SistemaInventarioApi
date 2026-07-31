using System.ComponentModel.DataAnnotations;

namespace ApiInventario.DTOs;

public class ProductoVentaDto
{
    public string Producto { get; set; }

    public int CantidadVendida { get; set; }
}