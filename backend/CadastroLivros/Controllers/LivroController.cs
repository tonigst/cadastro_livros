using Microsoft.AspNetCore.Mvc;
using CadastroLivros.Data.Persistence.Interfaces;

namespace CadastroLivros.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LivroController : ControllerBase
    {
        private readonly ILivroPersistence _livroPersistence;

        public LivroController(ILivroPersistence livroPersistence)
        {
            _livroPersistence = livroPersistence;
        }

        [HttpGet("{codL}")]
        public async Task<IActionResult> Get([FromRoute] int codL)
        {
            var livro = await _livroPersistence.Read(codL);

            if (livro == null)
                return NotFound($"Livro não encontrado");

            return Ok(livro);
        }
    }
}
