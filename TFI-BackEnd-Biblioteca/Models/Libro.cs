using System;
using System.Collections.Generic;

namespace TFI_BackEnd_Biblioteca.Models;

public partial class Libro
{
    public string Isbn { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public string? Categoria { get; set; }

    public string? Autores { get; set; }

    public int? Paginas { get; set; }

    public DateOnly? FechaPublicacion { get; set; }

    public int? IdEditorial { get; set; }

    public virtual ICollection<EjemplarLibro> EjemplarLibros { get; set; } = new List<EjemplarLibro>();

    public virtual Editorial? IdEditorialNavigation { get; set; }
}
