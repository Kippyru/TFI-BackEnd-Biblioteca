using System;
using System.Collections.Generic;

namespace TFI_BackEnd_Biblioteca.Models;

public partial class Editorial
{
    public int IdEditorial { get; set; }

    public string? Nombre { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public virtual ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
