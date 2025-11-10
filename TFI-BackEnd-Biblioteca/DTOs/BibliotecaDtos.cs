namespace TFI_BackEnd_Biblioteca.DTOs
{
    // DTOs para Libro
    public class LibroDto
    {
        public int Id { get; set; }
     public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public string? Editorial { get; set; }
        public int? AñoPublicacion { get; set; }
    public string? Genero { get; set; }
        public int CantidadDisponible { get; set; }
      public int CantidadTotal { get; set; }
        public DateTime FechaRegistro { get; set; }
 }

    public class CreateLibroDto
  {
   public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
    public string? ISBN { get; set; }
        public string? Editorial { get; set; }
    public int? AñoPublicacion { get; set; }
        public string? Genero { get; set; }
        public int CantidadDisponible { get; set; }
   public int CantidadTotal { get; set; }
    }

    public class UpdateLibroDto
    {
     public string Titulo { get; set; } = string.Empty;
public string Autor { get; set; } = string.Empty;
     public string? ISBN { get; set; }
    public string? Editorial { get; set; }
        public int? AñoPublicacion { get; set; }
  public string? Genero { get; set; }
    public int CantidadDisponible { get; set; }
public int CantidadTotal { get; set; }
    }

    // DTOs para Socio
    public class SocioDto
    {
     public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
 public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
     public string? Direccion { get; set; }
        public DateTime FechaRegistro { get; set; }
      public bool Activo { get; set; }
    }

    public class CreateSocioDto
    {
        public string Nombre { get; set; } = string.Empty;
  public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    }

    public class UpdateSocioDto
    {
  public string Nombre { get; set; } = string.Empty;
 public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
 public string? Telefono { get; set; }
      public string? Direccion { get; set; }
      public bool Activo { get; set; }
    }

    // DTOs para Préstamo
    public class PrestamoDto
    {
   public int Id { get; set; }
        public int LibroId { get; set; }
        public string LibroTitulo { get; set; } = string.Empty;
   public int SocioId { get; set; }
        public string SocioNombreCompleto { get; set; } = string.Empty;
   public DateTime FechaPrestamo { get; set; }
public DateTime FechaDevolucionEstimada { get; set; }
   public DateTime? FechaDevolucionReal { get; set; }
        public string Estado { get; set; } = string.Empty;
      public string? Observaciones { get; set; }
    public bool EstaAtrasado { get; set; }
    }

    public class CreatePrestamoDto
    {
        public int LibroId { get; set; }
    public int SocioId { get; set; }
   public DateTime FechaDevolucionEstimada { get; set; }
    public string? Observaciones { get; set; }
    }

    public class DevolverPrestamoDto
    {
 public string? Observaciones { get; set; }
    }
}
