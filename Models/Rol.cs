using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiInventario.Models;


[Table("Roles")]
public class Rol
{
    [Key]
    public int RolId {get;set;}

    [Required]
    public string Nombre {get;set;}="";

    public bool Activo {get;set;}=true;

    public ICollection<Usuario> Usuarios {get;set;}
        = new List<Usuario>();

    public ICollection<RolPermiso> RolPermisos {get;set;}
        = new List<RolPermiso>();
}