using System.ComponentModel.DataAnnotations;

namespace Pr3Obligatorio_AAN2023.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Constraseña { get; set; }
    }
}
