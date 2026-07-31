using System.ComponentModel.DataAnnotations;

namespace ApiInventario.DTOs;

public class CrearUsuarioDto
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public int RolId { get; set; }
}