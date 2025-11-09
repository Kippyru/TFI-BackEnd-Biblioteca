using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFI_BackEnd_Biblioteca.Data;
using TFI_BackEnd_Biblioteca.Models;

namespace TFI_BackEnd_Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SociosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

     public SociosController(BibliotecaContext context)
        {
_context = context;
        }

        // GET: api/Socios
    [HttpGet]
        public async Task<ActionResult<IEnumerable<Socio>>> GetSocios()
     {
     return await _context.Socios.ToListAsync();
        }

        // GET: api/Socios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Socio>> GetSocio(int id)
        {
      var socio = await _context.Socios
                .Include(s => s.Prestamos)
      .ThenInclude(p => p.Libro)
              .FirstOrDefaultAsync(s => s.Id == id);

    if (socio == null)
       {
      return NotFound(new { message = "Socio no encontrado" });
    }

         return socio;
        }

        // GET: api/Socios/activos
        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<Socio>>> GetSociosActivos()
        {
     return await _context.Socios
     .Where(s => s.Activo)
    .ToListAsync();
        }

    // GET: api/Socios/buscar?termino=perez
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Socio>>> BuscarSocios([FromQuery] string termino)
        {
        if (string.IsNullOrWhiteSpace(termino))
        {
       return BadRequest(new { message = "Debe proporcionar un término de búsqueda" });
            }

            var socios = await _context.Socios
      .Where(s => s.Nombre.Contains(termino) || 
         s.Apellido.Contains(termino) ||
   s.Email.Contains(termino))
        .ToListAsync();

            return socios;
     }

   // GET: api/Socios/5/prestamos
        [HttpGet("{id}/prestamos")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamosDeSocio(int id)
     {
  var socioExists = await _context.Socios.AnyAsync(s => s.Id == id);
            if (!socioExists)
            {
  return NotFound(new { message = "Socio no encontrado" });
    }

            var prestamos = await _context.Prestamos
         .Include(p => p.Libro)
           .Where(p => p.SocioId == id)
   .OrderByDescending(p => p.FechaPrestamo)
         .ToListAsync();

            return prestamos;
    }

        // GET: api/Socios/5/prestamos/activos
        [HttpGet("{id}/prestamos/activos")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamosActivosDeSocio(int id)
        {
      var socioExists = await _context.Socios.AnyAsync(s => s.Id == id);
if (!socioExists)
      {
           return NotFound(new { message = "Socio no encontrado" });
      }

            var prestamos = await _context.Prestamos
      .Include(p => p.Libro)
        .Where(p => p.SocioId == id && p.Estado == "Activo")
        .OrderByDescending(p => p.FechaPrestamo)
     .ToListAsync();

       return prestamos;
        }

        // POST: api/Socios
    [HttpPost]
  public async Task<ActionResult<Socio>> PostSocio(Socio socio)
        {
      if (!ModelState.IsValid)
   {
        return BadRequest(ModelState);
            }

    // Verificar si el email ya existe
            var emailExists = await _context.Socios.AnyAsync(s => s.Email == socio.Email);
if (emailExists)
{
       return BadRequest(new { message = "El email ya está registrado" });
    }

  socio.FechaRegistro = DateTime.Now;
    socio.Activo = true;

    _context.Socios.Add(socio);
    await _context.SaveChangesAsync();

       return CreatedAtAction(nameof(GetSocio), new { id = socio.Id }, socio);
        }

     // PUT: api/Socios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSocio(int id, Socio socio)
        {
   if (id != socio.Id)
       {
       return BadRequest(new { message = "El ID no coincide" });
            }

 // Verificar si el email ya existe en otro socio
            var emailExists = await _context.Socios
     .AnyAsync(s => s.Email == socio.Email && s.Id != id);
       if (emailExists)
            {
    return BadRequest(new { message = "El email ya está registrado por otro socio" });
            }

  _context.Entry(socio).State = EntityState.Modified;

            try
      {
          await _context.SaveChangesAsync();
            }
     catch (DbUpdateConcurrencyException)
        {
         if (!SocioExists(id))
           {
     return NotFound(new { message = "Socio no encontrado" });
       }
        else
          {
      throw;
                }
    }

   return NoContent();
        }

// PUT: api/Socios/5/desactivar
    [HttpPut("{id}/desactivar")]
        public async Task<IActionResult> DesactivarSocio(int id)
        {
            var socio = await _context.Socios.FindAsync(id);
         if (socio == null)
      {
     return NotFound(new { message = "Socio no encontrado" });
            }

            // Verificar si tiene préstamos activos
      var tienePrestamosActivos = await _context.Prestamos
                .AnyAsync(p => p.SocioId == id && p.Estado == "Activo");

            if (tienePrestamosActivos)
            {
                return BadRequest(new { message = "No se puede desactivar el socio porque tiene préstamos activos" });
            }

      socio.Activo = false;
            await _context.SaveChangesAsync();

      return NoContent();
     }

  // PUT: api/Socios/5/activar
   [HttpPut("{id}/activar")]
        public async Task<IActionResult> ActivarSocio(int id)
        {
            var socio = await _context.Socios.FindAsync(id);
if (socio == null)
 {
      return NotFound(new { message = "Socio no encontrado" });
 }

       socio.Activo = true;
        await _context.SaveChangesAsync();

     return NoContent();
        }

        // DELETE: api/Socios/5
        [HttpDelete("{id}")]
 public async Task<IActionResult> DeleteSocio(int id)
        {
        var socio = await _context.Socios.FindAsync(id);
    if (socio == null)
        {
      return NotFound(new { message = "Socio no encontrado" });
 }

            // Verificar si tiene préstamos
            var tienePrestamos = await _context.Prestamos.AnyAsync(p => p.SocioId == id);
       if (tienePrestamos)
     {
       return BadRequest(new { message = "No se puede eliminar el socio porque tiene préstamos registrados. Considere desactivarlo en su lugar." });
  }

   _context.Socios.Remove(socio);
            await _context.SaveChangesAsync();

          return NoContent();
        }

   private bool SocioExists(int id)
        {
       return _context.Socios.Any(e => e.Id == id);
        }
    }
}
