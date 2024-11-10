using CadastroLivros.Data.Entities;
using CadastroLivros.Data.Models;

namespace CadastroLivros.Data.Mapping
{
    public static class AssuntoMapping
    {
        public static AssuntoDTO ToDTO(Assunto assunto)
        {
            return new AssuntoDTO()
            {
                CodAs = assunto.CodAs,
                Descricao = assunto.Descricao,
            };
        }

        public static Assunto FromDTO(AssuntoDTO assuntoDTO)
        {
            return new Assunto()
            {
                CodAs = assuntoDTO.CodAs,
                Descricao = assuntoDTO.Descricao,
            };
        }

        public static IEnumerable<AssuntoDTO> ToDTOList(IEnumerable<Assunto> assuntos)
        {
            return assuntos.Select(a => ToDTO(a));
        }

        public static IEnumerable<Assunto> FromDTOList(IEnumerable<AssuntoDTO> assuntosDTO)
        {
            return assuntosDTO.Select(a=> FromDTO(a));
        }
    }
}
