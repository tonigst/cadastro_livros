using CadastroLivros.Data.Models;

namespace CadastroLivros.Services.Interfaces
{
    public interface IAssuntoService
    {
        Task<AssuntoDTO?> Read(long codAs);
        Task<AssuntoDTO> Insert(AssuntoDTO assunto);
        Task Update(AssuntoDTO assunto);
        Task Delete(long codAs);
        Task<IEnumerable<AssuntoDTO>> ReadPage(int page = 0, int pageSize = 40);
    }
}
