using Bot.ReservaMultiBR.Util;
using Factory.Portobens;
using Factory.Rodobens;

namespace Test
{
    public class Program
    {
        public static void Main()
        {
            Console.Title = "..:::: MULT BR  ::::..";

            Console.Write("Aperte ENTER para começar...", Console.ForegroundColor = ConsoleColor.DarkGreen);

            Console.ReadKey();

            Shared.Inicializar();

            FileHelpers.CreateDirectorys();

            Start();
        }
        public static void Start()
        {
            Rodobens.Start();

            Portobens.Start();

            Main();
        }
    }
}