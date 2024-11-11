using CadastroLivros.Data.Entities;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Exceptions;
using System.Data;
using System.Data.Common;

namespace CadastroLivros.Data.Persistence
{
    public class LivroPersistence : ILivroPersistence
    {
        private readonly IBasicPersistence _basicPersistence;

        public LivroPersistence(IBasicPersistence basicPersistence)
        {
            _basicPersistence = basicPersistence;
        }

        public async Task<Livro?> Read(long codL)
        {
            Livro? livro = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
            {
                if (await reader.ReadAsync())
                {
                    livro = new Livro()
                    {
                        CodL = codL,
                        Titulo = reader.GetString(0),
                        Editora = reader.GetString(1),
                        Edicao = reader.GetInt32(2),
                        AnoPublicacao = reader.GetString(3),
                    };
                }
            },
            "SELECT Titulo, Editora, Edicao, AnoPublicacao FROM Livro WHERE CodL = $CodL",
            ("$CodL", codL));

            return livro;
        }

        public async Task<bool> Exists(long codL)
        {
            var result = await _basicPersistence.ExecuteScalarAsync<long?>(
            "SELECT Count(CodL) FROM Livro WHERE CodL = $CodL",
            ("$CodL", codL));

            return result == 1;
        }

        public async Task<Livro> Insert(Livro livro) 
        {
            var newId = await _basicPersistence.ExecuteScalarAsync<long?>(
                @"INSERT INTO Livro (Titulo, Editora, Edicao, AnoPublicacao) 
                VALUES ($Titulo, $Editora, $Edicao, $AnoPublicacao); 
                SELECT last_insert_rowid();",
                ("$Titulo", livro.Titulo),
                ("$Editora", livro.Editora),
                ("$Edicao", livro.Edicao),
                ("$AnoPublicacao", livro.AnoPublicacao)
            );

            if (newId == null)
                throw new CadastroLivrosDataBaseException("Erro ao inserir livro");

            livro.CodL = newId.Value;
            return livro;
        }

        public async Task<bool> Update(Livro livro)
        {
            var result = await _basicPersistence.ExecuteNonQueryAsync(
                "UPDATE Livro SET Titulo = $Titulo, Editora = $Editora, Edicao = $Edicao, AnoPublicacao = $AnoPublicacao WHERE CodL = $CodL",
                ("$CodL", livro.CodL),
                ("$Titulo", livro.Titulo),
                ("$Editora", livro.Editora),
                ("$Edicao", livro.Edicao),
                ("$AnoPublicacao", livro.AnoPublicacao)
            );

            return result == 1;
        }

        public async Task<bool> Delete(long codL)
        {
            var result = await _basicPersistence.ExecuteNonQueryAsync(
                "DELETE FROM Livro WHERE CodL = $CodL",
                ("$CodL", codL)
            );

            await _basicPersistence.ExecuteNonQueryAsync(
                "DELETE FROM Livro_Autor WHERE Livro_CodL = $CodL",
                ("$CodL", codL)
            );

            await _basicPersistence.ExecuteNonQueryAsync(
                "DELETE FROM Livro_Assunto WHERE Livro_CodL = $CodL",
                ("$CodL", codL)
            );

            await _basicPersistence.ExecuteNonQueryAsync(
                "DELETE FROM Preco WHERE CodL = $CodL",
                ("$CodL", codL)
            );

            return result == 1;
        }

        private static async Task<List<Livro>> ReadListFromDataAdapter(DbDataReader reader)
        {
            var list = new List<Livro>();

            while (await reader.ReadAsync())
            {
                var livro = new Livro()
                {
                    CodL = reader.GetInt32(0),
                    Titulo = reader.GetString(1),
                    Editora = reader.GetString(2),
                    Edicao = reader.GetInt32(3),
                    AnoPublicacao = reader.GetString(4),
                };
                list.Add(livro);
            }

            return list;
        }

        public async Task<IEnumerable<Livro>> ReadList(int offset = 0, int limit = 40)
        {
            List<Livro> list = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
            {
                list = await ReadListFromDataAdapter(reader);
            },
            "SELECT CodL, Titulo, Editora, Edicao, AnoPublicacao FROM Livro LIMIT $Limit OFFSET $Offset",
            ("$Limit", limit),
            ("$Offset", offset));

            return list;
        }

    }
}