using Bot.ReservaMultiBR.Util;
using Factory.Portobens;
using Factory.Rodobens;

namespace Test
{
    public class Program
    {
        public static void Main()
        {
            Console.Title = "..:::: MULT BR  ::::.. 2.0";

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
                    while (true)
                    {
                        Rodobens.Start();
                        Portobens.Start();
                        BRQualy.Start();
                        CNF.Start();
                    }
                case "2":
                    while (true)
                    {
                        Portobens.Start();
                        Rodobens.Start();
                        BRQualy.Start();
                        CNF.Start();
                    }

                case "3":
                    while (true)
                    {
                        BRQualy.Start();
                        Portobens.Start();
                        Rodobens.Start();
                        CNF.Start();
                    }
                case "4":
                    while (true)
                    {
                        CNF.Start();
                        BRQualy.Start();
                        Portobens.Start();
                        Rodobens.Start();
                    }
                default:
                    Console.Clear();
                    Console.WriteLine($"Opção inválida");
                    Main();
                    return;

            }
        }
    }
}