using Microsoft.AspNetCore.Mvc;

namespace FinanzasPlus.Controllers.Historial
{
    public class HistorialController : Controller
    {
        public IActionResult HistorialTransacciones()
        {
            return View();
        }
        public IActionResult HacerTransaccion()
        {
            return View();
        }
    }
}
