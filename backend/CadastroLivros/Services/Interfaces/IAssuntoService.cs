using CadastroLivros.Data.Models;

namespace CadastroLivros.Services.Interfaces
{
    public interface IAssuntoService
    {
        Task<AssuntoDTO?> Read(int codAs);
        Task<AssuntoDTO> Insert(AssuntoDTO assunto);
        Task Update(AssuntoDTO assunto);
        Task Delete(int codAs);
        Task<IEnumerable<AssuntoDTO>> ReadPage(int page = 0, int pageSize = 40);
    }
}
