using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Exceptions;
using CadastroLivros.Services.Interfaces;
using System.Data.Common;
using System.Data.SQLite;

namespace CadastroLivros.Data.Persistence
{
    public class BasicPersistence : IBasicPersistence
    {
        private readonly IAppSettingsService _appSettingsService;
        private readonly string _connectionString;

        public BasicPersistence(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
            _connectionString = _appSettingsService.GetConnectionString();

            CreateDatabaseIfNeeded().Wait();
        }

        private void AddCommandWithParameters(SQLiteCommand command, string commandText, params (string Name, object Value)[] parameters)
        {
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = commandText;
            command.Parameters.Clear();

            if (parameters != null)
            {
                foreach (var (Name, Value) in parameters)
                    command.Parameters.AddWithValue(Name, Value);
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string commandText, params (string Name, object Value)[] parameters)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            AddCommandWithParameters(command, commandText, parameters);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<T?> ExecuteScalarAsync<T>(string commandText, params (string Name, object Value)[] parameters)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            AddCommandWithParameters(command, commandText, parameters);

            return (T?) await command.ExecuteScalarAsync();
        }

        public async Task ExecuteReaderAsync(Action<DbDataReader> action, string commandText, params (string Name, object Value)[] parameters)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            AddCommandWithParameters(command, commandText, parameters);

            using var reader = await command.ExecuteReaderAsync();
            action(reader);
        }

        private async Task CreateDatabaseIfNeeded()
        {
            if (await DatabaseExists())
                return;

            await RunCreateDBScript();

            if (!await DatabaseExists())
                throw new CadastroLivrosDataBaseException("Não foi possível criar o banco de dados.");
        }

        private async Task<bool> DatabaseExists()
        {
            var livroTableName = await ExecuteScalarAsync<string>("SELECT name FROM sqlite_master WHERE type='table' AND name='Livro'");

            // se tabela Livro existe, vamos assumir que o banco existe e está integro
            return (livroTableName != null && livroTableName.Equals("livro", StringComparison.OrdinalIgnoreCase)); 
        }

        private async Task RunCreateDBScript()
        {
            var createDbFile = _appSettingsService.GetCreateDBFile();
            if (!File.Exists(createDbFile))
                throw new CadastroLivrosDataBaseException($"Não foi possível criar o banco de dados. Script de criação do banco não encontrado.");

            using (var reader = new StreamReader(createDbFile))
                await ExecuteNonQueryAsync(reader.ReadToEnd());
        }
    }
}