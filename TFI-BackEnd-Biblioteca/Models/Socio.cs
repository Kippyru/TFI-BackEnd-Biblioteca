namespace TFI_BackEnd_Biblioteca.Models
{
    public class Socio
    {
        public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
 public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;

        // Navegación
      public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
