using CadastroLivros.Data.Mapping;
using CadastroLivros.Data.Models;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Exceptions;
using CadastroLivros.Services.Interfaces;

namespace CadastroLivros.Service
{
    public class LivroService : ILivroService
    {
        private readonly IBasicPersistence _basicPersistence;
        private readonly ILivroPersistence _livroPersistence;
        private readonly IAutorPersistence _autorPersistence;
        private readonly IAssuntoPersistence _assuntoPersistence;
        private readonly IPrecoPersistence _precoPersistence;

        public LivroService(IBasicPersistence basicPersistence, ILivroPersistence livroPersistence, IAutorPersistence autorPersistence, IAssuntoPersistence assuntoPersistence, IPrecoPersistence precoPersistence)
        {
            _basicPersistence = basicPersistence;
            _livroPersistence = livroPersistence;
            _autorPersistence = autorPersistence;
            _assuntoPersistence = assuntoPersistence;
            _precoPersistence = precoPersistence;
        }

        #region Private Methods 

        private async Task ReadRelationships(LivroDTO livroDTO)
        {
            var assuntos = await _assuntoPersistence.ReadListFromLivro(livroDTO.CodL.Value);
            livroDTO.Assuntos = AssuntoMapping.ToDTOList(assuntos);

            var autores = await _autorPersistence.ReadListFromLivro(livroDTO.CodL.Value);
            livroDTO.Autores = AutorMapping.ToDTOList(autores);

            var precos = await _precoPersistence.ReadListFromLivro(livroDTO.CodL.Value);
            livroDTO.Precos = PrecoMapping.ToDTOList(precos);
        }

        private async Task UpdateRelationships(LivroDTO livroDTO)
        {
            if (livroDTO.Assuntos != null && livroDTO.Assuntos.Any())
            {
                var assuntos = AssuntoMapping.FromDTOList(livroDTO.Assuntos);
                if (!await _assuntoPersistence.UpdateRelationshipWithLivro(livroDTO.CodL.Value, assuntos))
                    throw new CadastroLivrosDataBaseException("Erro ao inserir/atualizar assunto(s) do livro.");
            }

            if (livroDTO.Autores != null && livroDTO.Autores.Any())
            {
                var autores = AutorMapping.FromDTOList(livroDTO.Autores);
                if (!await _autorPersistence.UpdateRelationshipWithLivro(livroDTO.CodL.Value, autores))
                    throw new CadastroLivrosDataBaseException("Erro ao inserir/atualizar autor(es) do livro.");
            }

            if (livroDTO.Precos != null && livroDTO.Precos.Any())
            {
                var precos = PrecoMapping.FromDTOList(livroDTO.Precos, livroDTO.CodL.Value);
                precos = await _precoPersistence.UpdateRelationshipWithLivro(livroDTO.CodL.Value, precos); // vai atualizar precos com os novos IDs adicionados
            }
        }

        private async Task RollBackAndThrowException(Exception ex, string message)
        {
            await _basicPersistence.Rollback();
            if (ex is CadastroLivrosException)
                throw ex;
            else
                throw new CadastroLivrosException(message, ex);
        }

        #endregion

        public async Task<LivroDTO?> Read(long codL)
        {
            var livro = await _livroPersistence.Read(codL);
            if (livro == null)
                throw new CadastroLivrosNotFoundException("Livro não encontrado.");

            var livroDTO = LivroMapping.ToDTO(livro);
            await ReadRelationships(livroDTO);

            return livroDTO;
        }

        public async Task<LivroDTO> Insert(LivroDTO livroDTO)
        {
            if (livroDTO == null)
                throw new CadastroLivrosBadRequestException("Livro inválido, não é possível inserir.");

            try
            {
                await _basicPersistence.BeginTransaction();

                var livro = LivroMapping.FromDTO(livroDTO);
                livro = await _livroPersistence.Insert(livro); // Insert vai atualizar livro com o novo ID adicionado
                livroDTO.CodL = livro.CodL;

                await UpdateRelationships(livroDTO);

                await _basicPersistence.Commit();
            }
            catch (Exception ex)
            {
                await RollBackAndThrowException(ex, $"Erro ao inserir livro. Detalhes: {ex.Message}");
            }

            return livroDTO;
        }

        public async Task Update(LivroDTO livroDTO)
        {
            if (livroDTO == null || livroDTO.CodL <= 0)
                throw new CadastroLivrosBadRequestException("Livro inválido, não é possível atualizar.");

            try
            {
                if (!await _livroPersistence.Exists(livroDTO.CodL.Value))
                    throw new CadastroLivrosNotFoundException("Livro não encontrado.");

                await _basicPersistence.BeginTransaction();

                var livro = LivroMapping.FromDTO(livroDTO);
                if (!await _livroPersistence.Update(livro))
                    throw new CadastroLivrosDataBaseException("Erro ao atualizar livro no banco de dados.");

                await UpdateRelationships(livroDTO);

                await _basicPersistence.Commit();
            }
            catch (Exception ex) 
            {
                await RollBackAndThrowException(ex, $"Erro ao atualizar livro. Detalhes: {ex.Message}");
            }
        }

        public async Task Delete(long codL)
        {
            if (codL <= 0)
                throw new CadastroLivrosBadRequestException("Livro inválido, não é possível deletar.");
            try
            {
                if (!await _livroPersistence.Exists(codL))
                    throw new CadastroLivrosNotFoundException("Livro não encontrado.");

                await _basicPersistence.BeginTransaction();

                if (!await _livroPersistence.Delete(codL)) // deleta também relacionamentos Livro_Autor, Livro_Assunto e Precos
                    throw new CadastroLivrosDataBaseException("Erro ao deletar livro do banco de dados.");

                await _basicPersistence.Commit();
            }
            catch (Exception ex)
            {
                await RollBackAndThrowException(ex, $"Erro ao deletar livro. Detalhes: {ex.Message}");
            }
        }

        public async Task<IEnumerable<LivroDTO>> ReadPage(int page = 1, int pageSize = 40)
        {
            var offset = (page - 1) * pageSize;
            var livros = await _livroPersistence.ReadList(offset, pageSize);

            var livrosDTO = new List<LivroDTO>();

            foreach (var livro in livros)
            {
                var livroDTO = LivroMapping.ToDTO(livro);
                await ReadRelationships(livroDTO);
                livrosDTO.Add(livroDTO);
            }

            return livrosDTO;
        }
    }
}
