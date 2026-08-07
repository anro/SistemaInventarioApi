using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiInventario.Data;
using ApiInventario.Models;
using ApiInventario.DTOs;
using ApiInventario.Security;

namespace ApiInventario.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProveedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProveedoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/proveedores
		[Permiso("PROVEEDORES_VER")]  //LISTAR TODOS LOS PROVEEDORES
        [HttpGet]
		public async Task<ActionResult<IEnumerable<ProveedorDto>>> Get()
		{
			var proveedores = await _context.Proveedores
				.OrderBy(p => p.Nombre)
				.Select(p => new ProveedorDto
				{
					ProveedorId = p.ProveedorId,
					RazonSocial = p.Nombre,
					Ruc = p.RUC,
					Telefono = p.Telefono,
					Email = p.Email,
					Direccion = p.Direccion,
					Activo = p.Activo
				})
				.ToListAsync();

			return Ok(proveedores);
		}

        // GET: api/proveedores/5
		[Permiso("PROVEEDORES_VER")] //BUSCAR PROVEEDOR
        [HttpGet("{id}")]
		public async Task<ActionResult<ProveedorDto>> Get(int id)
		{
			var proveedor = await _context.Proveedores
				.Where(p => p.ProveedorId == id)
				.Select(p => new ProveedorDto
				{
					ProveedorId = p.ProveedorId,
					RazonSocial = p.Nombre,
					Ruc = p.RUC,
					Telefono = p.Telefono,
					Email = p.Email,
					Direccion = p.Direccion,
					Activo = p.Activo
				})
				.FirstOrDefaultAsync();

			if (proveedor == null)
				return NotFound();

			return Ok(proveedor);
		}

        // POST: api/proveedores
		[Permiso("PROVEEDORES_CREAR")] //CREAR NUEVO PROVEEDOR
        [HttpPost]
        public async Task<ActionResult> Post(ProveedorCreateDto dto)
		{
			// Validar RUC duplicado
			if (!string.IsNullOrWhiteSpace(dto.Ruc))
			{
				bool existe = await _context.Proveedores
					.AnyAsync(p => p.RUC == dto.Ruc);

				if (existe)
					return BadRequest("Ya existe un proveedor con ese RUC.");
			}

			var proveedor = new Proveedor
			{
				Nombre = dto.RazonSocial,
				RUC = dto.Ruc,
				Telefono = dto.Telefono,
				Email = dto.Email,
				Direccion = dto.Direccion,
				Activo = dto.Activo
			};

			_context.Proveedores.Add(proveedor);
			await _context.SaveChangesAsync();

			return CreatedAtAction(
				nameof(Get),
				new { id = proveedor.ProveedorId },
				proveedor
			);
		}

        // PUT: api/proveedores/5
		[Permiso("PROVEEDORES_EDITAR")] // MODIFICAR PROVEEDOR
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] ProveedorUpdateDto dto)
		{
			var p = await _context.Proveedores.FindAsync(id);

			if (p == null)
				return NotFound();

			// Validar RUC duplicado
			if (!string.IsNullOrWhiteSpace(dto.Ruc))
			{
				bool existe = await _context.Proveedores
					.AnyAsync(x => x.RUC == dto.Ruc &&
								   x.ProveedorId != id);

				if (existe)
					return BadRequest("Ya existe un proveedor con ese RUC.");
			}

			p.Nombre = dto.RazonSocial;
			p.RUC = dto.Ruc;
			p.Telefono = dto.Telefono;
			p.Email = dto.Email;
			p.Direccion = dto.Direccion;
			p.Activo = dto.Activo;

			await _context.SaveChangesAsync();

			return NoContent();
		}

        // DELETE: api/proveedores/5
		[Permiso("PROVEEDORES_ELIMINAR")] //ELIMINAR PROVEEDOR
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return NotFound();

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}