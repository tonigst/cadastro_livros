using Microsoft.AspNetCore.Mvc;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Services.Interfaces;
using CadastroLivros.Service;
using CadastroLivros.Data.Models;

namespace CadastroLivros.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LivroController : ControllerBase
    {
        private readonly ILivroService _livroService;

        public LivroController(ILivroService livroService)
        {
            _livroService = livroService;
        }

        [HttpGet("{codL}")]
        public async Task<IActionResult> Get([FromRoute] long codL)
        {
            var livro = await _livroService.Read(codL);
            return Ok(livro);
        }

        [HttpGet("page/{page}/{pageSize}")]
        public async Task<IActionResult> GetPage([FromRoute] int page, [FromRoute] int pageSize)
        {
            var livros = await _livroService.ReadPage(page, pageSize);
            return Ok(livros);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] LivroDTO livroDTO)
        {
            var livroResult = await _livroService.Insert(livroDTO);
            return Ok(livroResult);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LivroDTO livroDTO)
        {
            await _livroService.Update(livroDTO);
            return Ok();
        }

        [HttpDelete("{codL}")]
        public async Task<IActionResult> Delete([FromRoute] long codL)
        {
            await _livroService.Delete(codL);
            return Ok();
        }
    }
}
