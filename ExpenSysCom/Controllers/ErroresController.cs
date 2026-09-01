using Microsoft.AspNetCore.Mvc;

namespace ExpenSysCom.Controllers
{
    public class ErroresController : Controller
    {
        // Vista para Acceso Denegado (403)
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        // Vista para Error Genérico
        public IActionResult Error()
        {
            return View("Error");
        }

        // Vista para códigos de estado (ej: 404)
        [Route("Errores/StatusCode/{code}")]
        public IActionResult StatusCode(int code)
        {
            // URL de la página anterior
            var returnUrl = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = "/"; // fallback al inicio si no hay referer

            ViewBag.ReturnUrl = returnUrl;

            if (code == 404)
                return View("Error404");
            else
                return View("Error");
        }
    }
}
