using CadastroLivros.Data.Entities;

namespace CadastroLivros.Data.Persistence.Interfaces
{
    public interface IPrecoPersistence
    {
        Task<Preco?> Read(int codP);
        Task<Preco> Insert(Preco preco); // vai atualizar entidade com o novo ID adicionado
        Task<bool> Update(Preco preco);
        Task<bool> Delete(int codP);
        Task<IEnumerable<Preco>> ReadList(int offset = 0, int limit = 40);
        Task<IEnumerable<Preco>> ReadListFromLivro(int codL);
        Task<IEnumerable<Preco>> InsertOrUpdateFromLivro(int codL, IEnumerable<Preco> precos); // vai atualizar entidades com os novos IDs adicionados
    }
}
