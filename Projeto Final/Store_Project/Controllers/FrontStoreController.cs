using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Store_Project.Models;
using System.Globalization;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


namespace Store_Project.Controllers
{
    public class FrontStoreController : Controller
    {
        // Propriedade estática para armazenar a lista de lojas
        private static List<Loja> lojas;
        string NomeVerficado = string.Empty;

        private readonly IHttpClientFactory _httpClientFactory;

        public FrontStoreController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("Store");
            lojas = await client.GetFromJsonAsync<List<Loja>>("/Loja/Get");
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
        public async Task<IActionResult> CreateLoja(string nome, string cidade)
        {
            NomeVerficado = VerificaNome(nome);
            var client = _httpClientFactory.CreateClient("Store");

            try
            {
                var loja = new Loja
                {
                    Id = lojas.Count + 1,
                    Nome = NomeVerficado,
                    Cidade = cidade
                };

                HttpResponseMessage response = await client.PostAsJsonAsync("/Loja/Post", loja);
                string mensagem = await response.Content.ReadAsStringAsync();
                mensagem = mensagem.Trim('"');

                if (response.IsSuccessStatusCode) {

                    TempData["Sucesso"] = mensagem;
                }
                else
                {
                    TempData["Erro"] = mensagem;
                    return StatusCode((int)response.StatusCode, "Erro na criação das lojas");
                }


                lojas.Add(loja);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro não esperado: {ex.Message}");
                TempData["Erro"] = "Ocorreu um erro inesperado. Por favor, tente novamente.";
            }

            return RedirectToAction("Index", lojas);


        }

        [HttpPost]
        public async Task<IActionResult> CreateProdutos(int LojaId, string? section, string selectedSection, string nomeProduto, string nomeDescricao, string valorInput)
        {

            try
            {

                if (section == null)
                {
                    section = selectedSection;
                }

                if (double.TryParse(valorInput.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double valor))
                {

                    Loja CreateLojaMin = new()
                    {
                        Id = LojaId,
                        Nome = " ",
                        Cidade = " ",
                        Sections = new List<Section>
                        {
                            new Section
                            {
                                Nome = section,
                                Produtos = new List<Produto>
                                {
                                    new Produto
                                    {
                                        Nome = nomeProduto,
                                        Descricao= nomeDescricao,
                                        Valor = valor,
                                    }
                                }
                            }

                        }
                    };

                    var client = _httpClientFactory.CreateClient("Store");

                    HttpResponseMessage response = await client.PostAsJsonAsync("/Loja/SectionAndProduto/Post", CreateLojaMin);
                    string mensagem = await response.Content.ReadAsStringAsync();

                    mensagem = mensagem.Trim('"');

                    if (response.IsSuccessStatusCode)
                    {

                        TempData["Sucesso"] = mensagem;
                    }
                    else
                    {
                        TempData["Erro"] = mensagem;

                    }

                }

                else
                {
                    TempData["Erro"] = "Campo 'VALOR' tem que ser um numero";

                }

                return RedirectToAction("Index");

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Erro não esperado: {ex.Message}");
                TempData["Erro"] = "Ocorreu um erro inesperado. Por favor, tente novamente.";
            }

            return NotFound();

        }

        public async Task<IActionResult> EditeLoja(int EditeId, string nome, string cidade)
        {
            NomeVerficado = VerificaNome(nome);

            try
            {

                Loja lojaEditada = lojas.FirstOrDefault(l => l.Id == EditeId);
                if (lojaEditada != null)
                {
                    lojaEditada.Nome = NomeVerficado;
                    lojaEditada.Cidade = cidade;                 
                }
                else
                {
                    TempData["Erro"] = "Loja não encontrada na lista.";
                }

                var client = _httpClientFactory.CreateClient("Store");
                HttpResponseMessage response = await client.PutAsJsonAsync("/Loja/Put", lojaEditada);

                string mensagem = await response.Content.ReadAsStringAsync();

                mensagem = mensagem.Trim('"');

                if (response.IsSuccessStatusCode)
                {

                    TempData["Sucesso"] = mensagem;
                }
                else
                {
                    TempData["Erro"] = mensagem;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro não esperado: {ex.Message}");
                TempData["Erro"] = "Ocorreu um erro inesperado. Por favor, tente novamente.";
            }

            return RedirectToAction("Index", lojas);
        }

        public async Task<IActionResult> DeleteLoja(int DeleteId)
        {

            try
            {
                var client = _httpClientFactory.CreateClient("Store");
                HttpResponseMessage response = await client.DeleteAsync($"/Loja/Delete/{DeleteId}");

                string mensagem = await response.Content.ReadAsStringAsync();

                mensagem = mensagem.Trim('"');

                if (response.IsSuccessStatusCode)
                {

                    TempData["Sucesso"] = mensagem;
                }
                else
                {
                    TempData["Erro"] = mensagem;

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro não esperado: {ex.Message}");
                TempData["Erro"] = "Ocorreu um erro inesperado. Por favor, tente novamente.";
            }

            return RedirectToAction("Index", lojas);
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
        public string VerificaNome(string nome)
            {
                var verificaLoja = lojas.FirstOrDefault(n => n.Nome == nome);

                if (verificaLoja == null)
                {
                    NomeVerficado = nome;
                }
                else
                {
                    int Qtd = lojas.Count(n => n == verificaLoja);
                    NomeVerficado = nome + " " + Qtd;
                }

                return NomeVerficado;
            }
    }

}
