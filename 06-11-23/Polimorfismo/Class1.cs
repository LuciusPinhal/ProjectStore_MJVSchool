namespace testes
{
   public void teste()
   {
        Animal meucachorro = new Cachorro();
        meucachorro.Falar();
    }

    public class Animal
    {
        private string nome = "nome aleatorio";
        public virtual void Falar()
        {
            Console.WriteLine("Um animal");
        }

        public override string ToString()
        {
            return base.ToString();
        }

    }
    public class Cachorro : Animal 
    {
        public override void Falar()
        {
            Console.WriteLine("Au au");
        }
      //polimorfismo
    }

    public class Gato : Animal
    {
        public override void Falar()
        {
            Console.WriteLine("Maiu");
        }

    }
    //classe abistrata 
    public abstract class Veiculo
    {
        public abstract void Mover();
    }
    public class Carro : Veiculo, IVeiculo
    { 
        public override void Mover()
        {
            Console.WriteLine("Carro ta movendo");
        }

        public void movimentar()
        {
            Console.WriteLine("Carro ta movendo");
        }
    }
    //interface rs
    public interface IVeiculo
    {
        void movimentar();
    }

    var joao = new Pessoa(Nome: "Jao", Idade: 25);
    public record Pessoa(string Nome, int Idade);
}

// Cria o diretório se ele não existir
//string caminho = Path.Combine(Environment.CurrentDirectory, "pasta");
//DirectoryInfo dInfo = new DirectoryInfo(caminho);
//dInfo.Create();

// Escreve as informações das pessoas em um arquivo de texto
//string filePath = Path.Combine(caminho, "pessoas.txt");

//using (StreamWriter writer = File.CreateText(filePath))
//{
//    foreach (var pessoa in pessoas)
//    {
//        writer.WriteLine($"Nome: {pessoa.Nome}, Ano que fará 100 anos: {pessoa.Idade}");
//    }
//}

//Console.WriteLine("Dados das pessoas foram escritos no arquivo pessoas.txt na pasta criada.");


        catch (Exception ex)
{
    Console.WriteLine($"Ocorreu um erro: {ex.Message}");
    // posso chamar uma funcao
    // return getNome();
}