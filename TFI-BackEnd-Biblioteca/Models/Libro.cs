namespace TFI_BackEnd_Biblioteca.Models
{
    public class Libro
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
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

      // Navegación
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
