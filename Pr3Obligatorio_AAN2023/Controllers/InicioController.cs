using Microsoft.AspNetCore.Mvc;
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
                        TempData["mensajeError"] = "La contrseña es incorrecta!";
                    }
                    else
                    {
                        return RedirectToAction("Index","Home");
                    }
                }
                else
                {
                    TempData["mensajeError"] = "Correo no existe";
                }
            }
            else
            {
                TempData["mensajeError"] = "Ingrese correo y contraseña";
            }

            return View();
        }
    }
}
