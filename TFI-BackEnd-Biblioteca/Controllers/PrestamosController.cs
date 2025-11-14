using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFI_BackEnd_Biblioteca.Data;
using TFI_BackEnd_Biblioteca.Models;

namespace TFI_BackEnd_Biblioteca.Controllers
{
  [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public PrestamosController(BibliotecaContext context)
      {
            _context = context;
  }

        // GET: api/Prestamos
      [HttpGet]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamos()
    {
       return await _context.Prestamos
   .Include(p => p.Libro)
     .Include(p => p.Socio)
  .OrderByDescending(p => p.FechaPrestamo)
        .ToListAsync();
      }

        // GET: api/Prestamos/5
        [HttpGet("{id}")]
     public async Task<ActionResult<Prestamo>> GetPrestamo(int id)
   {
            var prestamo = await _context.Prestamos
  .Include(p => p.Libro)
      .Include(p => p.Socio)
                .FirstOrDefaultAsync(p => p.Id == id);

 if (prestamo == null)
            {
      return NotFound(new { message = "Préstamo no encontrado" });
            }

return prestamo;
        }

        // GET: api/Prestamos/activos
        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamosActivos()
     {
            return await _context.Prestamos
      .Include(p => p.Libro)
    .Include(p => p.Socio)
      .Where(p => p.Estado == "Activo")
     .OrderByDescending(p => p.FechaPrestamo)
                .ToListAsync();
        }

 // GET: api/Prestamos/atrasados
[HttpGet("atrasados")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamosAtrasados()
        {
            var hoy = DateTime.Now;
return await _context.Prestamos
             .Include(p => p.Libro)
     .Include(p => p.Socio)
   .Where(p => p.Estado == "Activo" && p.FechaDevolucionEstimada < hoy)
        .OrderBy(p => p.FechaDevolucionEstimada)
        .ToListAsync();
    }

   // POST: api/Prestamos
   [HttpPost]
        public async Task<ActionResult<Prestamo>> PostPrestamo(Prestamo prestamo)
{
            if (!ModelState.IsValid)
    {
    return BadRequest(ModelState);
      }

      // Verificar que el libro existe
     var libro = await _context.Libros.FindAsync(prestamo.LibroId);
       if (libro == null)
            {
           return NotFound(new { message = "El libro no existe" });
      }

 // Verificar que el socio existe y está activo
     var socio = await _context.Socios.FindAsync(prestamo.SocioId);
            if (socio == null)
       {
          return NotFound(new { message = "El socio no existe" });
            }

            if (!socio.Activo)
            {
      return BadRequest(new { message = "El socio no está activo" });
            }

            // Verificar disponibilidad del libro
        if (libro.CantidadDisponible <= 0)
   {
         return BadRequest(new { message = "No hay ejemplares disponibles de este libro" });
            }

            // Verificar que el socio no tenga préstamos atrasados
            var tienePrestamosAtrasados = await _context.Prestamos
          .AnyAsync(p => p.SocioId == prestamo.SocioId && 
   p.Estado == "Activo" && 
  p.FechaDevolucionEstimada < DateTime.Now);

   if (tienePrestamosAtrasados)
        {
        return BadRequest(new { message = "El socio tiene préstamos atrasados. Debe devolverlos antes de realizar un nuevo préstamo." });
            }

            // Crear el préstamo
        prestamo.FechaPrestamo = DateTime.Now;
     prestamo.Estado = "Activo";
            
            // Si no se especifica fecha de devolución, se establece en 15 días
  if (prestamo.FechaDevolucionEstimada == default)
            {
         prestamo.FechaDevolucionEstimada = DateTime.Now.AddDays(15);
     }

   // Reducir cantidad disponible
    libro.CantidadDisponible--;

    _context.Prestamos.Add(prestamo);
   await _context.SaveChangesAsync();

   // Recargar el préstamo con las relaciones
      var prestamoCreado = await _context.Prestamos
.Include(p => p.Libro)
              .Include(p => p.Socio)
    .FirstOrDefaultAsync(p => p.Id == prestamo.Id);

            return CreatedAtAction(nameof(GetPrestamo), new { id = prestamo.Id }, prestamoCreado);
        }

   // PUT: api/Prestamos/5/devolver
        [HttpPut("{id}/devolver")]
        public async Task<ActionResult<Prestamo>> DevolverPrestamo(int id, [FromBody] string? observaciones = null)
      {
            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
            .FirstOrDefaultAsync(p => p.Id == id);

    if (prestamo == null)
            {
           return NotFound(new { message = "Préstamo no encontrado" });
            }

      if (prestamo.Estado != "Activo")
   {
       return BadRequest(new { message = "Este préstamo ya ha sido devuelto" });
            }

            // Registrar devolución
       prestamo.FechaDevolucionReal = DateTime.Now;
       prestamo.Estado = "Devuelto";
            
            if (!string.IsNullOrWhiteSpace(observaciones))
   {
              prestamo.Observaciones = observaciones;
            }

        // Incrementar cantidad disponible
        prestamo.Libro.CantidadDisponible++;

    await _context.SaveChangesAsync();

            return Ok(prestamo);
      }

        // PUT: api/Prestamos/5/renovar
        [HttpPut("{id}/renovar")]
        public async Task<ActionResult<Prestamo>> RenovarPrestamo(int id, [FromQuery] int dias = 15)
        {
     var prestamo = await _context.Prestamos.FindAsync(id);

      if (prestamo == null)
            {
                return NotFound(new { message = "Préstamo no encontrado" });
        }

            if (prestamo.Estado != "Activo")
     {
     return BadRequest(new { message = "Solo se pueden renovar préstamos activos" });
  }

         // Extender la fecha de devolución
      prestamo.FechaDevolucionEstimada = prestamo.FechaDevolucionEstimada.AddDays(dias);

            await _context.SaveChangesAsync();

        return Ok(prestamo);
     }

// PUT: api/Prestamos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestamo(int id, Prestamo prestamo)
    {
         if (id != prestamo.Id)
            {
  return BadRequest(new { message = "El ID no coincide" });
            }

            _context.Entry(prestamo).State = EntityState.Modified;

    try
            {
      await _context.SaveChangesAsync();
        }
            catch (DbUpdateConcurrencyException)
            {
          if (!PrestamoExists(id))
         {
        return NotFound(new { message = "Préstamo no encontrado" });
                }
  else
       {
          throw;
                }
   }

 return NoContent();
        }

        // DELETE: api/Prestamos/5
      [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
  var prestamo = await _context.Prestamos
      .Include(p => p.Libro)
      .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null)
 {
       return NotFound(new { message = "Préstamo no encontrado" });
            }

  // Si el préstamo está activo, restaurar la cantidad disponible
            if (prestamo.Estado == "Activo")
  {
           prestamo.Libro.CantidadDisponible++;
         }

         _context.Prestamos.Remove(prestamo);
     await _context.SaveChangesAsync();

    return NoContent();
   }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.Id == id);
        }
    }
}
