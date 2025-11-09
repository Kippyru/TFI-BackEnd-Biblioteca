namespace TFI_BackEnd_Biblioteca.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
   public int SocioId { get; set; }
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;
        public DateTime FechaDevolucionEstimada { get; set; }
        public DateTime? FechaDevolucionReal { get; set; }
   public string Estado { get; set; } = "Activo"; // Activo, Devuelto, Atrasado
   public string? Observaciones { get; set; }

// Navegación
        public Libro Libro { get; set; } = null!;
  public Socio Socio { get; set; } = null!;
  }
}
