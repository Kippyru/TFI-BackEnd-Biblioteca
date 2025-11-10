using System;
using System.Collections.Generic;

namespace TFI_BackEnd_Biblioteca.Models;

public partial class Prestamo
{
    public int IdPrestamo { get; set; }

    public int? IdUser { get; set; }

    public string? CodigoInventario { get; set; }

    public DateOnly? FechaDeInicio { get; set; }

    public DateOnly? FechaDeFin { get; set; }

    public int? IdUsuario { get; set; }

    public virtual ICollection<EjemplarLibro> EjemplarLibros { get; set; } = new List<EjemplarLibro>();

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
