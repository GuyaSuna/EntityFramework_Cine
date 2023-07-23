using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pr3Obligatorio_AAN2023.Datos;
using Pr3Obligatorio_AAN2023.Models;

namespace Pr3Obligatorio_AAN2023.Controllers
{
    public class FuncionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FuncionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Funcions
        public async Task<IActionResult> Index()
        {
            var funciones = await _context.Funciones
                .Include(f => f.Pelicula)
                .Include(f => f.Sala)
                .ToListAsync();

            if (funciones != null)
            {
                return View(funciones);
            }
            else
            {
                return Problem("Entity set 'ApplicationDbContext.Funciones' is null.");
            }
        }


        // GET: Funcions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Funciones == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcion == null)
            {
                return NotFound();
            }

            return View(funcion);
        }

        // GET: Funcions/Create
        public IActionResult Create()
        {
            ViewBag.Peliculas = _context.Peliculas.ToList();
            ViewBag.Salas = _context.Salas.ToList();

            return View();
        }


        // POST: Funcions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Pelicula,Sala,Fecha,Horario")] Funcion funcion)
        {
            if (ModelState.IsValid)
            {
                // Obtener los objetos de Pelicula y Sala según los identificadores seleccionados
                funcion.Pelicula = await _context.Peliculas.FindAsync(funcion.Pelicula.Id);
                funcion.Sala = await _context.Salas.FindAsync(funcion.Sala.NroSala);

                _context.Add(funcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(funcion);
        }

        // GET: Funcions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Funciones == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones.FindAsync(id);
            if (funcion == null)
            {
                return NotFound();
            }
            return View(funcion);
        }

        // POST: Funcions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Horario")] Funcion funcion)
        {
            if (id != funcion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionExists(funcion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(funcion);
        }

        // GET: Funcions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Funciones == null)
            {
                return NotFound();
            }

            var funcion = await _context.Funciones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcion == null)
            {
                return NotFound();
            }

            return View(funcion);
        }

        // POST: Funcions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Funciones == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Funciones'  is null.");
            }
            var funcion = await _context.Funciones.FindAsync(id);
            if (funcion != null)
            {
                _context.Funciones.Remove(funcion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FuncionExists(int id)
        {
            return (_context.Funciones?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
