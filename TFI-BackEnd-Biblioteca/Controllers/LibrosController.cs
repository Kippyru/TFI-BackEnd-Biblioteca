using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TFI_BackEnd_Biblioteca.Data;
using TFI_BackEnd_Biblioteca.Models;

namespace TFI_BackEnd_Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

  public LibrosController(BibliotecaContext context)
        {
       _context = context;
        }

      // GET: api/Libros
 [HttpGet]
  public async Task<ActionResult<IEnumerable<Libro>>> GetLibros()
 {
return await _context.Libros.ToListAsync();
        }

        // GET: api/Libros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
      var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
     {
     return NotFound(new { message = "Libro no encontrado" });
   }

  return libro;
        }

        // GET: api/Libros/disponibles
        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosDisponibles()
        {
 return await _context.Libros
     .Where(l => l.CantidadDisponible > 0)
    .ToListAsync();
        }

        // GET: api/Libros/buscar?termino=garcia
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Libro>>> BuscarLibros([FromQuery] string termino)
        {
  if (string.IsNullOrWhiteSpace(termino))
         {
     return BadRequest(new { message = "Debe proporcionar un término de búsqueda" });
            }

      var libros = await _context.Libros
      .Where(l => l.Titulo.Contains(termino) || 
      l.Autor.Contains(termino) ||
               (l.ISBN != null && l.ISBN.Contains(termino)))
      .ToListAsync();

      return libros;
}

        // POST: api/Libros
    [HttpPost]
  public async Task<ActionResult<Libro>> PostLibro(Libro libro)
{
       if (!ModelState.IsValid)
            {
 return BadRequest(ModelState);
            }

  libro.FechaRegistro = DateTime.Now;
       _context.Libros.Add(libro);
     await _context.SaveChangesAsync();

  return CreatedAtAction(nameof(GetLibro), new { id = libro.Id }, libro);
        }

        // PUT: api/Libros/5
        [HttpPut("{id}")]
     public async Task<IActionResult> PutLibro(int id, Libro libro)
        {
            if (id != libro.Id)
            {
           return BadRequest(new { message = "El ID no coincide" });
       }

            _context.Entry(libro).State = EntityState.Modified;

            try
          {
    await _context.SaveChangesAsync();
          }
 catch (DbUpdateConcurrencyException)
            {
   if (!LibroExists(id))
          {
     return NotFound(new { message = "Libro no encontrado" });
          }
       else
   {
            throw;
     }
 }

return NoContent();
      }

  // DELETE: api/Libros/5
    [HttpDelete("{id}")]
 public async Task<IActionResult> DeleteLibro(int id)
        {
      var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
   {
 return NotFound(new { message = "Libro no encontrado" });
            }

 // Verificar si tiene préstamos activos
      var tienePrestamosActivos = await _context.Prestamos
       .AnyAsync(p => p.LibroId == id && p.Estado == "Activo");

      if (tienePrestamosActivos)
 {
     return BadRequest(new { message = "No se puede eliminar el libro porque tiene préstamos activos" });
       }

 _context.Libros.Remove(libro);
   await _context.SaveChangesAsync();

      return NoContent();
        }

        private bool LibroExists(int id)
        {
  return _context.Libros.Any(e => e.Id == id);
        }
    }
}
