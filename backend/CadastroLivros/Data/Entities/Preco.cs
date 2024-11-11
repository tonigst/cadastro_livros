namespace CadastroLivros.Data.Entities
{
    public class Preco
    {
        public long CodP { get; set; }
        public long CodL { get; set; }
        public long CodFC { get; set; }
        public string FormaCompra { get; set; }
        public decimal Valor { get; set; }
    }
}
