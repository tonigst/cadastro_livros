using CadastroLivros.Data.Models;

namespace CadastroLivros.Services.Interfaces
{
    public interface IAutorService
    {
        Task<AutorDTO?> Read(long codAu);
        Task<AutorDTO> Insert(AutorDTO autor);
        Task Update(AutorDTO autor);
        Task Delete(long codAu);
        Task<IEnumerable<AutorDTO>> ReadPage(int page = 0, int pageSize = 40);
    }
}
