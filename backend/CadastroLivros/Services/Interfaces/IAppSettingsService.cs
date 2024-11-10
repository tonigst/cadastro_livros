using CadastroLivros.Data.Models;

namespace CadastroLivros.Services.Interfaces
{
    public interface IAppSettingsService
    {
        string GetConnectionString();
        string GetCreateDBFile();
    }
}
