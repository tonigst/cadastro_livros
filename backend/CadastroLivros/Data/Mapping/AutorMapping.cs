using CadastroLivros.Data.Entities;
using CadastroLivros.Data.Models;

namespace CadastroLivros.Data.Mapping
{
    public static class AutorMapping
    {
        public static AutorDTO ToDTO(Autor autor)
        {
            return new AutorDTO()
            {
                CodAu = autor.CodAu,
                Nome = autor.Nome,
            };
        }

        public static Autor FromDTO(AutorDTO autorDTO)
        {
            return new Autor()
            {
                CodAu = autorDTO.CodAu ?? -1,
                Nome = autorDTO.Nome,
            };
        }

        public static IEnumerable<AutorDTO> ToDTOList(IEnumerable<Autor> autores)
        {
            return autores.Select(a => ToDTO(a));
        }

        public static IEnumerable<Autor> FromDTOList(IEnumerable<AutorDTO> autoresDTO)
        {
            return autoresDTO.Select(a => FromDTO(a));
        }
    }
}
