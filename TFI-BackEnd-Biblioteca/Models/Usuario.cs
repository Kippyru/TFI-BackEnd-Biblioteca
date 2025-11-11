using System;
using System.Collections.Generic;

namespace TFI_BackEnd_Biblioteca.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string? NLegajo { get; set; }

    public virtual Estudiante? NLegajoNavigation { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
