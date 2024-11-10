using Microsoft.AspNetCore.Mvc;

namespace CadastroLivros.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {

        public MainController()
        {
        }

        [HttpGet("pingtest")]
        public IActionResult PingTest()
        {
            return Ok("ping ok");
        }

    }
}
