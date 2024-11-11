using CadastroLivros.Data.Entities;

namespace CadastroLivros.Data.Persistence.Interfaces
{
    public interface IAssuntoPersistence
    {
        Task<Assunto?> Read(long codAs);
        Task<bool> Exists(long codAs);
        Task<Assunto> Insert(Assunto assunto); // vai atualizar entidade com o novo ID adicionado
        Task<bool> Update(Assunto assunto);
        Task<bool> Delete(long codAs); // deleta também relacionamentos Livro_Assunto
        Task<IEnumerable<Assunto>> ReadList(int offset = 0, int limit = 40);
        Task<IEnumerable<Assunto>> ReadListFromLivro(long codL);
        Task<bool> UpdateRelationshipWithLivro(long codL, IEnumerable<Assunto> assuntos);
    }
}
