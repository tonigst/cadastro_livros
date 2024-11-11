namespace CadastroLivros.Data.Models
{
    public class PrecoDTO
    {
        public long? CodP { get; set; }   // Insert:ignorado    UpdateRelationshipWithLivro:ignorado, deleta todos precos do livro e insere novamente
        public long? CodFC { get; set; }  // UpdateRelationshipWithLivro: null ou <= 0 cria nova forma compra com Descricao = FormaCompra
        public string FormaCompra { get; set; }  // UpdateRelationshipWithLivro: ignorado se CodFC >= 1
        public decimal Valor { get; set; }
    }
}
