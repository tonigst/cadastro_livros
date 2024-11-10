using CadastroLivros.Service;
using System.Data.Common;

namespace CadastroLivros.Data.Persistence.Interfaces
{
    public interface IBasicPersistence
    {
        Task<int> ExecuteNonQueryAsync(string commandText, params (string Name, object Value)[] parameters);
        Task<T?> ExecuteScalarAsync<T>(string commandText, params (string Name, object Value)[] parameters);
        Task ExecuteReaderAsync(Action<DbDataReader> action, string commandText, params (string Name, object Value)[] parameters);
    }

}
