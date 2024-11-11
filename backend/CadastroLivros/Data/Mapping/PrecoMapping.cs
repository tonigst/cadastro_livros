using CadastroLivros.Data.Entities;
using CadastroLivros.Data.Models;

namespace CadastroLivros.Data.Mapping
{
    public static class PrecoMapping
    {
        public static PrecoDTO ToDTO(Preco preco)
        {
            return new PrecoDTO()
            {
                CodP = preco.CodP,
                CodFC = preco.CodFC,
                FormaCompra = preco.FormaCompra,
                Valor = preco.Valor,
            };
        }

        public static Preco FromDTO(PrecoDTO precoDTO, long codL)
        {
            return new Preco()
            {
                CodP = precoDTO.CodP ?? -1,
                CodFC = precoDTO.CodFC ?? -1,
                CodL = codL,
                FormaCompra = precoDTO.FormaCompra,
                Valor = precoDTO.Valor,
            };
        }

        public static FormaCompra ToFormaCompra(Preco preco)
        {
            return new FormaCompra
            {
                CodFC = preco.CodFC,
                Descricao = preco.FormaCompra,
            };
        }

        public static IEnumerable<PrecoDTO> ToDTOList(IEnumerable<Preco> precos)
        {
            return precos.Select(a => ToDTO(a));
        }

        public static IEnumerable<Preco> FromDTOList(IEnumerable<PrecoDTO> precosDTO, long codL)
        {
            return precosDTO.Select(a => FromDTO(a, codL));
        }
    }
}
