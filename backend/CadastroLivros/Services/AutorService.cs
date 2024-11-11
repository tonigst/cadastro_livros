using CadastroLivros.Data.Models;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Services.Interfaces;

namespace CadastroLivros.Service
{
    public class AutorService : IAutorService
    {
        private readonly IAutorPersistence _autorPersistence;

        public AutorService(IAutorPersistence autorPersistence)
        {
            _autorPersistence = autorPersistence;
        }

        public async Task<AutorDTO?> Read(long codAu)
        {
            throw new NotImplementedException();
        }

        public async Task<AutorDTO> Insert(AutorDTO autor)
        {
            throw new NotImplementedException();
        }

        public async Task Update(AutorDTO autor)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(long codAu)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AutorDTO>> ReadPage(int page = 0, int pageSize = 40)
        {
            throw new NotImplementedException();
        }
        
    }
}