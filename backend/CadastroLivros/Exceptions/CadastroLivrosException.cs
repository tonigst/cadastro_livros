using System.Net;

namespace CadastroLivros.Exceptions
{
    public class CadastroLivrosException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.InternalServerError;

        public CadastroLivrosException() { }

        public CadastroLivrosException(string message) : base(message) { }

        public CadastroLivrosException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
