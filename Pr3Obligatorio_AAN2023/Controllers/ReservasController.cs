using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pr3Obligatorio_AAN2023.Datos;
using Pr3Obligatorio_AAN2023.Models;




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
        // Resto del código del controlador...

        // GET: Reservas/Create
        public IActionResult Create(int funcionId)
        {
            var Usuario = _cache.Get("Usuario") as Usuario;
            var Funciones = _context.Funciones.Include(r => r.Sala).Include(r => r.Pelicula).ToList();
            var Reservas = _context.Reservas.Include(r => r.Funcion).ToList();

            // Obtener la función actual
            var funcion = Funciones.FirstOrDefault(f => f.Id == funcionId);

            if (funcion == null)
            {
                // Si no se encontró la función con el Id proporcionado,
                // muestra un mensaje de error o redirige a otra página
                return RedirectToAction("Index", "Funciones");
            }

            // Obtener la cantidad total de asientos en la sala
            int cantidadTotalAsientos = funcion.Sala.CantAsientos;

            // Calcular la cantidad de asientos reservados para la función actual
            int cantidadAsientosReservados = Reservas
                .Where(r => r.Funcion.Id == funcionId)
                .Sum(r => r.Asiento);

            // Calcular la cantidad de asientos disponibles
            int cantidadAsientosDisponibles = cantidadTotalAsientos - cantidadAsientosReservados;

            if (cantidadAsientosDisponibles <= 0)
            {
                TempData["mensajeError"] = "No quedan asientos suficientes para su reserva, intente con menos asientos";
                return RedirectToAction("Index", "Funciones");
            }

            if (Usuario != null)
            {
                ViewData["Usuario"] = Usuario.Id; // Asignar el ID del usuario a ViewData
                ViewData["FuncionId"] = funcionId;
                ViewData["FuncionTitulo"] = funcion.Pelicula.Titulo;
                ViewData["FuncionFecha"] = funcion.Fecha;
                ViewData["FuncionHorario"] = funcion.Horario;
                ViewData["FuncionSalaNr"] = funcion.Sala.NroSala;
                return View();
            }
            else
            {
                return NotFound();
            }
        }

        // Resto del código del controlador...






        // POST: Reservas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, volvemos a mostrar el formulario con los errores
                return View(reserva);
            }

            var funcionId = int.Parse(Request.Form["Funcion"]);

            // Obtener el valor de FuncionId desde ViewData
            if (funcionId == null)
            {
                // Si no se pudo obtener el valor de funcionId correctamente, redirigir a otra página o mostrar un mensaje de error
                Console.WriteLine("Nope x2"); 
                return RedirectToAction("Index", "Reservas");
            }

            var funcion = await _context.Funciones
               .Include(r => r.Sala)
               .Include(r => r.Pelicula)
               .FirstOrDefaultAsync(f => f.Id == funcionId);

            if (funcion == null)
            {
                // Si no se encontró la función con el Id proporcionado,
                // muestra un mensaje de error o redirige a otra página
                return RedirectToAction("Create", "Reservas", new { funcionId });
            }

            // Obtener la cantidad total de asientos en la sala
            int cantidadTotalAsientos = funcion.Sala.CantAsientos;

            // Calcular la cantidad de asientos reservados para la función actual
            int cantidadAsientosReservados = _context.Reservas
                .Where(r => r.Funcion.Id == funcionId && r.Id != reserva.Id) // Excluir la reserva actual si es una edición
                .Sum(r => r.Asiento);

            // Calcular la cantidad de asientos disponibles
            int cantidadAsientosDisponibles = cantidadTotalAsientos - cantidadAsientosReservados;

            if (reserva.Asiento > cantidadAsientosDisponibles)
            {
                TempData["mensajeError"] = "No hay suficientes asientos disponibles para su reserva, intente con menos asientos";
                return RedirectToAction("Create", "Reservas", new { funcionId });
            }

            // Obtener el valor de usuarioId desde el formulario
            int usuarioId = int.Parse(Request.Form["Usuario"]);

            // Buscar el Usuario en la base de datos por su Id
            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            if (usuario == null)
            {
                // Si no se encontró el usuario con el Id proporcionado,
                // muestra un mensaje de error o redirige a otra página
                Console.WriteLine("nope");
                return RedirectToAction("Index", "Reservas");
            }

            // Asignar los objetos Funcion y Usuario a la reserva
            reserva.Funcion = funcion;
            reserva.Usuario = usuario;

            // Agregar la reserva a la base de datos
            _context.Add(reserva);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
