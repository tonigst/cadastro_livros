using System.Net;

namespace CadastroLivros.Exceptions
{
    public class CadastroLivrosBadRequestException : CadastroLivrosException
    {
        public CadastroLivrosBadRequestException()
        {
            HttpStatusCode = HttpStatusCode.BadRequest;
        }

        public CadastroLivrosBadRequestException(string message) : base(message)
        {
            HttpStatusCode = HttpStatusCode.BadRequest;
        }
    }
}
