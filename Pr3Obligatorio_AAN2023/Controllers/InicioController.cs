using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Pr3Obligatorio_AAN2023.Datos;
using Pr3Obligatorio_AAN2023.Models;

namespace Pr3Obligatorio_AAN2023.Controllers
{
    public class InicioController : Controller
    {
       
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
 
        public InicioController(ApplicationDbContext context , IMemoryCache cache)
        {
            _context = context;
            _cache = cache;  
        }
        public ActionResult Inicio()
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
                        TempData["mensajeError"] = "La contrseña es incorrecta!";
                    }
                    else
                    {
                        _cache.Set("Usuario", Usuario);
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
                TempData["mensajeError"] = "Ingrese correo y contraseña";
            }

            return View();
        }
    }
}
