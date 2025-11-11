using System;
using System.Collections.Generic;

namespace TFI_BackEnd_Biblioteca.Models;

public partial class Estudiante
{
    public string NLegajo { get; set; } = null!;

    public DateOnly? FechaIngresoFacultad { get; set; }

    public string? MateriasCursando { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
