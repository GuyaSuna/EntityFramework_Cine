using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pr3Obligatorio_AAN2023.Datos;
using Pr3Obligatorio_AAN2023.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace Pr3Obligatorio_AAN2023.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMemoryCache cache)
        {
            _logger = logger;
            _context = context;
            _cache = cache;
        }

        public IActionResult Buscar(string searchString)
        {
            var resultados = _context.Peliculas.Where(e => e.Titulo.Contains(searchString)).ToList();
            return View(resultados);
        }

        public IActionResult Index()
        {
            var funciones = _context.Funciones.Include(f => f.Sala).Include(f => f.Pelicula).ToList();
            var Usuario = _cache.Get("Usuario") as Usuario;
            if (Usuario != null)
            {
                ViewData["Usuario"] = Usuario;
            }

            // Filtrar las funciones por las fechas y horas válidas
            funciones = FiltrarFuncionesPorFechaYHora(funciones);

            return View(funciones);
        }

        [HttpPost]
        public IActionResult Index(int peliculaId)
        {
            var salas = _context.Funciones
                .Where(f => f.Pelicula.Id == peliculaId)
                .Select(f => f.Sala)
                .Distinct()
                .ToList();

            var viewModel = new
            {
                Peliculas = _context.Peliculas.ToList(),
                Funciones = _context.Funciones.Include(f => f.Sala).ToList(),
                Salas = salas
            };

            return View(viewModel);
        }

        private static List<Funcion> FiltrarFuncionesPorFechaYHora(List<Funcion> funciones)
        {
            var fechaHoraActual = DateTime.Now;


            funciones = funciones.Where(f => DateTime.Parse(f.Fecha) >= fechaHoraActual.Date).ToList();


            funciones = funciones.Where(f => (DateTime.Parse(f.Fecha) > fechaHoraActual.Date) ||
                                        (DateTime.Parse(f.Fecha) == fechaHoraActual.Date && TimeSpan.Parse(f.Horario) > fechaHoraActual.TimeOfDay)).ToList();

            return funciones;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            // Eliminar el usuario o administrativo de la memoria caché para cerrar la sesión
            HttpContext.RequestServices.GetService<IMemoryCache>().Remove("Usuario");
            HttpContext.RequestServices.GetService<IMemoryCache>().Remove("Administrativo");

            // Redirigir a la acción Inicio o a cualquier otra página que desees después del logout
            return RedirectToAction("Index", "Home");
        }
    }
}

