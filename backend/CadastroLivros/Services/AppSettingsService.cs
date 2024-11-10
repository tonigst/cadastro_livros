using CadastroLivros.Exceptions;
using CadastroLivros.Services.Interfaces;

namespace CadastroLivros.Service
{
    public class AppSettingsService : IAppSettingsService
    {
        private readonly IConfiguration _configuration;

        public AppSettingsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return _configuration.GetValue<string>("AppSettings:ConnectionString")
                ?? throw new CadastroLivrosException("ConnectionString não encontrada.");
        }
        public string GetCreateDBFile()
        {
            return _configuration.GetValue<string>("AppSettings:CreateDbFile")
                ?? throw new CadastroLivrosException("Banco de dados/Script de criação do banco não encontrados.");
        }
    }
}