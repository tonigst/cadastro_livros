using CadastroLivros.Data.Entities;

namespace CadastroLivros.Data.Persistence.Interfaces
{
    public interface ILivroPersistence
    {
        Task<Livro?> Read(long codL);
        Task<bool> Exists(long codL);
        Task<Livro> Insert(Livro livro); // vai atualizar entidade com o novo ID adicionado
        Task<bool> Update(Livro livro);
        Task<bool> Delete(long codL); // deleta também relacionamentos Livro_Autor, Livro_Assunto e Precos
        Task<IEnumerable<Livro>> ReadList(int offset = 0, int limit = 40);
    }
}
