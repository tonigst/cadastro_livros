using CadastroLivros.Data.Models;

namespace CadastroLivros.Services.Interfaces
{
    public interface IAutorService
    {
        Task<AutorDTO?> Read(int codAu);
        Task<AutorDTO> Insert(AutorDTO autor);
        Task Update(AutorDTO autor);
        Task Delete(int codAu);
        Task<IEnumerable<AutorDTO>> ReadPage(int page = 0, int pageSize = 40);
    }
}
