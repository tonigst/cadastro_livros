using CadastroLivros.Data.Entities;
using CadastroLivros.Data.Models;

namespace CadastroLivros.Data.Mapping
{
    public static class LivroMapping
    {
        public static LivroDTO ToDTO(Livro livro)
        {
            return new LivroDTO()
            {
                CodL = livro.CodL,
                Titulo = livro.Titulo,
                Edicao = livro.Edicao,
                Editora = livro.Editora,
                AnoPublicacao = livro.AnoPublicacao
            };
        }

        public static Livro FromDTO(LivroDTO livroDTO)
        {
            return new Livro()
            {
                CodL = livroDTO.CodL ?? -1,
                Titulo = livroDTO.Titulo,
                Edicao = livroDTO.Edicao,
                Editora = livroDTO.Editora,
                AnoPublicacao = livroDTO.AnoPublicacao
            };
        }
    }
}
