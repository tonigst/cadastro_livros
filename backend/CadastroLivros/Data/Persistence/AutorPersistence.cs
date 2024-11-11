using CadastroLivros.Data.Entities;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Exceptions;
using System.Data.Common;
using System.Reflection.PortableExecutable;

namespace CadastroLivros.Data.Persistence
{
    public class AutorPersistence : IAutorPersistence
    {
        private readonly IBasicPersistence _basicPersistence;

        public AutorPersistence(IBasicPersistence basicPersistence)
        {
            _basicPersistence = basicPersistence;
        }

        public async Task<Autor?> Read(long codAu)
        {
            Autor? autor = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
            {
                if (await reader.ReadAsync())
                {
                    var autor = new Autor()
                    {
                        CodAu = codAu,
                        Nome = reader.GetString(0),
                    };
                }
            },
            "SELECT Nome FROM Autor WHERE CodAu = $CodAu",
            ("$CodAu", codAu));

            return autor;
        }

        public async Task<bool> Exists(long codAu)
        {
            var result = await _basicPersistence.ExecuteScalarAsync<long?>(
            "SELECT Count(CodAu) FROM Autor WHERE CodAu = $CodAu",
            ("$CodAu", codAu));

            return result == 1;
        }

        public async Task<Autor> Insert(Autor autor)
        {
            var newId = await _basicPersistence.ExecuteScalarAsync<long?>(
                @"INSERT INTO Autor (Nome) VALUES ($Nome); 
                SELECT last_insert_rowid();",
                ("$Nome", autor.Nome)
            );

            if (newId == null)
                throw new CadastroLivrosDataBaseException("Erro ao inserir autor");

            autor.CodAu = newId.Value;
            return autor;
        }

        public async Task<bool> Update(Autor autor)
        {
            var result = await _basicPersistence.ExecuteNonQueryAsync(
                "UPDATE Autor SET Nome = $Nome WHERE CodAu = $CodAu",
                ("$CodAu", autor.CodAu),
                ("$Nome", autor.Nome)
            );

            return result == 1;
        }

        public async Task<bool> Delete(long codAu)
        {
            var result = await _basicPersistence.ExecuteNonQueryAsync(
                "DELETE FROM Autor WHERE CodAu = $CodAu",
                ("$CodAu", codAu)
            );

            await _basicPersistence.ExecuteNonQueryAsync(
                "DELETE FROM Livro_Autor WHERE Autor_CodAu = $CodAu",
                ("$CodAu", codAu)
            );

            return result == 1;
        }

        private static async Task<List<Autor>> ReadListFromDataAdapter(DbDataReader reader)
        {
            var list = new List<Autor>();

            while (await reader.ReadAsync())
            {
                var autor = new Autor()
                {
                    CodAu = reader.GetInt32(0),
                    Nome = reader.GetString(1)
                };
                list.Add(autor);
            }

            return list;
        }

        public async Task<IEnumerable<Autor>> ReadList(int offset = 0, int limit = 40)
        {
            List<Autor> list = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
            {
                list = await ReadListFromDataAdapter(reader);
            },
            "SELECT CodAu, Nome FROM Autor LIMIT $Limit OFFSET $Offset",
            ("$Limit", limit),
            ("$Offset", offset));

            return list;
        }

        public async Task<IEnumerable<Autor>> ReadListFromLivro(long codL)
        {
            List<Autor> list = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
                list = await ReadListFromDataAdapter(reader),

            @"SELECT CodAu, Nome FROM Autor AU 
                INNER JOIN Livro_Autor LA ON LA.Autor_CodAu = AU.CodAu AND LA.Livro_CodL = $CodL",
            ("$CodL", codL));

            return list;
        }

        public async Task<bool> UpdateRelationshipWithLivro(long codL, IEnumerable<Autor> autores)
        {
            await _basicPersistence.ExecuteNonQueryAsync(
                   "DELETE FROM Livro_Autor WHERE Livro_CodL = $CodL",
                   ("$CodL", codL)
               );

            int result = 0;
            foreach (var autor in autores)
            {
                if (autor.CodAu <= 0)    // criar novo autor antes de associar
                    await Insert(autor); // vai atualizar ID

                result += await _basicPersistence.ExecuteNonQueryAsync(
                    "INSERT INTO Livro_Autor (Livro_CodL, Autor_CodAu) VALUES ($CodL, $CodAu)",
                    ("$CodL", codL),
                    ("$CodAu", autor.CodAu)
                );
            }

            return result == autores.Count();
        }
    }
}