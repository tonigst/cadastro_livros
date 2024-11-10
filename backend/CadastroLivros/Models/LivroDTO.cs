namespace CadastroLivros.Data.Models
{
    public class LivroDTO
    {
        public int CodL { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }

        public IEnumerable<AutorDTO> Autores { get; set; }
        public IEnumerable<AssuntoDTO> Assuntos { get; set; }
        public IEnumerable<PrecoDTO> Precos { get; set; }
    }
}
