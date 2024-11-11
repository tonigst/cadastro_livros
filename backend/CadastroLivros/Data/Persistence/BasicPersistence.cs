using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Exceptions;
using CadastroLivros.Services.Interfaces;
using Microsoft.AspNetCore.Routing.Matching;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;

namespace CadastroLivros.Data.Persistence
{
    public class BasicPersistence : IBasicPersistence, IDisposable
    {
        private readonly IAppSettingsService _appSettingsService;
        private readonly string _connectionString;

        private SQLiteConnection? _connection = null;
        private SQLiteTransaction? _transaction = null;

        public bool HasTransaction
        {
            get => _transaction != null && _connection != null;
        }

        public BasicPersistence(IAppSettingsService appSettingsService)
        {
            _appSettingsService = appSettingsService;
            _connectionString = _appSettingsService.GetConnectionString();

            CreateDatabaseIfNeeded().Wait();
        }

        #region IDisposable 

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BasicPersistence()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
                _transaction = null;
                _connection = null;
            }
        }

        #endregion

        #region Private Methods 

        private async Task ExecuteReaderAsync(SQLiteConnection connection, Action<DbDataReader> action, string commandText, params (string Name, object Value)[] parameters)
        {
            using var command = connection.CreateCommand();
            AddCommandWithParameters(command, commandText, parameters);
            using var reader = await command.ExecuteReaderAsync();
            action(reader);
        }

        private async Task<int> ExecuteNonQueryAsync(SQLiteConnection connection, string commandText, (string Name, object Value)[] parameters)
        {
            using var command = connection.CreateCommand();
            AddCommandWithParameters(command, commandText, parameters);
            return await command.ExecuteNonQueryAsync();
        }

        private async Task<T?> ExecuteScalarAsync<T>(SQLiteConnection connection, string commandText, params (string Name, object Value)[] parameters)
        {
            using var command = connection.CreateCommand();
            AddCommandWithParameters(command, commandText, parameters);
            return (T?)await command.ExecuteScalarAsync();
        }

        private void CheckConnectionState()
        {
            if ((_transaction == null && _connection != null) || (_transaction != null && _connection == null))
                throw new CadastroLivrosException("Estado inválido de conexão com o DB");
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

        #endregion

        public async Task BeginTransaction()
        {
            if (HasTransaction)
                throw new CadastroLivrosException("Não é possível iniciar outra DB transaction antes de terminar a anterior");

            _connection = new SQLiteConnection(_connectionString);
            await _connection.OpenAsync();

            _transaction = _connection.BeginTransaction();
        }

        public async Task Commit()
        {
            if (!HasTransaction)
                throw new CadastroLivrosException("Não é possível fazer commit sem ter criado uma DB transaction");

            await _transaction.CommitAsync();
            _transaction?.Dispose();
            _connection?.Dispose();
            _transaction = null;
            _connection = null;
        }

        public async Task Rollback()
        {
            if (!HasTransaction)
                return;

            await _transaction.RollbackAsync();
            _transaction?.Dispose();
            _connection?.Dispose();
            _transaction = null;
            _connection = null;
        }

        public async Task<int> ExecuteNonQueryAsync(string commandText, params (string Name, object Value)[] parameters)
        {
            CheckConnectionState();

            if (!HasTransaction)
            {
                using var connection = new SQLiteConnection(_connectionString);
                await connection.OpenAsync();

                return await ExecuteNonQueryAsync(connection, commandText, parameters);
            }
            else
            {
                return await ExecuteNonQueryAsync(_connection, commandText, parameters);
            }
        }

        public async Task<T?> ExecuteScalarAsync<T>(string commandText, params (string Name, object Value)[] parameters)
        {
            CheckConnectionState();

            if (!HasTransaction)
            {
                using var connection = new SQLiteConnection(_connectionString);
                await connection.OpenAsync();

                return await ExecuteScalarAsync<T>(connection, commandText, parameters);
            }
            else
            {
                return await ExecuteScalarAsync<T>(_connection, commandText, parameters);
            }
        }

        public async Task ExecuteReaderAsync(Action<DbDataReader> action, string commandText, params (string Name, object Value)[] parameters)
        {
            CheckConnectionState();

            if (!HasTransaction)
            {
                using var connection = new SQLiteConnection(_connectionString);
                await connection.OpenAsync();

                await ExecuteReaderAsync(connection, action, commandText, parameters);
            }
            else
            {
                await ExecuteReaderAsync(_connection, action, commandText, parameters);
            }
        }
    }
}