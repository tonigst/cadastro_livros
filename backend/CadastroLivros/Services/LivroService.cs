using CadastroLivros.Data.Entities;
using CadastroLivros.Data.Mapping;
using CadastroLivros.Data.Models;
using CadastroLivros.Data.Persistence.Interfaces;
using CadastroLivros.Exceptions;
using CadastroLivros.Services.Interfaces;

namespace CadastroLivros.Service
{
    public class LivroService : ILivroService
    {
        private readonly ILivroPersistence _livroPersistence;
        private readonly IAutorPersistence _autorPersistence;
        private readonly IAssuntoPersistence _assuntoPersistence;

        public LivroService(ILivroPersistence livroPersistence, IAutorPersistence autorPersistence, IAssuntoPersistence assuntoPersistence)
        {
            _livroPersistence = livroPersistence;
            _autorPersistence = autorPersistence;
            _assuntoPersistence = assuntoPersistence;
        }

        private async Task ReadRelations(LivroDTO livroDTO)
        {
            var assuntos = await _assuntoPersistence.ReadListFromLivro(livroDTO.CodL);
            livroDTO.Assuntos = AssuntoMapping.ToDTOList(assuntos);

            var autores = await _autorPersistence.ReadListFromLivro(livroDTO.CodL);
            livroDTO.Autores = AutorMapping.ToDTOList(autores);
        }
        private async Task InsertOrUpdateRelations(LivroDTO livroDTO)
        {
            if (livroDTO.Assuntos.Any())
            {
                var assuntos = AssuntoMapping.FromDTOList(livroDTO.Assuntos);
                if (!await _assuntoPersistence.InsertOrUpdateFromLivro(livroDTO.CodL, assuntos))
                    throw new CadastroLivrosDataBaseException("Erro ao inserir/atualizar assunto(s) do livro.");
            }

            if (livroDTO.Autores.Any())
            {
                var autores = AutorMapping.FromDTOList(livroDTO.Autores);
                if (!await _autorPersistence.InsertOrUpdateFromLivro(livroDTO.CodL, autores))
                    throw new CadastroLivrosDataBaseException("Erro ao inserir/atualizar autor(es) do livro.");
            }
        }

        public async Task<LivroDTO?> Read(int codL)
        {
            var livro = await _livroPersistence.Read(codL);
            if (livro == null)
                return null;

            var livroDTO = LivroMapping.ToDTO(livro);
            await ReadRelations(livroDTO);

            return livroDTO;
        }

        public async Task<LivroDTO> Insert(LivroDTO livroDTO)
        {
            if (livroDTO == null)
                throw new CadastroLivrosBadRequestException("Livro inválido, não é possível inserir.");

            var livro = LivroMapping.FromDTO(livroDTO);
            livro = await _livroPersistence.Insert(livro); // Insert vai atualizar livro com o novo ID adicionado
            livroDTO.CodL = livro.CodL;

            await InsertOrUpdateRelations(livroDTO);

            return livroDTO;
        }

        public async Task Update(LivroDTO livroDTO)
        {
            if (livroDTO == null || livroDTO.CodL <= 0)
                throw new CadastroLivrosBadRequestException("Livro inválido, não é possível atualizar.");

            var livro = LivroMapping.FromDTO(livroDTO);
            if (!await _livroPersistence.Update(livro))
                throw new CadastroLivrosDataBaseException("Erro ao atualizar livro.");

            await InsertOrUpdateRelations(livroDTO);
        }

        public async Task Delete(int codL)
        {
            if (codL <= 0)
                throw new CadastroLivrosBadRequestException("Livro inválido, não é possível deletar.");

            if (!await _livroPersistence.Delete(codL)) // deleta também relacionamentos Livro_Autor, Livro_Assunto e Precos
                throw new CadastroLivrosDataBaseException("Erro ao deletar livro.");
        }

        public async Task<IEnumerable<LivroDTO>> ReadPage(int page = 1, int pageSize = 40)
        {
            var offset = (page - 1) * pageSize;
            var livros = await _livroPersistence.ReadList(offset, pageSize);

            var livrosDTO = new List<LivroDTO>();

            foreach (var livro in livros)
            {
                var livroDTO = LivroMapping.ToDTO(livro);
                await ReadRelations(livroDTO);
                livrosDTO.Add(livroDTO);
            }

            return livrosDTO;
        }
    }
}
