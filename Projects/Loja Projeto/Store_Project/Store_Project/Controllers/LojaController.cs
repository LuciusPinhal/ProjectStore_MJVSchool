﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Store_Project.DALPg;
using Store_Project.Models;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Store_Project.Controllers
{
    public class LojaController : Controller
    {
        // Propriedade estática para armazenar a lista de lojas
        private static List<Loja> lojas;
    
        static LojaController()
        {

            DALPostegres sql = new DALPostegres();
            // Inicializa a lista de lojas apenas uma vez
            lojas = sql.ListLojaDB();

            //for (int i = 2; i <= 4; i++)
            //{
            //    var loja = new Loja
            //    {
            //        Id = i,
            //        Nome = "Medeiros " + i,
            //        Cidade = "São Joaquim",
            //        Sections = new List<Section>
            //    {
            //        new Section
            //        {
            //            Nome = "Limpeza",
            //            Produtos = new List<Produto>
            //            {
            //                new Produto { Nome = "Detergente", Descricao = "Produto para limpeza", Valor = 1.69 },
            //                new Produto { Nome = "Vanish White", Descricao = "Produto para limpeza", Valor = 29.99 },
            //            }
            //        },
            //        new Section
            //        {
            //            Nome = "Bebidas",
            //            Produtos = new List<Produto>
            //            {
            //                new Produto { Nome = "Coca Cola", Descricao = "Refrigerante", Valor = 5.49 },
            //                new Produto { Nome = "Leite", Descricao = "Bebidas", Valor = 3.60 },
            //            }
            //        }
            //    }
            //    };

            //    lojas.Add(loja);
            //}
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
            var verificaLoja = lojas.FirstOrDefault(n => n.Nome == nome);
            string NomeVerficado = string.Empty;


            if (verificaLoja == null)
            {
                NomeVerficado = nome;
            }
            else
            {
                int Qtd = lojas.Count(n => n == verificaLoja);
                NomeVerficado = nome + " " + Qtd;
            }

            var loja = new Loja
            {
                Id = lojas.Count + 1,
                Nome = NomeVerficado,
                Cidade = cidade
            };

            lojas.Add(loja);
            DALPostegres sql = new DALPostegres();
            sql.CreateLoja(loja);
            return RedirectToAction("Index", lojas);

            //Tem dois mederos 1 tenho que ajustar a logica 
        }
        [HttpPost]
        public IActionResult CreateProdutos(int LojaId, string section, string nomeProduto, string nomeDescricao, string valorInput)
        {
            DALPostegres sql = new DALPostegres();
  
            var lojaEncontrada = lojas.FirstOrDefault(l => l.Id == LojaId);

            if (lojaEncontrada != null)
            {
                if (lojaEncontrada.Sections == null)
                {
                    lojaEncontrada.Sections = new List<Section>();
                }

                var secaoEncontrada = lojaEncontrada.Sections.FirstOrDefault(s => s.Nome.ToUpper().Trim() == section.ToUpper().Trim());
                //var idEncotrado = lojaEncontrada.Sections.FirstOrDefault(s => s.Id == section.Id);

                if (secaoEncontrada == null)
                {
                    secaoEncontrada = new Section
                    {
                        Nome = section,
                        Produtos = new List<Produto>()
                    };

                    sql.CreateSecao(lojaEncontrada.Id, secaoEncontrada);
                    lojaEncontrada.Sections.Add(secaoEncontrada);
                }

                var novoProduto = new Produto
                {
                    Nome = nomeProduto,
                    Descricao = nomeDescricao,
                    Valor = double.Parse(valorInput.Replace(',', '.'), CultureInfo.InvariantCulture)
                };
                Console.WriteLine(secaoEncontrada.Id);
        
                //sql.CreateProduto(secaoEncontrada.Id, novoProduto);
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

            foreach (var section in loja.Sections)
            {
                produtosText.AppendLine($"### {section.Nome} ###");

                foreach (var produto in section.Produtos)
                {
                    produtosText.AppendLine($"Nome do Produto: {produto.Nome}");
                    produtosText.AppendLine($"Descrição: {produto.Descricao}");
                    produtosText.AppendLine($"Valor: {produto.Valor}");
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
