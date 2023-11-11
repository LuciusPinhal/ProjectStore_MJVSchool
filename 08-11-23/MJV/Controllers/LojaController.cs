using Microsoft.AspNetCore.Mvc;
using MJV.Models;
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
                    Nome = "SonjaEletronics " + i,
                    Cidade = "Sonjaquim " + i,
                    sections = new List<Section>
                {
                    new Section
                    {
                        Nome = "Limpeza",
                        Produtos = new List<Produto>
                        {
                            new Produto { Nome = "Macarrao", Descricao = "Macarrao de preparo rapido", valor = 20.0 },
                            new Produto { Nome = "Macarrao", Descricao = "Macarrao de preparo rapido", valor = 20.0 },
                        }
                    },
                    new Section
                    {
                        Nome = "Cozinha",
                        Produtos = new List<Produto>
                        {
                            new Produto { Nome = "Macarrao" + i, Descricao = "Macarrao de preparo rapido", valor = 20.0 },
                            new Produto { Nome = "Macarrao", Descricao = "Macarrao de preparo rapido", valor = 20.0 },
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
        public IActionResult CreateProdutos(int LojaId, string section, string nomeProduto, string nomeDescricao, double valor)
        {
            var lojaEncontrada = lojas.FirstOrDefault(l => l.Id == LojaId);

            if (lojaEncontrada != null)
            {
                if (lojaEncontrada.sections == null)
                {
                    // Cria uma nova lista de seções se for nula
                    lojaEncontrada.sections = new List<Section>();
                }

                // Procura a seção existente pelo nome
                var secaoEncontrada = lojaEncontrada.sections.FirstOrDefault(s => s.Nome == section);

                if (secaoEncontrada == null)
                {
                    // Cria uma nova seção e adiciona à lista de seções
                    secaoEncontrada = new Section
                    {
                        Nome = section,
                        Produtos = new List<Produto>()
                    };

                    lojaEncontrada.sections.Add(secaoEncontrada);
                }

                // Adiciona um novo produto à seção existente ou recém-criada
                var novoProduto = new Produto
                {
                    Nome = nomeProduto,
                    Descricao = nomeDescricao,
                    valor = valor
                };

                secaoEncontrada.Produtos.Add(novoProduto);

                // Aqui, você pode realizar outras ações, como salvar a loja atualizada no seu repositório de dados

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
