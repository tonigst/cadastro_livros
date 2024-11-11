namespace CadastroLivros.Data.Models
{
    public class AutorDTO
    {
        public long? CodAu { get; set; }  // Insert:ignorado    UpdateRelationshipWithLivro: null ou <= 0 cria novo autor com Nome
        public string? Nome { get; set; } // UpdateRelationshipWithLivro: ignorado se CodAu >= 1 
    }
}
