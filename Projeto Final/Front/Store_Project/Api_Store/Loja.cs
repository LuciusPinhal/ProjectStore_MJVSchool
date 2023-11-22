namespace Api_Store
{
    public class Loja
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public List<Section>? Sections { get; set; }

    }

    public class Section
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Produto> Produtos { get; set; }

    }
}