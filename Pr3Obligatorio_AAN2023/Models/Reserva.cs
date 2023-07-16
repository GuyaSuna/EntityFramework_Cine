using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pr3Obligatorio_AAN2023.Models
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }

        public Funcion? Funcion { get; set; }

        public Usuario? Usuario { get; set; }

        public int Asiento { get; set; }    

        public int Precio { get; set; } 
    }
}
