using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiInventario.Models;

[Table("Permisos")]
public class Permiso
{
    [Key]
    public int PermisoId { get; set; }


    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = "";


    [MaxLength(200)]
    public string? Descripcion { get; set; }


    public bool Activo { get; set; } = true;


    public ICollection<RolPermiso> RolPermisos { get; set; }
        = new List<RolPermiso>();
}