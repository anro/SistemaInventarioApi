using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiInventario.Models;

[Table("Usuarios")]
public class Usuario
{

    [Key]
    public int UsuarioId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = "";

    [MaxLength(150)]
    public string? Email { get; set; }

    [Column("PasswordHash")]
	public string PasswordHash { get; set; } = "";

    public int RolId { get; set; }

    public bool Activo { get; set; } = true;

    public DateTime? FechaCreacion { get; set; }

    // Navegación
    public Rol? Rol { get; set; } = null!;

}

