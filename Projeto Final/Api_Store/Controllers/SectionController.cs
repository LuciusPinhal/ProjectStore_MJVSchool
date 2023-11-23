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
    public class SectionController : ControllerBase
    {

        private readonly DALPostegres _sql;
        private readonly ILogger<SectionController> _logger;

        public SectionController(DALPostegres sql, ILogger<SectionController> logger)
        {
            _sql = sql;
            _logger = logger;
        }

        [HttpPost]
        [Route("Post")]
   
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

    }
}












