using System.ComponentModel.DataAnnotations;

namespace Pr3Obligatorio_AAN2023.Models
{
    public class Sala
    {
        [Key]
        public int NroSala { get; set; }

        public int CantAsientos { get; set; }
    }
}
