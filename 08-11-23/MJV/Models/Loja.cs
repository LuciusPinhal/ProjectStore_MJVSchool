namespace MJV.Models
{
    public class Loja
    {
        public string Nome { get; set; }
        public string Cidade { get; set; }

        public List<Produto> Produtos { get; set; }
    }
}
