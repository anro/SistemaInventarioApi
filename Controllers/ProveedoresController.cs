using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiInventario.Data;
using ApiInventario.Models;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> Get()
        {
            return await _context.Proveedores
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        // GET: api/proveedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> Get(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
                return NotFound();

            return proveedor;
        }

        // POST: api/proveedores
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Proveedor>> Post(Proveedor proveedor)
        {
            // Validar RUC duplicado
            if (!string.IsNullOrWhiteSpace(proveedor.RUC))
            {
                bool existe = await _context.Proveedores
                    .AnyAsync(p => p.RUC == proveedor.RUC);

                if (existe)
                    return BadRequest("Ya existe un proveedor con ese RUC.");
            }

            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = proveedor.ProveedorId }, proveedor);
        }

        // PUT: api/proveedores/5
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Put(int id, Proveedor proveedor)
        {
            var p = await _context.Proveedores.FindAsync(id);

            if (p == null)
                return NotFound();

            // Validar RUC duplicado
            if (!string.IsNullOrWhiteSpace(proveedor.RUC))
            {
                bool existe = await _context.Proveedores
                    .AnyAsync(x => x.RUC == proveedor.RUC &&
                                   x.ProveedorId != id);

                if (existe)
                    return BadRequest("Ya existe un proveedor con ese RUC.");
            }

            p.Nombre = proveedor.Nombre;
            p.RUC = proveedor.RUC;
            p.Telefono = proveedor.Telefono;
            p.Email = proveedor.Email;
            p.Direccion = proveedor.Direccion;
            p.Activo = proveedor.Activo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/proveedores/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
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