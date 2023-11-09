using Microsoft.AspNetCore.Mvc;
using MJV.Models;

namespace MJV.Controllers
{

    public class LojaController : Controller
    {
        public IActionResult Index()
        {

            Loja loja = new Loja();
            loja.Nome = "SonjaEletronics";
            loja.Cidade = "Sonjaquim";

            return View(loja);

        }
        public IActionResult Produtos()
        {
            List<Loja> lojas = new List<Loja>();

            Loja loja;

            loja = new Loja();
            loja.Nome = "SonjaEletronics";
            loja.Cidade = "Sonjaquim";
            loja.sections = new List<Section>();
            loja.sections.Add(new Section()
            {
                Nome = "Limpeza",
                Produtos = new List<Produto>()
                {
                    new Produto() { Nome = "Macarrao", Descricao = "Macarrao de preparo rapido", valor = 20.0 },
                    new Produto() { Nome = "Macarrao", Descricao = "Macarrao de preparo rapido", valor = 20.0 },
                }
            });

            loja.sections.Add(new Section()
            {
                Nome = "Cozinha",
                Produtos = new List<Produto>()
                {
                    new Produto() { Nome = "Macarrao", Descricao = "Macarrao de preparo rapido", valor = 20.0 },
                    new Produto() { Nome = "Macarrao", Descricao = "Macarrao de preparo rapido", valor = 20.0 },
                }
            });

            lojas.Add(loja);
            
            return View(lojas);
        }


    }
}
