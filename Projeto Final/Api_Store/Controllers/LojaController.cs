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












