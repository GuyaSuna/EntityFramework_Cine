﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pr3Obligatorio_AAN2023.Datos;
using Pr3Obligatorio_AAN2023.Models;
using System.Diagnostics;

namespace Pr3Obligatorio_AAN2023.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var peliculas = _context.Peliculas.ToList();
            var funciones = _context.Funciones.Include(f => f.Sala).ToList();

            var viewModel = new HomeViewModel
            {
                Peliculas = peliculas,
                Funciones = funciones
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(int peliculaId)
        {
            var salas = _context.Funciones
                .Where(f => f.Pelicula.Id == peliculaId)
                .Select(f => f.Sala)
                .Distinct()
                .ToList();

            var viewModel = new HomeViewModel
            {
                Peliculas = _context.Peliculas.ToList(),
                Funciones = _context.Funciones.Include(f => f.Sala).ToList(),
                Salas = salas
            };

            return View(viewModel);
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
    }
}
