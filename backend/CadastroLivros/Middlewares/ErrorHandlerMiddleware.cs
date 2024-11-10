using CadastroLivros.Exceptions;
using System.Net;

namespace CadastroLivros.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {                
                var httpStatusCode = (int)((ex as CadastroLivrosException)?.HttpStatusCode ?? HttpStatusCode.InternalServerError);

                var errorDetails = new
                {
                    ex.Message,
                    Type = ex.GetType().Name,
                    Inner = ex.InnerException?.Message ?? string.Empty,
                    HttpStatusCode = httpStatusCode,
                };
                context.Response.StatusCode = httpStatusCode;

                await context.Response.WriteAsJsonAsync(errorDetails);
            }
        }
    }
}
