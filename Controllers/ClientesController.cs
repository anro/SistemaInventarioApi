using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiInventario.Data;
using ApiInventario.Models;
using ApiInventario.DTOs;
using ApiInventario.Security;

namespace ApiInventario.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientesController(AppDbContext context)
    {
        _context = context;
    }

	// GET: api/Clientes
	[Permiso("CLIENTES_VER")]  //LISTAR TODOS LOS CLIENTES
	[HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> Get()
    {
        var clientes = await _context.Clientes
            .OrderBy(c => c.Nombre)
            .Select(c => new ClienteDto
            {
                ClienteId = c.ClienteId,
                Nombre = c.Nombre,
                Documento = c.Documento,
                Telefono = c.Telefono,
                Email = c.Email,
                Direccion = c.Direccion,
                Activo = c.Activo
            })
            .ToListAsync();

        return Ok(clientes);
    }

    // GET: api/Clientes/5
	[Permiso("CLIENTES_VER")] //BUSCAR CLIENTE POR ID
    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDto>> Get(int id)
    {
        var cliente = await _context.Clientes
            .Where(c => c.ClienteId == id)
            .Select(c => new ClienteDto
            {
                ClienteId = c.ClienteId,
                Nombre = c.Nombre,
                Documento = c.Documento,
                Telefono = c.Telefono,
                Email = c.Email,
                Direccion = c.Direccion,
                Activo = c.Activo
            })
            .FirstOrDefaultAsync();

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    // POST: api/Clientes
	[Permiso("CLIENTES_CREAR")] //INSERTAR NUEVO CLIENTE
    [HttpPost]
        public async Task<ActionResult<ClienteDto>> Post(ClienteCreateDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Documento))
        {
            bool existe = await _context.Clientes
                .AnyAsync(c => c.Documento == dto.Documento);

            if (existe)
                return BadRequest("Ya existe un cliente con ese Documento.");
        }

        var cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Documento = dto.Documento,
            Telefono = dto.Telefono,
            Email = dto.Email,
            Direccion = dto.Direccion,
            Activo = dto.Activo
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        //dto.ClienteId = cliente.ClienteId;

        return CreatedAtAction(nameof(Get), new { id = cliente.ClienteId }, dto);
    }

    // PUT: api/Clientes/5
	[Permiso("CLIENTES_EDITAR")] //MODIFICAR CLIENTE
    [HttpPut("{id}")]
	public async Task<IActionResult> Put(int id,[FromBody] ClienteUpdateDto dto)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Documento))
        {
            bool existe = await _context.Clientes
                .AnyAsync(c => c.Documento == dto.Documento &&
                               c.ClienteId != id);

            if (existe)
                return BadRequest("Ya existe un cliente con ese Documento.");
        }

        cliente.Nombre = dto.Nombre;
        cliente.Documento = dto.Documento;
        cliente.Telefono = dto.Telefono;
        cliente.Email = dto.Email;
        cliente.Direccion = dto.Direccion;
        cliente.Activo = dto.Activo;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Clientes/5
	[Permiso("CLIENTES_ELIMINAR")] //ELIMINAR CLIENTE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound();

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}