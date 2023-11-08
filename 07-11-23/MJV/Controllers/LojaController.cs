using Microsoft.AspNetCore.Mvc;


namespace MJV.Controllers
{

    public class LojaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Produtos()
        {
            return View();
        }
    }
}
