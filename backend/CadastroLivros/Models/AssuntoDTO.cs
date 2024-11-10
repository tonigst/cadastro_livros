namespace CadastroLivros.Data.Models
{
    public class AssuntoDTO
    {
        public int CodAs { get; set; }
        public string Descricao { get; set; }

        public IEnumerable<LivroDTO> Livro { get; set; }
    }
}
