namespace Store_Project.Models
{
    public class Loja
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public List<Section>? sections { get; set; } 

    }

    public class Section
    {
        public string Nome { get; set; }
        public List<Produto> Produtos { get; set; }

    }
}
