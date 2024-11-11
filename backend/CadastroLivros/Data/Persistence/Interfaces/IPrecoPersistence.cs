using CadastroLivros.Data.Entities;

namespace CadastroLivros.Data.Persistence.Interfaces
{
    public interface IPrecoPersistence
    {
        Task<Preco?> Read(long codP);
        Task<bool> Exists(long codP);
        Task<Preco> Insert(Preco preco); // vai atualizar entidade com o novo ID adicionado
        Task<bool> Update(Preco preco);
        Task<bool> Delete(long codP);
        Task<IEnumerable<Preco>> ReadList(int offset = 0, int limit = 40);
        Task<IEnumerable<Preco>> ReadListFromLivro(long codL);
        Task<IEnumerable<Preco>> UpdateRelationshipWithLivro(long codL, IEnumerable<Preco> precos); // vai atualizar entidades com os novos IDs adicionados
    }
}
