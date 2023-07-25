using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Pr3Obligatorio_AAN2023.Datos;
using Pr3Obligatorio_AAN2023.Models;

namespace Pr3Obligatorio_AAN2023.Controllers
{
    public class InicioAdminController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public InicioAdminController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public ActionResult InicioAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Administrativo u)
        {
            {
                if (u != null)
                {
                    // Verificar si el correo pertenece a un administrador
                    var Administrativo = _context.Administrativos.FirstOrDefault(obj => obj.Email == u.Email);
                    if (Administrativo != null)
                    {
                        // Verificar que el correo termine con "@admin.com"
                        if (!u.Email.EndsWith("@admin.com"))
                        {
                            TempData["mensajeError"] = "No eres admin";
                            return RedirectToAction("InicioAdmin");
                        }

                        if (u.Constraseña != Administrativo.Constraseña)
                        {
                            TempData["mensajeError"] = "La contraseña es incorrecta!";
                        }
                        else
                        {
                            _cache.Set("Administrativo", Administrativo);
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        TempData["mensajeError"] = "No eres admin";
                    }
                }
                else
                {
                    TempData["mensajeError"] = "Ingrese correo y contrase単a";
                }

                return RedirectToAction("InicioAdmin");
            }
        }
    }
}