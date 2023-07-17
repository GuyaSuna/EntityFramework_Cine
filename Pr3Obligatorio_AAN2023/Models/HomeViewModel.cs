using System.Collections.Generic;
using Pr3Obligatorio_AAN2023.Models;

namespace Pr3Obligatorio_AAN2023.Models
{
    public class HomeViewModel
    {
        public List<Pelicula> Peliculas { get; set; }
        public List<Funcion> Funciones { get; set; }
        public List<Sala> Salas { get; set; }
    }
}
