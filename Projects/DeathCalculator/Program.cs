namespace MJV
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Entre com seus dados: ");

            try
            {
                bool Continue = false;
                do
                {
                    VerificationList();

                    Console.Write("Deseja inserir dados novamente? (s/n): ");
                    string Verification = Console.ReadLine();
                    Continue = VerificacaoStatus(Verification);
                    Console.WriteLine();

                } while (Continue);

                Console.WriteLine("Fim do programa.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            }
        }
        static bool VerificacaoStatus(string Verification)
        {
            if (Verification == "s" || Verification == "S")
            {
                return true;
            }
            else
                return false;
        }

        static void VerificationList()
        {
            int n;
            bool inputValid = false;

            do
            {
                
                Console.Write("Quantas pessoas será registrada ? ");
                string registered = Console.ReadLine();
                if (int.TryParse(registered, out n))
                {
                    inputValid = true;
                }
                else
                {
                    Console.WriteLine("Por favor, insira um número inteiro válido.");
                    Console.WriteLine();
                }

            } while (!inputValid);

            List<People> people = new List<People>();
            CreateListPeople(n, people);
        }
        static void CreateListPeople(int n, List<People> people)
        {
            bool inputValid = false;

            for (int i = 1; i <= n; i++)
            {
                Console.WriteLine();
                Console.WriteLine("Pessoa #" + i);
                VerficationName(inputValid, people);
            }
        }

        static void VerficationName(bool inputValid, List<People> people)
        {
            string name;

            while (!inputValid)
            {
              
                People ClassePeople = new();
                
                Console.Write("Nome: ");
                name = Console.ReadLine();

                try
                {
                    ClassePeople.StringIsNull(name);
                    inputValid = true;
                    VerificationYear(name, inputValid, ClassePeople, people);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        static void VerificationYear(string name, bool inputValid, People ClassePeople, List<People> people)
        {
            while (inputValid)
            {
                Console.Write("Data de Nascimento (dd/mm/aaaa): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime DateOfBirth))
                {
                   inputValid = false;
                   string dateCemAnosString = ClassePeople.CalculationYear(DateOfBirth);

                    Console.WriteLine($"Olá {name}, você vai viver até o ano de {dateCemAnosString:dd/MM/yyyy}.");

                    People peoples = new People(name, dateCemAnosString);
                    people.Add(peoples);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Data de Nascimento inválida. Certifique-se de usar o formato dd/mm/aaaa.");
                    Console.WriteLine();
                }
            }
        }
    }
}

