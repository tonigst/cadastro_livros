using CadastroLivros.Data.Entities;

namespace CadastroLivros.Data.Persistence.Interfaces
{
    public interface IAssuntoPersistence
    {
        Task<Assunto?> Read(int codAs);
        Task<Assunto> Insert(Assunto assunto); // vai atualizar entidade com o novo ID adicionado
        Task<bool> Update(Assunto assunto);
        Task<bool> Delete(int codAs); // deleta também relacionamentos Livro_Assunto
        Task<IEnumerable<Assunto>> ReadList(int offset = 0, int limit = 40);
        Task<IEnumerable<Assunto>> ReadListFromLivro(int codL);
        Task<bool> InsertOrUpdateFromLivro(int codL, IEnumerable<Assunto> assuntos);
    }
}
