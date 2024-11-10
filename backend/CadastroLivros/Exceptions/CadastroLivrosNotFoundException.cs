using System.Net;

namespace CadastroLivros.Exceptions
{
    public class CadastroLivrosNotFoundException : CadastroLivrosException
    {
        public CadastroLivrosNotFoundException()
        {
            HttpStatusCode = HttpStatusCode.NotFound;
        }

        public CadastroLivrosNotFoundException(string message) : base(message)
        {
            HttpStatusCode = HttpStatusCode.NotFound;
        }
    }
}
