using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiInventario.Models;

[Table("CompraDetalle")]
public class CompraDetalle
{
    [Key]
    public int CompraDetalleId { get; set; }

    [Required]
    public int CompraId { get; set; }

    [Required]
    public int ProductoId { get; set; }

    [Required]
    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; private set; }

    // Relaciones
    public Compra? Compra { get; set; }

    public Producto? Producto { get; set; }
}