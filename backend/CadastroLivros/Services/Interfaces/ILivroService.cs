using CadastroLivros.Data.Models;

namespace CadastroLivros.Services.Interfaces
{
    public interface ILivroService
    {
        Task<LivroDTO?> Read(int codL);
        Task<LivroDTO> Insert(LivroDTO livro);
        Task Update(LivroDTO livro);
        Task Delete(int codL);
        Task<IEnumerable<LivroDTO>> ReadPage(int page = 1, int pageSize = 40);
    }
}
