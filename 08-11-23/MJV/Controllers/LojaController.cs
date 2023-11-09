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
            for (int i = 1; i <= 5; i++)
            {
                loja = new Loja();
                loja.Nome = "Seção "+ i ;

      
                loja.Produtos = new List<Produto>();
                loja.Produtos.Add(new Produto() { Nome = "Macarrao", Descricao = "Macarrao de preparo rapido", valor = 20.0 });
                loja.Produtos.Add(new Produto() { Nome = "Macarrao", Descricao = "Macarrao de preparo rapido", valor = 20.0 });
                loja.Produtos.Add(new Produto() { Nome = "Macarrao", Descricao = "Macarrao de preparo rapido", valor = 20.0 });

                lojas.Add(loja);
            }

            return View(lojas);
        }
    }
}
