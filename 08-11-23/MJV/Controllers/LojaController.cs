using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using MJV.Models;
using System.Globalization;
using System.Text;

namespace MJV.Controllers
{
    public class LojaController : Controller
    {
        // Propriedade estática para armazenar a lista de lojas
        private static List<Loja> lojas;

        static LojaController()
        {
            // Inicializa a lista de lojas apenas uma vez
            lojas = new List<Loja>();

            for (int i = 1; i <= 2; i++)
            {
                var loja = new Loja
                {
                    Id = i,
                    Nome = "Medeiros " + i,
                    Cidade = "São Joaquim",
                    sections = new List<Section>
                {
                    new Section
                    {
                        Nome = "Limpeza",
                        Produtos = new List<Produto>
                        {
                            new Produto { Nome = "Detergente", Descricao = "Macarrao de preparo rapido", valor = 1.69 },
                            new Produto { Nome = "Vanish White", Descricao = "Macarrao de preparo rapido", valor = 29.99 },
                        }
                    },
                    new Section
                    {
                        Nome = "Bebidas",
                        Produtos = new List<Produto>
                        {
                            new Produto { Nome = "Coca Cola", Descricao = "Macarrao de preparo rapido", valor = 5.49 },
                            new Produto { Nome = "Leite", Descricao = "Macarrao de preparo rapido", valor = 3.60 },
                        }
                    }
                }
                };

                lojas.Add(loja);
            }
        }

        public IActionResult Index()
        {
            return View(lojas);
        }

        public IActionResult Produtos(int id)
        {
            var lojaEncontrada = lojas.FirstOrDefault(l => l.Id == id);
            if (lojaEncontrada == null)
            {
                return NotFound();
            }

            return View(new List<Loja> { lojaEncontrada });
        }

        [HttpPost]
        public IActionResult CreateLoja(string nome, string cidade)
        {
            var loja = new Loja
            {   
                Id = lojas.Count + 1,
                Nome = nome,
                Cidade = cidade
            };
            lojas.Add(loja);
            return RedirectToAction("Index", lojas);
        }
        [HttpPost]
        public IActionResult CreateProdutos(int LojaId, string section, string nomeProduto, string nomeDescricao, string valorInput)
        {
            var lojaEncontrada = lojas.FirstOrDefault(l => l.Id == LojaId);

            if (lojaEncontrada != null)
            {
                if (lojaEncontrada.sections == null)
                {
                    lojaEncontrada.sections = new List<Section>();
                }

                var secaoEncontrada = lojaEncontrada.sections.FirstOrDefault(s => s.Nome.ToUpper() == section.ToUpper());

                if (secaoEncontrada == null)
                {
                    secaoEncontrada = new Section
                    {
                        Nome = section,
                        Produtos = new List<Produto>()
                    };

                    lojaEncontrada.sections.Add(secaoEncontrada);
                }

                var novoProduto = new Produto
                {
                    Nome = nomeProduto,
                    Descricao = nomeDescricao,
                    valor = double.Parse(valorInput.Replace(',', '.'), CultureInfo.InvariantCulture)
                };

                secaoEncontrada.Produtos.Add(novoProduto);

                return RedirectToAction("Index", lojas);
            }

           return NotFound();

        }
        public IActionResult DownloadProdutos(int id)
        {
            var loja = lojas.FirstOrDefault(l => l.Id == id);
            if (loja == null)
            {
                return NotFound();
            }

            var produtosText = new StringBuilder();

            produtosText.AppendLine($"### Produtos da Loja: {loja.Nome} ###");
            produtosText.AppendLine(); // Adiciona uma linha em branco entre os produtos

            foreach (var section in loja.sections)
            {
                produtosText.AppendLine($"### {section.Nome} ###");

                foreach (var produto in section.Produtos)
                {
                    produtosText.AppendLine($"Nome do Produto: {produto.Nome}");
                    produtosText.AppendLine($"Descrição: {produto.Descricao}");
                    produtosText.AppendLine($"Valor: {produto.valor}");
                    produtosText.AppendLine(); 
                }
            }

            var content = Encoding.UTF8.GetBytes(produtosText.ToString());
            var fileName = $"Produtos_{loja.Nome}.txt";

            // Retorna a resposta de download
            return File(content, "text/plain", fileName);
        }
    }


}
