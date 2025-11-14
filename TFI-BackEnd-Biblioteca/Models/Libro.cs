using System.ComponentModel.DataAnnotations;

namespace TFI_BackEnd_Biblioteca.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, ErrorMessage = "El título no puede exceder 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es requerido")]
        [StringLength(150, ErrorMessage = "El autor no puede exceder 150 caracteres")]
        public string Autor { get; set; } = string.Empty;

        [StringLength(13, ErrorMessage = "El ISBN no puede exceder 13 caracteres")]
        [RegularExpression(@"^\d{10}(\d{3})?$", ErrorMessage = "El ISBN debe tener 10 o 13 dígitos")]
        public string? ISBN { get; set; }

        [StringLength(100, ErrorMessage = "La editorial no puede exceder 100 caracteres")]
        public string? Editorial { get; set; }

        [Range(1000, 2100, ErrorMessage = "El año de publicación debe estar entre 1000 y 2100")]
        public int? AñoPublicacion { get; set; }

        [StringLength(50, ErrorMessage = "El género no puede exceder 50 caracteres")]
        public string? Genero { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad disponible no puede ser negativa")]
        public int CantidadDisponible { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad total no puede ser negativa")]
        public int CantidadTotal { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Navegación
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
