using Api_Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Store_Project.DALPg;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Store_Project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LojaController : ControllerBase
    {

        private readonly DALPostegres _sql;
        private readonly ILogger<LojaController> _logger;

        public LojaController(DALPostegres sql, ILogger<LojaController> logger)
        {
            _sql = sql;
            _logger = logger;
        }


        [HttpGet]
        [Route("Get")]
        public IEnumerable<Loja> Get()
        {
            try
            {
                var lojas = _sql.ListLojaDB();
                return lojas;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter a lista de lojas: {ex.Message}");
                return null;
            }

        }



        [HttpPost]
        [Route("Post")]
        public IActionResult CreateLoja([FromBody] Loja novaLoja)
        {
            if (novaLoja == null)
            {
                return BadRequest("Dados inválidos para criação de loja.");
            }

            try
            {
                novaLoja.Id = _sql.GetUltimoIdLoja();

                var loja = new Loja
                {
                    Id = novaLoja.Id,
                    Nome = novaLoja.Nome,
                    Cidade = novaLoja.Cidade
                };

                _sql.CreateLoja(novaLoja);

                return Ok("Loja criada com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro não esperado: {ex.Message}");
                return StatusCode(500, "Erro interno no servidor");
            }

        }

        [HttpPost]
        [Route("SectionAndProduto/Post")]
   
        public IActionResult CreateProdutos([FromBody] Loja request)
        {
            if (request == null)
            {
                return BadRequest("Dados inválidos para criação de seção e produto.");
            }

            try
            {
                // Obter a loja correspondente com todos os produto que tinha 
                var lojaEncontrada = _sql.GetLojaById(request.Id);


                //logica para adcionar os produtos na loja nova.
                if (lojaEncontrada != null)
                {
                    // Verificar se a lista de seções está inicializada
                    if (lojaEncontrada.Sections == null || lojaEncontrada.Sections.Count == 0)
                    {
                        lojaEncontrada.Sections = new List<Section>();
                    }

                    //Encontrar a seção correspondente
                    var Secao = request.Sections[0];
                    var secaoEncontrada = lojaEncontrada.Sections.FirstOrDefault(s => s.Nome.ToUpper().Trim() == Secao.Nome.ToUpper().Trim());
                    if (secaoEncontrada == null)
                    {
                        // Se a seção não existir, criá-la
                        secaoEncontrada = new Section
                        {
                     
                            Nome = Secao.Nome,
                            Produtos = new List<Produto>()
                        };

                        // Adicionar a nova seção à lista de seções da loja
                        lojaEncontrada.Sections.Add(secaoEncontrada);
                    }
                    var Produtos = request.Sections[0].Produtos[0];


                    secaoEncontrada.Id = _sql.CreateSecao(lojaEncontrada.Id, secaoEncontrada);

                    var novoProduto = new Produto
                    {

                        Nome = Produtos.Nome,
                        Descricao = Produtos.Descricao,
                        Valor = Produtos.Valor,
                        Section_id = secaoEncontrada.Id
                    };

                    // Adicionar o produto à lista de produtos da seção
                    secaoEncontrada.Produtos.Add(novoProduto);

                    // Chamar o método na DALPostegres para criar a seção e o produto no banco de dados
                    

                    _sql.CreateProduto(novoProduto);

                    return Ok("Seção criada com sucesso");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro não esperado: {ex.Message}");
                return StatusCode(500, "Erro interno no servidor");
            }

            return NotFound();
        }


        [HttpPut("Put")]
        public IActionResult EditLoja([FromBody] Loja model)
        {
   
            try
            {
                Loja lojaEditada = _sql.GetLojaById(model.Id);
                if (lojaEditada != null)
                {
                    lojaEditada.Nome = model.Nome;
                    lojaEditada.Cidade = model.Cidade;

                    _sql.EditeLoja(lojaEditada);
                }
                else
                {
                    // Trate o caso em que a loja não foi encontrada
                    return NotFound("Loja não encontrada");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro não esperado: {ex.Message}");
                // Trate o erro adequadamente e retorne um status de erro
                return StatusCode(500, "Erro interno do servidor");
            }

            // Retorna um status de sucesso
            return Ok("Loja alterada com sucesso");
        }

        [HttpDelete("Delete/{DeleteId}")]
        public IActionResult DeleteLoja(int DeleteId)
        {
            try
            {                 
               _sql.DeleteLoja(DeleteId);

                return Ok("Loja excluída com sucesso");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro não esperado: {ex.Message}");
                // Trate o erro adequadamente e retorne um status de erro
                return StatusCode(500, "Erro interno do servidor");
            }
        
        }
    }
}












