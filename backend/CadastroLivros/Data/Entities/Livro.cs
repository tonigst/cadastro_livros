namespace CadastroLivros.Data.Entities
{
    public class Livro
    {
        public long CodL { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }
    }
}
