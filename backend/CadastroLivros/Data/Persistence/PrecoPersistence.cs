using CadastroLivros.Data.Entities;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Exceptions;
using System.Data.Common;
using System.Reflection.PortableExecutable;

namespace CadastroLivros.Data.Persistence
{
    public class PrecoPersistence : IPrecoPersistence
    {
        private readonly IBasicPersistence _basicPersistence;

        public PrecoPersistence(IBasicPersistence basicPersistence)
        {
            _basicPersistence = basicPersistence;
        }

        public async Task<Preco?> Read(int codP)
        {
            Preco? preco = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
            {
                if (await reader.ReadAsync())
                {
                    var preco = new Preco()
                    {
                        CodP = codP,
                        CodL = reader.GetInt32(0),
                        CodFC = reader.GetInt32(1),
                        Valor = reader.GetDecimal(2),
                    };
                }
            },
            "SELECT CodL, CodFC, Valor FROM Preco WHERE CodP = $CodP",
            ("$CodP", codP));

            return preco;
        }

        public async Task<Preco> Insert(Preco preco)
        {
            var newId = await _basicPersistence.ExecuteScalarAsync<int?>(
                @"INSERT INTO Preco (CodL, CodFC, Valor) VALUES ($CodL, $CodFC, $Valor); 
                SELECT last_insert_rowid();",
                ("$CodL", preco.CodL),
                ("$CodFC", preco.CodFC),
                ("$Valor", preco.Valor)
            );

            if (newId == null)
                throw new CadastroLivrosDataBaseException("Erro ao inserir preco");

            preco.CodP = newId.Value;
            return preco;
        }

        public async Task<bool> Update(Preco preco)
        {
            var result = await _basicPersistence.ExecuteNonQueryAsync(
                "UPDATE Preco SET CodL = $CodL, CodFC = $CodFC, Valor = $Valor WHERE CodP = $CodP",
                ("$CodP", preco.CodP),
                ("$CodL", preco.CodL),
                ("$CodFC", preco.CodFC),
                ("$Valor", preco.Valor)
            );

            return result == 1;
        }

        public async Task<bool> Delete(int codP)
        {
            var result = await _basicPersistence.ExecuteNonQueryAsync(
                "DELETE FROM Preco WHERE CodP = $CodP",
                ("$CodP", codP)
            );

            return result == 1;
        }

        private static async Task<List<Preco>> ReadListFromDataAdapter(DbDataReader reader)
        {
            var list = new List<Preco>();

            while (await reader.ReadAsync())
            {
                var preco = new Preco()
                {
                    CodP = reader.GetInt32(0),
                    CodL = reader.GetInt32(1),
                    CodFC = reader.GetInt32(2),
                    Valor = reader.GetDecimal(3),
                };
                list.Add(preco);
            }

            return list;
        }

        public async Task<IEnumerable<Preco>> ReadList(int offset = 0, int limit = 40)
        {
            List<Preco> list = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
            {
                list = await ReadListFromDataAdapter(reader);
            },
            "SELECT CodP, CodL, CodFC, Valor FROM Preco LIMIT $Limit OFFSET $Offset",
            ("$Limit", limit),
            ("$Offset", offset));

            return list;
        }

        public async Task<IEnumerable<Preco>> ReadListFromLivro(int codL)
        {
            List<Preco> list = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
                list = await ReadListFromDataAdapter(reader),

            @"SELECT CodP, CodL, CodFC, Valor FROM Preco WHERE CodL = $CodL",
            ("$CodL", codL));

            return list;
        }

        public async Task<IEnumerable<Preco>> InsertOrUpdateFromLivro(int codL, IEnumerable<Preco> precos)
        {
            if (precos.Any(a => a.CodL != codL))
                throw new CadastroLivrosBadRequestException("Dados inválidos na inserção/atualização de preços do livro.");

            await _basicPersistence.ExecuteNonQueryAsync(
                   "DELETE FROM Preco WHERE Livro_CodL = $CodL",
                   ("$CodL", codL)
               );

            int result = 0;
            foreach (var preco in precos)
            {
                var newId = await _basicPersistence.ExecuteScalarAsync<int?>(
                    @"INSERT INTO Preco (CodL, CodFC, Valor) VALUES ($CodL, $CodFC, $Valor); 
                    SELECT last_insert_rowid();",
                    ("$CodL", preco.CodL),
                    ("$CodFC", preco.CodFC),
                    ("$Valor", preco.Valor)
                );

                if (newId == null)
                    throw new CadastroLivrosDataBaseException("Erro ao inserir preco");

                preco.CodP = newId.Value;
            }

            return precos;
        }
    }
}