using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiInventario.Models;

[Table("Compras")]
public class Compra
{
    [Key]
    public int CompraId { get; set; }

    [Required]
    public int ProveedorId { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }
	
	public string Observacion { get; set; } = string.Empty;

    // Relaciones
    public Proveedor? Proveedor { get; set; }

    public Usuario? Usuario { get; set; }

    public ICollection<CompraDetalle> Detalles { get; set; }
        = new List<CompraDetalle>();
}