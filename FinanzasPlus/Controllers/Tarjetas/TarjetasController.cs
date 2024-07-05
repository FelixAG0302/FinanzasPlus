using Microsoft.AspNetCore.Mvc;

namespace FinanzasPlus.Controllers.Tarjetas
{
    public class TarjetasController : Controller
    {
        public IActionResult AgregarTarjetas()
        {
            return View();
        }

        public IActionResult VerTarjetas()
        {
            return View();
        }
    }
}
