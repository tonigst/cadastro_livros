using CadastroLivros.Data.Models;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Services.Interfaces;

namespace CadastroLivros.Service
{
    public class AssuntoService : IAssuntoService
    {
        private readonly IAssuntoPersistence _assuntoPersistence;

        public AssuntoService(IAssuntoPersistence assuntoPersistence)
        {
            _assuntoPersistence = assuntoPersistence;
        }

        public async Task<AssuntoDTO?> Read(int codAs)
        {
            throw new NotImplementedException();
        }

        public async Task<AssuntoDTO> Insert(AssuntoDTO assunto)
        {
            throw new NotImplementedException();
        }

        public async Task Update(AssuntoDTO assunto)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int codAs)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AssuntoDTO>> ReadPage(int page = 0, int pageSize = 40)
        {
            throw new NotImplementedException();
        }
    }
}