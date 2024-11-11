using CadastroLivros.Data.Entities;

namespace CadastroLivros.Data.Persistence.Interfaces
{
    public interface IAutorPersistence
    {
        Task<Autor?> Read(long codAu);
        Task<bool> Exists(long codAu);
        Task<Autor> Insert(Autor autor); // vai atualizar entidade com o novo ID adicionado
        Task<bool> Update(Autor autor);
        Task<bool> Delete(long codAu); // deleta também relacionamentos Livro_Autor
        Task<IEnumerable<Autor>> ReadList(int offset = 0, int limit = 40);
        Task<IEnumerable<Autor>> ReadListFromLivro(long codL);
        Task<bool> UpdateRelationshipWithLivro(long codL, IEnumerable<Autor> autores);
    }
}
