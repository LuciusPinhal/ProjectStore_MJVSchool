using Microsoft.AspNetCore.Mvc;

namespace MJV.Controllers
{
    public class CoxinhaController : Controller
    {
        public IActionResult Index()
        {
            Exemplo.calculaTarifaServico calcula = new Exemplo.calculaTarifaServico();
            calcula.cepOrigem = "14500";



            return View();
        }
        public IActionResult Pastel()
        {
            return View();
        }
    }
}
