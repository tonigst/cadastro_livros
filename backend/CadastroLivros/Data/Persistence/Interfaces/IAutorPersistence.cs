using CadastroLivros.Data.Entities;

namespace CadastroLivros.Data.Persistence.Interfaces
{
    public interface IAutorPersistence
    {
        Task<Autor?> Read(int codAu);
        Task<Autor> Insert(Autor autor); // vai atualizar entidade com o novo ID adicionado
        Task<bool> Update(Autor autor);
        Task<bool> Delete(int codAu); // deleta também relacionamentos Livro_Autor
        Task<IEnumerable<Autor>> ReadList(int offset = 0, int limit = 40);
        Task<IEnumerable<Autor>> ReadListFromLivro(int codL);
        Task<bool> InsertOrUpdateFromLivro(int codL, IEnumerable<Autor> autores);
    }
}
