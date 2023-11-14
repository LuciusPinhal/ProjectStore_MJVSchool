using ConexaoBancodeDados.dal;
using ConexaoBancodeDados.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ConexaoBancodeDados.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            //iNJETAR DEPENDENCIA 
            PostegresDAL sql = new PostegresDAL();
            //sql.ListarAlunos();
            sql.InserirAlunos("d1", "d2");

            //RECUPERAR COM BIND 


            //AppSETTINGS

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}