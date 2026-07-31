using ApiInventario.DTOs;

namespace ApiInventario.Services;

public interface IProductoService
{
    Task<IEnumerable<ProductoDto>> ObtenerTodosAsync(); // Listar todos los productos

    Task<ProductoDto?> ObtenerPorIdAsync(int id); // Obtener 1 producto por Id

    Task<ProductoDto> CrearAsync(ProductoCreateDto dto); //Insertar nuevo producto

    Task<ProductoDto?> ActualizarAsync( int id, ProductoUpdateDto dto); //Modicar un Producto

    Task<bool> EliminarAsync(int id); // Eliminar un producto
}