using AutomationTest.Core;
using System.IO;

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
            try
            {

                Console.Title = "..:::: MULT BR SERVICOS FINANCEIROS LTDA ::::..";
                Selenium.Delay(9000);
                Console.Clear();

                Console.WriteLine(".:: 1. Login");
                Selenium.GoToUrl("https://edigital.rodobens.com.br/parceiros/home");
                Selenium.FillTextBoxById("signInName", "23539897000121");
                Selenium.FillTextBoxById("password", "Lousada@0409");
                Selenium.ClickById("next");

                Selenium.Delay(9000);
                Console.WriteLine(".:: 2. Acessar Consorsio");
                Selenium.ExecuteScript();

                Console.WriteLine(".:: 3. Obter Arquivo");
                CODES_ARRAY = Pendentes();

                Selenium.Delay(2000);
                Console.WriteLine(".:: 4. Selecionar Tab");
                Selenium.SetTab();

                Console.WriteLine(".:: 5. Token");
                URL_TOKEN = Selenium.GetUrl();
                Console.WriteLine(".:: 6. Feito");
                Selenium.Dispose();
            }
            finally
            {
                Start();
            }
        }

        private static List<string> Pendentes()
        {
            String line = string.Empty;

            using (StreamReader readerPendentes = new StreamReader("C:\\bots\\pendentes\\pendentes.txt"))
            {
                line = readerPendentes.ReadLine();
            }

            return line?.Split(';').ToList();
        }

        private static void SetReprocessar(string code)
        {
            SetProcessamentos(code);

            String line = string.Empty;

            using (StreamReader readerPendentes = new StreamReader("C:\\bots\\pendentes\\pendentes.txt"))
            {
                line = readerPendentes.ReadLine();
            }

            using (StreamWriter writerPendentes = new StreamWriter("C:\\bots\\pendentes\\pendentes.txt"))
            {
                line = line.Replace(code + ";", "");

                line += code + ";";

                writerPendentes.WriteLine(line);
            }

        }

        private static void SetSucesso(string code)
        {
            SetProcessamentos(code);

            String line = string.Empty;

            using (StreamReader readerPendentes = new StreamReader("C:\\bots\\pendentes\\pendentes.txt"))
            {
                line = readerPendentes.ReadLine();
            }

            using (StreamWriter writerPendentes = new StreamWriter("C:\\bots\\pendentes\\sucesso.txt"))
            {
                line = line.Replace(code + ";", "");

                line += code + ";";

                writerPendentes.WriteLine(line);
            }
        }

        private static void SetProcessamentos(string code)
        {

            String line = string.Empty;

            using (StreamReader readerPendentes = new StreamReader("C:\\bots\\pendentes\\processamentos.txt"))
            {
                line = readerPendentes.ReadLine();
            }

            using (StreamWriter writerPendentes = new StreamWriter("C:\\bots\\processamentos\\processamentos.txt"))
            {
                line += DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + "/Grupo:" + code + "\n";

                writerPendentes.WriteLine(line);
            }
        }

        private static void WorkFlow()
        {

            if (CODES_ARRAY is not null)
            {
                try
                {
                    foreach (var code in CODES_ARRAY)
                    {
                        if (code.Length <= 2) continue;

                        Selenium = new SeleniumHelper(new ConfigurationHelper());

                        Console.Clear();
                        Console.WriteLine(".:: Inicio --> " + code);
                        Console.WriteLine(".:: 1. Selecionar consorcio");
                        Selenium.GoToUrl(URL_TOKEN);

                        Console.WriteLine(".:: 2. Procurando o Card RODOBENS...");
                        Selenium.Delay(9000);
                        Procurar_RODOBENS(Selenium);

                        Console.WriteLine(".:: 4. Acessar Reserva");
                        Selenium.Delay(9000);
                        Selenium.ClickByXPath("/html/body/div/div[2]/div/div[1]/div[1]/nav/div[1]/div[3]/div[5]/div[2]/div");

                        Console.WriteLine(".:: 5. Nova Reserva");
                        Selenium.Delay(9000);
                        Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/main/div/div/div[1]/div/div[3]/div/button");

                        var existeReserva = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]");
                        if (existeReserva.Contains("Não foi possível buscar as reservas"))
                        {
                            Console.WriteLine(".:: 5. Não foi possível buscar as reservas?");
                            Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[3]/div[2]/button");
                        }
                        else
                        {
                            Console.WriteLine(".:: 6. Buscar as reservas");
                        }

                        Console.WriteLine(".:: 7. Grupo Disponíveis");
                        Selenium.Delay(9000);
                        Selenium.FillTextBoxByXPath("/html/body/div/div[1]/div/div/div[2]/div[1]/div/div/div[1]/input", code);

                        Console.WriteLine(".:: 8. Buscar reserva");
                        Selenium.Delay(9000);
                        var vendaDisponivel = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div");
                        if (vendaDisponivel.Contains("Condições de venda não disponíveis"))
                        {
                            RESULTADOS.Add(code.ToString() + ".:: Fim: Condições de venda não disponíveis\n");
                            Selenium.Dispose();
                            SetReprocessar(code);
                            continue;
                        }

                        Console.WriteLine(".:: 9. Existe Grupo?");
                        Selenium.Delay(9000);
                        var existeGrupo = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div/div");
                        if (existeGrupo.Contains("Nenhum resultado"))
                        {
                            RESULTADOS.Add(code.ToString() + ".:: Fim: Nenhum resultado\n");
                            Selenium.Dispose();
                            SetReprocessar(code);
                            continue;
                        }

                        Console.WriteLine(".:: 10. Clique na lista");
                        Selenium.Delay(1000);
                        Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div");

                        Console.WriteLine(".:: 11. Reservar conta");
                        Selenium.Delay(1000);
                        Selenium.ClickById("ButtonReservarCota");
                        Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[3]/div[2]/button[2]");


                        Console.WriteLine(".:: 12. Validar limite maximo de reservas");
                        Selenium.Delay(9000);
                        var utrapassouLimte = Selenium.GetTextByXPath("/html/body/div/div[2]/div/div/div[1]");
                        if (utrapassouLimte.Contains("ATENÇÃO"))
                        {
                            RESULTADOS.Add(code.ToString() + ".:: Fim: Foi ultrapassada a quantidade máxima de reservas de cotas em estoque para este usuário.\n");
                            Selenium.Dispose();
                            SetReprocessar(code);
                            continue;
                        }

                        Console.WriteLine(".:: 13. Confirmar");
                        Selenium.Delay(1000);
                        Selenium.ClickByXPath("/html/body/div/div[2]/div/div/div[3]/div[2]/button[2]");

                        Selenium.Delay(9000);
                        var sucesso = Selenium.GetTextByXPath("/html/body/div/div[2]/div/div[1]/div[1]/main/div/div/div/div[2]/div[3]/div/div/div/h2");

                        if (sucesso.Contains("Selecione o Produto"))
                        {
                            RESULTADOS.Add(code.ToString() + ".:: Sucesso");
                            Selenium.Delay(9000);
                            Selenium.Dispose();
                            SetSucesso(code);
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
                    Thread.Sleep(15 * 60 * 1000);
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
                Console.WriteLine(".:: 3. Segundo card achou RODOBENS...");
                Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[3]/div/div[2]/button");
                Selenium.Delay(9000);
                return;
            }

            rodobens = Selenium.GetTextByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[2]/div/p");
            if (rodobens.Contains("RODOBENS"))
            {
                Console.WriteLine(".:: 3. Primeiro card achou RODOBENS...");
                Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[2]/div/div[2]/button");
                Selenium.Delay(9000);
                return;
            }

        }
    }
}