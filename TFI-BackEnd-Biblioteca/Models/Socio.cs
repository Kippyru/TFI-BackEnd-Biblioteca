using System.ComponentModel.DataAnnotations;

namespace TFI_BackEnd_Biblioteca.Models
{
  public class Socio
    {
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

     [Required(ErrorMessage = "El apellido es requerido")]
     [StringLength(100, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
     [StringLength(150, ErrorMessage = "El email no puede exceder 150 caracteres")]
     public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
     [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? Telefono { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        public string? Direccion { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

   public bool Activo { get; set; } = true;

        // Navegación
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
