using CadastroLivros.Data.Entities;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Exceptions;
using System.Data;
using System.Data.Common;
using System.Text;

namespace CadastroLivros.Data.Persistence
{
    public class FormaCompraPersistence : IFormaCompraPersistence
    {
        private readonly IBasicPersistence _basicPersistence;

        public FormaCompraPersistence(IBasicPersistence basicPersistence)
        {
            _basicPersistence = basicPersistence;
        }

        public async Task<FormaCompra?> Read(int codFC)
        {
            FormaCompra? formaCompra = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
            {
                if (await reader.ReadAsync())
                {
                    var autor = new FormaCompra()
                    {
                        CodFC = codFC,
                        Descricao = reader.GetString(0),
                    };
                }
            },
            "SELECT Descricao FROM FormaCompra WHERE CodFC = $CodFC",
            ("$CodFC", codFC));

            return formaCompra;
        }

        public async Task<FormaCompra> Insert(FormaCompra formaCompra)
        {
            var newId = await _basicPersistence.ExecuteScalarAsync<int?>(
                @"INSERT INTO FormaCompra (Descricao) VALUES ($Descricao); 
                SELECT last_insert_rowid();",
                ("$Descricao", formaCompra.Descricao)
            );

            if (newId == null)
                throw new CadastroLivrosDataBaseException("Erro ao inserir FormaCompra");

            formaCompra.CodFC = newId.Value;
            return formaCompra;
        }

        public async Task<bool> Update(FormaCompra formaCompra)
        {
            var result = await _basicPersistence.ExecuteNonQueryAsync(
                "UPDATE FormaCompra SET Descricao = $Descricao WHERE CodFC = $CodFC",
                ("$CodFC", formaCompra.CodFC),
                ("$Descricao", formaCompra.Descricao)
            );

            return result == 1;
        }

        public async Task<bool> Delete(int codFC)
        {
            var result = await _basicPersistence.ExecuteNonQueryAsync(
                "DELETE FROM FormaCompra WHERE CodFC = $CodFC",
                ("$CodFC", codFC)
            );

            await _basicPersistence.ExecuteNonQueryAsync(
                "DELETE FROM Preco WHERE CodFC = $CodFC",
                ("$CodFC", codFC)
            );

            return result == 1;
        }


        private static async Task<List<FormaCompra>> ReadListFromDataAdapter(DbDataReader reader)
        {
            var list = new List<FormaCompra>();

            while (await reader.ReadAsync())
            {
                var formaCompra = new FormaCompra()
                {
                    CodFC = reader.GetInt32(0),
                    Descricao = reader.GetString(1)
                };
                list.Add(formaCompra);
            }

            return list;
        }

        public async Task<IEnumerable<FormaCompra>> ReadList(int offset = 0, int limit = 40)
        {
            List<FormaCompra> list = null;

            await _basicPersistence.ExecuteReaderAsync(async (reader) =>
            {
                list = await ReadListFromDataAdapter(reader);
            },
            "SELECT CodFC, Descricao FROM FormaCompra LIMIT $Limit OFFSET $Offset",
            ("$Limit", limit),
            ("$Offset", offset));

            return list;
        }
    }
}
