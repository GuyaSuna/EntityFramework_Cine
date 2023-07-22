using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Pr3Obligatorio_AAN2023.Datos;
using Pr3Obligatorio_AAN2023.Models;

namespace Pr3Obligatorio_AAN2023.Controllers
{
    public class InicioController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        private readonly ApplicationDbContext _context;

        public InicioController(ApplicationDbContext context)
        {
            _context = context;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Usuario u)
        {
            if (u != null)
            {
                var Usuario = _context.Usuarios.FirstOrDefault(obj => obj.Email == u.Email);
                if (Usuario != null)
                {
                    if (u.Constraseña != Usuario.Constraseña)
                    {
                        TempData["mensajeError"] = "La contrse単a es incorrecta!";
                    }
                    else
                    {
                        _cache.Set("Usuario", u);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["mensajeError"] = "Correo no existe";
                }
            }
            else
            {
                TempData["mensajeError"] = "Ingrese correo y contrase単a";
            }

            return View();
        }
    }
}