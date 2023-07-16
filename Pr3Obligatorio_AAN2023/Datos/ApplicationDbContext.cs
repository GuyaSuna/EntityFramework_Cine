using Pr3Obligatorio_AAN2023.Models;
using Microsoft.EntityFrameworkCore;


namespace Pr3Obligatorio_AAN2023.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opciones) : base(opciones)

        {

        }

        public DbSet<Administrativo> Administrativos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Sala> Salas { get; set; }

        public DbSet<Pelicula> Peliculas { get; set;}

        public DbSet<Funcion> Funciones { get; set; }

        public DbSet<Reserva> Reservas { get; set; }

    }
}
