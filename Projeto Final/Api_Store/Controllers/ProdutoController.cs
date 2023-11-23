using Api_Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Store_Project.DALPg;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Store_Project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {

        private readonly DALPostegres _sql;
        private readonly ILogger<ProdutoController> _logger;

        public ProdutoController(DALPostegres sql, ILogger<ProdutoController> logger)
        {
            _sql = sql;
            _logger = logger;
        }

        [HttpPost]
        [Route("Post")]

        public IActionResult CreateProduto([FromBody] Loja request)
        {
            if (request == null)
            {
                return BadRequest("Dados inválidos para criação de seção e produto.");
            }

            try
            {
                var lojaEncontrada = _sql.GetLojaById(request.Id);

                Section sectionfound = lojaEncontrada.Sections.FirstOrDefault(s => s.Id == request.Sections[0].Id);

                var newProduto = request.Sections[0].Produtos[0];

                if (sectionfound != null)
                {
                    Produto produto = new Produto
                    {
                        Nome = newProduto.Nome,
                        Descricao = newProduto.Descricao,
                        Valor = newProduto.Valor,
                        Section_id = sectionfound.Id,
                    };

                    sectionfound.Produtos.Add(produto);

                    _sql.CreateProduto(produto);
                }
                else
                {
                    return StatusCode(404, "Seção não encontrada");
                }

       
               return Ok("Seção criada com sucesso");
                
            }
            catch (Exception ex)
            {             
                return StatusCode(500, "Erro interno no servidor");
            }
        }

    }
}












