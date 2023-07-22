using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pr3Obligatorio_AAN2023.Datos;
using Pr3Obligatorio_AAN2023.Models;
using System.Diagnostics;
using System.Threading.Tasks;

   


namespace Pr3Obligatorio_AAN2023.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
           
        public ReservasController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Funcion) // Cargar la propiedad de navegación Funcion
                .ThenInclude(f => f.Pelicula) // Cargar la propiedad de navegación Pelicula dentro de Funcion
                .ToListAsync();

            return View(reservas);
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create(int funcionId)
        {
            var Usuario = _cache.Get("Usuario") as Usuario; // Asegúrate de que Usuario sea el tipo correcto

            if (Usuario != null)
            {
                ViewData["Usuario"] = Usuario.Id;
                ViewData["Funcion"] = funcionId;
                return View();
            }
            else
            {
                return NotFound(); // Otra acción en caso de que el usuario no exista en la memoria caché
            }
        }


        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            // Obtener el valor de Funcion y Usuario desde el formulario
            int funcionId = int.Parse(Request.Form["Funcion"]);
            int usuarioId = int.Parse(Request.Form["Usuario"]);

            // Buscar la Funcion y el Usuario en la base de datos por su Id
            var funcion = await _context.Funciones.FindAsync(funcionId);
            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            if (funcion == null || usuario == null)
            {
                // Si no se encontró la función o el usuario con el Id proporcionado,
                // muestra un mensaje de error o redirige a otra página
                return RedirectToAction("Index", "Funciones");
            }

            // Asignar los objetos Funcion y Usuario a la reserva
            reserva.Funcion = funcion;
            reserva.Usuario = usuario;

            if (ModelState.IsValid)
            {
                // Agregar la reserva a la base de datos
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si el modelo no es válido, volvemos a mostrar el formulario con los errores
            return View(reserva);
        }


        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Asiento,Precio")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
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
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservas'  is null.");
            }
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
          return (_context.Reservas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
