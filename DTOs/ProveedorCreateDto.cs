namespace ApiInventario.DTOs
{
    public class ProveedorCreateDto
    {
        //public int IdProveedor { get; set; }

        public string RazonSocial { get; set; }

        public string Ruc { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public string Direccion { get; set; }

        public bool Activo { get; set; }
    }
}