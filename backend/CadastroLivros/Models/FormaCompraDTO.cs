namespace CadastroLivros.Data.Models
{
    public class FormaCompraDTO
    {
        public long? CodFC { get; set; }       // Insert:ignorado    UpdateRelationshipWithLivro: null ou <= 0 cria nova FormaCompra com Descricao
        public string Descricao { get; set; }  // UpdateRelationshipWithLivro: ignorado se CodFC >= 1 
    }
}