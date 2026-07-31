using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiInventario.Models
{
    [Table("Proveedores")]
    public class Proveedor
    {
        [Key]
        public int ProveedorId { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        [StringLength(50)]
        public string? RUC { get; set; }

        [StringLength(50)]
        public string? Telefono { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }

        public bool Activo { get; set; } = true;
		
		public ICollection<Producto> Productos { get; set; }
			= new List<Producto>();

		public ICollection<Compra> Compras { get; set; }
			= new List<Compra>();
    }
}