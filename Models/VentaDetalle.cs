using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiInventario.Models;

[Table("ventaDetalle")]
public class VentaDetalle
{
	[Key]
	public int VentaDetalleId { get; set; }
	
	[Required]
	public int VentaId { get; set; }
	
	[Required]
	public int ProductoId { get; set; }
	
	public int Cantidad { get; set; }
	
	[Column(TypeName = "decimal(18,2)")]
	public decimal Precio { get; set; }
	
	[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; private set; }
	
	// Relaciones
    public Venta? Venta { get; set; }
    public Producto? Producto { get; set; }
}



