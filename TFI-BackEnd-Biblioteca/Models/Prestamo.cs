using System.ComponentModel.DataAnnotations;

namespace TFI_BackEnd_Biblioteca.Models
{
    public class Prestamo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID del libro es requerido")]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "El ID del socio es requerido")]
        public int SocioId { get; set; }

        public DateTime FechaPrestamo { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "La fecha de devolución estimada es requerida")]
        public DateTime FechaDevolucionEstimada { get; set; }

        public DateTime? FechaDevolucionReal { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [RegularExpression("^(Activo|Devuelto|Atrasado)$", ErrorMessage = "El estado debe ser: Activo, Devuelto o Atrasado")]
        public string Estado { get; set; } = "Activo";

        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string? Observaciones { get; set; }

        // Navegación
        public Libro Libro { get; set; } = null!;
        public Socio Socio { get; set; } = null!;
    }
}
