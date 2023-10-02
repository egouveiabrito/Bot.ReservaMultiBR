using Bot.ReservaMultiBR.Util;
using Factory.Portobens;
using Factory.Rodobens;

namespace Test
{
    public class Program
    {
        public static void Main()
        {
            Console.Title = "..:::: MULT BR  ::::.. 2.5";

            Console.Write("Aperte ENTER para começar...", Console.ForegroundColor = ConsoleColor.DarkGreen);

            Console.ReadKey();

            Console.WriteLine(Environment.NewLine);

            FileHelpers.CreateDirectorys();

            Console.WriteLine("Por qual processo deseja começar? ");

            Console.WriteLine("Digite 1 para RODOBENS");

            Console.WriteLine("Digite 2 para PORTOBENS");

            Console.WriteLine("Digite 3 para BRQUALY");

            Console.WriteLine("Digite 4 para CNF");

            var selecao = Console.ReadLine();

            Start(selecao);
        }
        public static void Start(string selecao)
        {
            Shared.Inicializar();

            switch (selecao)
            {
                case "1":
                    Rodobens.Start();
                    Portobens.Start();
                    BRQualy.Start();
                    CNF.Start();
                    break;
                case "2":

                    Portobens.Start();
                    Rodobens.Start();
                    BRQualy.Start();
                    CNF.Start();
                    break;

                case "3":

                    BRQualy.Start();
                    Portobens.Start();
                    Rodobens.Start();
                    CNF.Start();
                    break;
                case "4":

                    CNF.Start();
                    BRQualy.Start();
                    Portobens.Start();
                    Rodobens.Start();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine($"Opção inválida");
                    Main();
                    return;

            }

            Start(selecao);
        }
    }
}