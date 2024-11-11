using CadastroLivros.Data.Entities;

namespace CadastroLivros.Data.Persistence.Interfaces
{
    public interface IFormaCompraPersistence
    {
        Task<FormaCompra?> Read(long codFC);
        Task<bool> Exists(long codFC);
        Task<FormaCompra> Insert(FormaCompra formaCompra); // vai atualizar entidade com o novo ID adicionado
        Task<bool> Update(FormaCompra formaCompra);
        Task<bool> Delete(long codFC);
        Task<IEnumerable<FormaCompra>> ReadList(int offset = 0, int limit = 40);
    }
}
