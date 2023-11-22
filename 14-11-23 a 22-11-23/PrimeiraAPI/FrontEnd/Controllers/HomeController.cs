using FrontEnd.Extentions;
using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("ClienteTempo");

            //var response = await client.GetAsync("/WeatherForecast");
            var listaTemperatura = await client.GetFromJsonAsync<List<Teste>>("/WeatherForecast");

            
            
            //nao precisa passar o parametro pq o paramentro fica a idade
            int idade = 0;
            var u = idade.Duplicar();
            return View(listaTemperatura);
        }

        public async Task<IActionResult> Privacy()
        {
            var client = _httpClientFactory.CreateClient("Alunos");
            var listaAlunos = await client.GetFromJsonAsync<List<Aluno>>("/ControllerAluno/Alunos");
         
            return View(listaAlunos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}