namespace MJV.Models
{
    public class Loja
    {
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public List<Section> sections { get; set; } 

    }

    public class Section
    {
        public string Nome { get; set; }
        public List<Produto> Produtos { get; set; }

    }
}
//fazer botoes para ir em duas lojas diferentes