using CadastroLivros.Data.Entities;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Exceptions;
using System.Data;
using System.Data.Common;
using System.Text;

namespace CadastroLivros.Data.Persistence
{
    public class AssuntoPersistence : IAssuntoPersistence
    {
        private readonly IBasicPersistence _basicPersistence;

        public AssuntoPersistence(IBasicPersistence basicPersistence)
        {
            _basicPersistence = basicPersistence;
        }

        public async Task<Assunto?> Read(long codAs)
        {
            Assunto? assunto = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
            {
                if (await reader.ReadAsync())
                {
                    var autor = new Assunto()
                    {
                        CodAs = codAs,
                        Descricao = reader.GetString(0),
                    };
                }
            },
            "SELECT Descricao FROM Assunto WHERE CodAs = $CodAs",
            ("$CodAs", codAs));

            return assunto;
        }

        public async Task<bool> Exists(long codAs)
        {
            var result = await _basicPersistence.ExecuteScalarAsync<long?>(
            "SELECT Count(CodAs) FROM Assunto WHERE CodAs = $CodAs",
            ("$CodAs", codAs));

            return result == 1;
        }

        public async Task<Assunto> Insert(Assunto assunto)
        {
            var newId = await _basicPersistence.ExecuteScalarAsync<long?>(
                @"INSERT INTO Assunto (Descricao) VALUES ($Descricao); 
                SELECT last_insert_rowid();",
                ("$Descricao", assunto.Descricao)
            );

            if (newId == null)
                throw new CadastroLivrosDataBaseException("Erro ao inserir assunto");

            assunto.CodAs = newId.Value;
            return assunto;
        }

        public async Task<bool> Update(Assunto assunto)
        {
            var result = await _basicPersistence.ExecuteNonQueryAsync(
                "UPDATE Assunto SET Descricao = $Descricao WHERE CodAs = $CodAs",
                ("$CodAs", assunto.CodAs),
                ("$Descricao", assunto.Descricao)
            );

            return result == 1;
        }

        public async Task<bool> Delete(long codAs)
        {
            var result = await _basicPersistence.ExecuteNonQueryAsync(
                "DELETE FROM Assunto WHERE CodAs = $CodAs",
                ("$CodAs", codAs)
            );

            await _basicPersistence.ExecuteNonQueryAsync(
                "DELETE FROM Livro_Assunto WHERE Assunto_CodAs = $CodAs",
                ("$CodAs", codAs)
            );

            return result == 1;
        }


        private static async Task<List<Assunto>> ReadListFromDataAdapter(DbDataReader reader)
        {
            var list = new List<Assunto>();

            while (await reader.ReadAsync())
            {
                var assunto = new Assunto()
                {
                    CodAs = reader.GetInt32(0),
                    Descricao = reader.GetString(1)
                };
                list.Add(assunto);
            }

            return list;
        }

        public async Task<IEnumerable<Assunto>> ReadList(int offset = 0, int limit = 40)
        {
            List<Assunto> list = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
            {
                list = await ReadListFromDataAdapter(reader);
            },
            "SELECT CodAs, Descricao FROM Assunto LIMIT $Limit OFFSET $Offset",
            ("$Limit", limit),
            ("$Offset", offset));

            return list;
        }

        public async Task<IEnumerable<Assunto>> ReadListFromLivro(long codL)
        {
            List<Assunto> list = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
                list = await ReadListFromDataAdapter(reader),

            @"SELECT CodAs, Descricao FROM Assunto AT 
                INNER JOIN Livro_Assunto LA on LA.Assunto_CodAs = AT.CodAs and LA.Livro_CodL = $CodL",
            ("$CodL", codL));

            return list;
        }

        public async Task<bool> UpdateRelationshipWithLivro(long codL, IEnumerable<Assunto> assuntos)
        {
            await _basicPersistence.ExecuteNonQueryAsync(
                   "DELETE FROM Livro_Assunto WHERE Livro_CodL = $CodL",
                   ("$CodL", codL)
               );

            int result = 0;
            foreach (var assunto in assuntos)
            {
                if (assunto.CodAs <= 0)    // criar novo assunto antes de associar
                    await Insert(assunto); // vai atualizar ID

                result += await _basicPersistence.ExecuteNonQueryAsync(
                    "INSERT INTO Livro_Assunto (Livro_CodL, Assunto_CodAs) VALUES ($CodL, $CodAs)",
                    ("$CodL", codL),
                    ("$CodAs", assunto.CodAs)
                );
            }

            return result == assuntos.Count();
        }
    }
}
