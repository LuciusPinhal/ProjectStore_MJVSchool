using Microsoft.AspNetCore.Mvc;

namespace MJV.Controllers
{
    public class CoxinhaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Pastel()
        {
            return View();
        }
    }
}
