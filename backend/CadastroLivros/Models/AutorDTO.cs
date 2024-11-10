namespace CadastroLivros.Data.Models
{
    public class AutorDTO
    {
        public int CodAu { get; set; }
        public string Nome { get; set; }

        public IEnumerable<LivroDTO> Livro { get; set; }
    }
}
