using Microsoft.AspNetCore.Mvc;

namespace PrimeiraAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ControllerAluno : ControllerBase
    {
        private static readonly string[] Alunos = new[]
        {
            "Lucius", "Bruno", "Raul"
        };

        private readonly ILogger<ControllerAluno> _logger;

        public ControllerAluno(ILogger<ControllerAluno> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("alunos")]
        [Route("Get")]
        public IEnumerable<Aluno> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Aluno
            {
                Id= 1,
                nome = Alunos[Random.Shared.Next(Alunos.Length)],
                sobrenome = "Pinhal"
            })
            .ToArray();
        }

        [HttpPost]
        [Route("gravar-alunos")]
        public IActionResult Gravar([FromBody]Aluno aluno)
        {
            return Ok(aluno);
        }

        [HttpPut]
        [Route("atualizar")]
        [Route("{id}")]
        public IActionResult Atualizar([FromBody] Aluno aluno)
        {
            return Ok(aluno);
        }
    }
}