using CadastroLivros.Data.Entities;

namespace CadastroLivros.Data.Persistence.Interfaces
{
    public interface IFormaCompraPersistence
    {
        Task<FormaCompra?> Read(int codFC);
        Task<FormaCompra> Insert(FormaCompra formaCompra); // vai atualizar entidade com o novo ID adicionado
        Task<bool> Update(FormaCompra formaCompra);
        Task<bool> Delete(int codFC);
        Task<IEnumerable<FormaCompra>> ReadList(int offset = 0, int limit = 40);
    }
}
