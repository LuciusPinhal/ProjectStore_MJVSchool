using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;

namespace MJVCurso
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Deseja Jogar na Loteria (s/n): ");
            string jogar = Console.ReadLine();
            while (JogarNovamente(jogar))
            {
                InputsUsuario();
                Console.WriteLine();
                Console.WriteLine("Deseja Jogar na Loteria (s/n): ");
                jogar = Console.ReadLine();
            }

        }

        /// <summary>
        /// Verificação se deseja jogar novamente na loteria
        /// </summary>
        static bool JogarNovamente(string jogar)
        {
            if (jogar == "s" || jogar == "S")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Inputs digitados pelo usuario, Nome e quantidade de bilhetes comprados
        /// </summary>
        static void InputsUsuario()
        {
            Console.Write("Qual é seu nome?: ");
            string nome = Console.ReadLine();
            Console.Write("O bilhete de loteria custa 5 reais");
            Console.Write("Quanto ira investir?: ");
            int n = int.Parse(Console.ReadLine());
            int investimento = n / 5;
            Console.WriteLine();
            Console.WriteLine("Voce comprou " + investimento + " Bilhetes");
            Console.WriteLine();
            Console.WriteLine("Os numeros foram: ");

            CalculosNumerosLoteria(investimento);
        }

        /// <summary>
        /// Criação dos numeros random sorteados 
        /// </summary>
        /// <param name="investimento">Calculo do valor que o usuario comprou dos bilhetes</param>
        static void CalculosNumerosLoteria(int investimento)
        {
            Random random = new Random();
            int acertos;

            int[] numerosSorteados = new int[6];
            for (int i = 0; i < 6; i++)
            {
                numerosSorteados[i] = random.Next(60);
            }

            int[][] bilhetes = new int[investimento][];
            for (int x = 0; x < investimento; x++)
            {

                int[] numeros = new int[6];
                for (int i = 0; i < 6; i++)
                {
                    numeros[i] = random.Next(60);

                }
                bilhetes[x] = numeros;
            }

            Console.WriteLine();

            for (int x = 0; x < investimento; x++)
            {
                acertos = 0;
                Console.Write("Bilhete " + (x + 1) + " tem os seguintes números: ");
                for (int i = 0; i < 6; i++)
                {
                    Console.Write(bilhetes[x][i] + " ");
                    if (bilhetes[x][i] == numerosSorteados[i])
                    {
                        acertos++;
                    }
                }
                Console.Write("| acertou " + acertos + " números.");
                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------------------");
            }

            Console.WriteLine();
            Console.Write("Bilhete Sorteado: ");
            NumeroSortiado(numerosSorteados);
            Console.WriteLine();
        }

        /// <summary>
        /// Imprimir Numero Sorteado
        /// </summary>
        static void NumeroSortiado(int[] NumeroSortiado)
        {
            int[] a = NumeroSortiado;
            for (int i = 0; i < 6; i++)
            {
                Console.Write(a[i] + " ");
            }
        }
    }
}
