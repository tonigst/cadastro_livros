namespace CadastroLivros.Data.Models
{
    public class AssuntoDTO
    {
        public long? CodAs { get; set; }       // Insert:ignorado    UpdateRelationshipWithLivro: null ou <= 0 cria novo assunto com Descricao
        public string? Descricao { get; set; } // UpdateRelationshipWithLivro: ignorado se CodAs >= 1 
    }
}
