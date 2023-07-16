using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pr3Obligatorio_AAN2023.Models
{
    public class Funcion
    {
        [Key]
        public int Id { get; set; }

        public Pelicula? Pelicula { get; set; }

        public Sala? Sala { get; set; }

        public string? Fecha { get; set; }  

        public string? Horario { get; set; }
    }
}
