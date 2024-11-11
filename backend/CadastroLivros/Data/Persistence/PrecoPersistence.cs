using CadastroLivros.Data.Entities;
using CadastroLivros.Data.Mapping;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Exceptions;
using System.Data.Common;
using System.Reflection.PortableExecutable;

namespace CadastroLivros.Data.Persistence
{
    public class PrecoPersistence : IPrecoPersistence
    {
        private readonly IBasicPersistence _basicPersistence;
        private readonly IFormaCompraPersistence _formaCompraPersistence;

        private const string _baseSelectQuery = 
            @"SELECT P.CodP, P.CodL, P.CodFC, FC.Descricao, P.Valor 
              FROM Preco P 
              LEFT JOIN FormaCompra FC on FC.CodFC = P.CodFC ";


        public PrecoPersistence(IBasicPersistence basicPersistence, IFormaCompraPersistence formaCompraPersistence)
        {
            _basicPersistence = basicPersistence;
            _formaCompraPersistence = formaCompraPersistence;
        }

        public async Task<Preco?> Read(long codP)
        {
            Preco? preco = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
            {
                if (await reader.ReadAsync())
                {
                    var preco = new Preco()
                    {
                        CodP = reader.GetInt32(0),
                        CodL = reader.GetInt32(1),
                        CodFC = reader.GetInt32(2),
                        FormaCompra = reader.GetString(3),
                        Valor = reader.GetDecimal(4),
                    };
                }
            },
            _baseSelectQuery + " WHERE CodP = $CodP",
            ("$CodP", codP));

            return preco;
        }

        public async Task<bool> Exists(long codP)
        {
            var result = await _basicPersistence.ExecuteScalarAsync<long?>(
            "SELECT Count(CodP) FROM Preco WHERE CodP = $CodP",
            ("$CodP", codP));

            return result == 1;
        }

        public async Task<Preco> Insert(Preco preco)
        {
            var formaCompra = PrecoMapping.ToFormaCompra(preco);

            if (preco.CodFC <= 0 && preco.FormaCompra == null)
                throw new CadastroLivrosBadRequestException("Erro ao inserir preço. FormaCompra ou CodFC precisam ser informados.");

            if (preco.CodFC <= 0)                                         // criar nova formaCompra antes inserir preco
                await _formaCompraPersistence.Insert(formaCompra);        // vai atualizar ID
            else if (!await _formaCompraPersistence.Exists(preco.CodFC))  // verificar se formaCompra CodFC realmente existe
                throw new CadastroLivrosDataBaseException($"Erro ao inserir preço. FormaCompra {preco.CodFC} não encontrada no banco de dados.");

            preco.CodFC = formaCompra.CodFC;

            var newId = await _basicPersistence.ExecuteScalarAsync<long?>(
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

        public async Task<bool> Delete(long codP)
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
                    FormaCompra = reader.GetString(3),
                    Valor = reader.GetDecimal(4),
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
            _baseSelectQuery + " LIMIT $Limit OFFSET $Offset",
            ("$Limit", limit),
            ("$Offset", offset));

            return list;
        }

        public async Task<IEnumerable<Preco>> ReadListFromLivro(long codL)
        {
            List<Preco> list = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
                list = await ReadListFromDataAdapter(reader),

            _baseSelectQuery + " WHERE CodL = $CodL",
            ("$CodL", codL));

            return list;
        }

        public async Task<IEnumerable<Preco>> UpdateRelationshipWithLivro(long codL, IEnumerable<Preco> precos)
        {
            if (precos.Any(a => a.CodL != codL))
                throw new CadastroLivrosBadRequestException("Dados inválidos na inserção/atualização de preços do livro.");

            // deletar precos do livro
            await _basicPersistence.ExecuteNonQueryAsync(
                "DELETE FROM Preco WHERE CodL = $CodL",
                ("$CodL", codL)
            );

            // (re)criar precos do livro 
            foreach (var preco in precos)
                await Insert(preco);

            return precos;
        }
    }
}