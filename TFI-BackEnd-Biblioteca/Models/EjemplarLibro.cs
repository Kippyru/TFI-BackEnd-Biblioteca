using System;
using System.Collections.Generic;

namespace TFI_BackEnd_Biblioteca.Models;

public partial class EjemplarLibro
{
    public string IdCodigoInventario { get; set; } = null!;

    public string? CodigoUbicacion { get; set; }

    public DateOnly? FechaAlta { get; set; }

    public DateOnly? FechaBaja { get; set; }

    public string? Isbn { get; set; }

    public int? IdPrestamo { get; set; }

    public virtual Prestamo? IdPrestamoNavigation { get; set; }

    public virtual Libro? IsbnNavigation { get; set; }
}
