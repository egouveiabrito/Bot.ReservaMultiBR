using AutomationTest.Core;

namespace Test
{
    public class Program
    {
        private static List<string> CODES_ARRAY = new List<string>();

        private static string URL_TOKEN = "";

        private static SeleniumHelper Selenium = new SeleniumHelper(new ConfigurationHelper());

        private static List<string> RESULTADOS = new List<string>();

        public static void Main()
        {
            Start();
        }
        public static void Start()
        {

            Console.Title = "..::::::::::::::::::: MULT BR SERVICOS FINANCEIROS LTDA :::::::::::::::::::..";

            Selenium.Delay(1000);
            Console.Clear();
            Console.WriteLine(".:: 1. Login :::::::::::::::::::::::::::..");
            Selenium.GoToUrl("https://edigital.rodobens.com.br/parceiros/home");
            Selenium.FillTextBoxById("signInName", "23539897000121");
            Selenium.FillTextBoxById("password", "Lousada@0409");
            Selenium.ClickById("next");

            Console.WriteLine(".:: 2. Acessar Consorsio :::::::::::::::::::::::::::..");
            Selenium.ExecuteScript();
            Selenium.Delay(5000);

            Console.WriteLine(".:: 3. Obter Arquivo :::::::::::::::::::::::::::..");
            CODES_ARRAY = GetCodes();

            Console.WriteLine(".:: 3. Selecionar Tab :::::::::::::::::::::::::::..");
            Selenium.SetTab();

            Console.WriteLine(".:: 4. Token :::::::::::::::::::::::::::..");
            URL_TOKEN = Selenium.GetUrl();

            Console.WriteLine(".:: 5. Feito :::::::::::::::::::::::::::..");
            Selenium.Dispose();
            WorkFlow();
        }

        private static List<string> GetCodes()
        {
            String line = string.Empty;
            try
            {
                StreamReader sr = new StreamReader("C:\\temp\\codes.txt");

                line = sr.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            return line.Split(',').ToList();

        }

        private static void WorkFlow()
        {

            if (CODES_ARRAY is not null)
            {
                try
                {
                    foreach (var code in CODES_ARRAY)
                    {

                        Selenium = new SeleniumHelper(new ConfigurationHelper());

                        Console.Clear();
                        Console.WriteLine(".:: Inicio --> " + code);
                        Console.WriteLine(".:: 1. Selecionar consorcio :::::::::::::::::::::::::::..");
                        Selenium.GoToUrl(URL_TOKEN);

                        Console.WriteLine(".:: 2. Procurando o Card RODOBENS... :::::::::::::::::::::::::::..");
                        Selenium.Delay(5000);
                        Procurar_RODOBENS(Selenium);

                        Console.WriteLine(".:: 3. Acessar Reserva :::::::::::::::::::::::::::..");
                        Selenium.Delay(3000);
                        Selenium.ClickByXPath("/html/body/div/div[2]/div/div[1]/div[1]/nav/div[1]/div[3]/div[5]/div[2]/div");

                        Console.WriteLine(".:: 4. Nova Reserva :::::::::::::::::::::::::::..");
                        Selenium.Delay(3000);
                        Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/main/div/div/div[1]/div/div[3]/div/button");

                        var existeReserva = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]");
                        if (existeReserva.Contains("Não foi possível buscar as reservas"))
                        {
                            Console.WriteLine(".:: 5. Não foi possível buscar as reservas? :::::::::::::::::::::::::::..");
                            Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[3]/div[2]/button");
                        }
                        else
                        {
                            Console.WriteLine(".:: 6. Buscar as reservas :::::::::::::::::::::::::::..");
                        }

                        Console.WriteLine(".:: 7. Grupo Disponíveis :::::::::::::::::::::::::::..");
                        Selenium.Delay(3000);
                        Selenium.FillTextBoxByXPath("/html/body/div/div[1]/div/div/div[2]/div[1]/div/div/div[1]/input", code);

                        Console.WriteLine(".:: 8. Buscar reserva :::::::::::::::::::::::::::..");
                        Selenium.Delay(3000);
                        var vendaDisponivel = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div");
                        if (vendaDisponivel.Contains("Condições de venda não disponíveis"))
                        {
                            RESULTADOS.Add(code.ToString() + ".:: Fim: Condições de venda não disponíveis\n");
                            Selenium.Dispose();
                            continue;
                        }

                        Console.WriteLine(".:: 9. Existe Grupo? :::::::::::::::::::::::::::..");
                        Selenium.Delay(3000);
                        var existeGrupo = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div/div");
                        if (existeGrupo.Contains("Nenhum resultado"))
                        {
                            RESULTADOS.Add(code.ToString() + ".:: Fim: Nenhum resultado\n");
                            Selenium.Dispose();
                            continue;
                        }

                        Console.WriteLine(".:: 10. Clique na lista :::::::::::::::::::::::::::..");
                        Selenium.Delay(1000);
                        Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div");

                        Console.WriteLine(".:: 11. Reservar conta :::::::::::::::::::::::::::..");
                        Selenium.Delay(1000);
                        Selenium.ClickById("ButtonReservarCota");
                        Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[3]/div[2]/button[2]");


                        Console.WriteLine(".:: 12. Validar limite maximo de reservas :::::::::::::::::::::::::::..");
                        Selenium.Delay(3000);
                        var utrapassouLimte = Selenium.GetTextByXPath("/html/body/div/div[2]/div/div/div[1]");
                        if (utrapassouLimte.Contains("ATENÇÃO"))
                        {
                            RESULTADOS.Add(code.ToString() + ".:: Fim: Foi ultrapassada a quantidade máxima de reservas de cotas em estoque para este usuário.\n");
                            Selenium.Dispose();
                            continue;
                        }

                        Console.WriteLine(".:: 13. Confirmar :::::::::::::::::::::::::::..");
                        Selenium.Delay(1000);
                        Selenium.ClickByXPath("/html/body/div/div[2]/div/div/div[3]/div[2]/button[2]");

                        Selenium.Delay(5000);
                        var sucesso = Selenium.GetTextByXPath("/html/body/div/div[2]/div/div[1]/div[1]/main/div/div/div/div[2]/div[3]/div/div/div/h2");

                        if (sucesso.Contains("Selecione o Produto"))
                        {
                            RESULTADOS.Add(code.ToString() + ".:: Sucesso");

                            Selenium.Delay(3000);
                            Selenium.Dispose();

                            continue;
                        }
                    }
                }
                finally
                {
                    Selenium.Dispose();
                    Console.Clear();
                    Console.WriteLine(string.Join(",", RESULTADOS).Replace(",", ""));
                    Console.WriteLine("Proxímo processamento:" + DateTime.Now.AddMilliseconds(30 * 60 * 1000));
                    Thread.Sleep(30 * 60 * 1000);
                    WorkFlow();
                }
            }
        }

        private static void Procurar_RODOBENS(SeleniumHelper Selenium)
        {
            var rodobens = string.Empty;

            rodobens = Selenium.GetTextByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[3]/div/p");
            if (rodobens.Contains("RODOBENS"))
            {
                Console.WriteLine(".:: 2. Achou RODOBENS... :::::::::::::::::::::::::::..");
                Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[3]/div/div[2]/button");
                Selenium.Delay(3000);
                return;
            }

            rodobens = Selenium.GetTextByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[2]/div/p");
            if (rodobens.Contains("RODOBENS"))
            {
                Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[2]/div/div[2]/button");
                Selenium.Delay(3000);
                return;
            }



        }
    }
}