using AutomationTest.Core;
using Bot.ReservaMultiBR.Util;

namespace Test
{
    public class Program
    {
        private static List<string> CODES_ARRAY = new List<string>();

        private static string URL_TOKEN = "";

        private static SeleniumHelper Selenium = new SeleniumHelper(new ConfigurationHelper());

        public static void Main()
        {
            FileHelpers.CreateDirectorys();

            Start();
        }

        public static void Start()
        {
            try
            {
                Console.Clear();
                Console.Title = "..:::: MULT BR SERVICOS FINANCEIROS LTDA ::::..";
                Selenium.Delay(9000);

                FileHelpers.SetInfos(".:: 1. Tem grupos pendentes?");
                CODES_ARRAY = FileHelpers.Pendentes();

                if (CODES_ARRAY?.Count > 0)
                {
                    FileHelpers.SetInfos(".:: 3. Login");
                    Selenium.GoToUrl("https://edigital.rodobens.com.br/parceiros/home");
                    Selenium.FillTextBoxById("signInName", "23539897000121");
                    Selenium.FillTextBoxById("password", "Lousada@0409");
                    Selenium.ClickById("next");

                    Selenium.Delay(9000);
                    FileHelpers.SetInfos(".:: 4. Acessar Consorcio");
                    Selenium.ExecuteScript();

                    Selenium.Delay(2000);
                    FileHelpers.SetInfos(".:: 5. Selecionar Tab");
                    Selenium.SetTab();

                    FileHelpers.SetInfos(".:: 6. Token");
                    URL_TOKEN = Selenium.GetUrl();

                    FileHelpers.SetInfos(".:: 7. Feito");
                    Selenium.Dispose();

                    WorkFlow(); // iniciar processo
                    Restart(30 * 60 * 1000); // proxima execução em 30 minutos
                }
                else
                {
                    FileHelpers.SetInfos(".:: 4. Sem grupos no arquivo de pendentes");

                    Selenium.Dispose();

                    return;
                }
            }
            catch (Exception error)
            {
                FileHelpers.SetErrors(error?.Message);

                Restart();
            }
        }

        private static void Restart(int timer = 10000)
        {
            Selenium.Delay(9000);
            Selenium.Dispose();
            FileHelpers.SetInfos("Proxímo processamento:" + DateTime.Now.AddSeconds(10));
            Thread.Sleep(timer);
            Start();
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

                        FileHelpers.SetInfos(".:: 1. Inicio de tentativa de reserva: " + code);
                        FileHelpers.SetInfos(".:: 2. Selecionar consorcio");
                        Selenium.GoToUrl(URL_TOKEN);

                        FileHelpers.SetInfos(".:: 3. Procurando o Card RODOBENS...");
                        Selenium.Delay(9000);
                        Procurar_RODOBENS(Selenium);

                        FileHelpers.SetInfos(".:: 5. Acessar Reserva");
                        Selenium.Delay(9000);
                        Selenium.ClickByXPath("/html/body/div/div[2]/div/div[1]/div[1]/nav/div[1]/div[3]/div[5]/div[2]/div");

                        FileHelpers.SetInfos(".:: 6. Nova Reserva");
                        Selenium.Delay(9000);
                        Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/main/div/div/div[1]/div/div[3]/div/button");

                        var existeReserva = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]");
                        if (existeReserva.Contains("Não foi possível buscar as reservas"))
                        {
                            FileHelpers.SetInfos(".:: 6. Não foi possível buscar as reservas?");
                            Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[3]/div[2]/button");
                        }
                        else
                        {
                            FileHelpers.SetInfos(".:: 7. Buscar as reservas");
                        }

                        FileHelpers.SetInfos(".:: 8. Grupo Disponíveis");
                        Selenium.Delay(9000);
                        Selenium.FillTextBoxByXPath("/html/body/div/div[1]/div/div/div[2]/div[1]/div/div/div[1]/input", code);

                        FileHelpers.SetInfos(".:: 9. Buscar reserva");
                        Selenium.Delay(9000);
                        var vendaDisponivel = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div");
                        if (vendaDisponivel.Contains("Condições de venda não disponíveis"))
                        {
                            Selenium.Dispose();
                            FileHelpers.SetStatus(code, ".:: 10. Fim: Condições de venda não disponíveis\n");
                            continue;
                        }

                        FileHelpers.SetInfos(".:: 10. Existe Grupo?");
                        Selenium.Delay(9000);
                        var existeGrupo = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div/div");
                        if (existeGrupo.Contains("Nenhum resultado"))
                        {
                            Selenium.Dispose();
                            FileHelpers.SetStatus(code, ".:: 11. Fim: Nenhum resultado\n");
                            continue;
                        }

                        FileHelpers.SetInfos(".:: 11. Clique na lista");
                        Selenium.Delay(1000);
                        Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div");

                        FileHelpers.SetInfos(".:: 12. Reservar conta");
                        Selenium.Delay(1000);
                        Selenium.ClickById("ButtonReservarCota");
                        Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[3]/div[2]/button[2]");


                        FileHelpers.SetInfos(".:: 13. Validar limite maximo de reservas");
                        Selenium.Delay(9000);
                        var utrapassouLimte = Selenium.GetTextByXPath("/html/body/div/div[2]/div/div/div[1]");
                        if (utrapassouLimte.Contains("ATENÇÃO"))
                        {
                            FileHelpers.SetStatus(code, ".:: 14. Fim: Foi ultrapassada a quantidade máxima de reservas de cotas em estoque para este usuário.\n");
                            Selenium.Dispose();
                            continue;
                        }

                        FileHelpers.SetInfos(".:: 14. Confirmar");
                        Selenium.Delay(1000);
                        Selenium.ClickByXPath("/html/body/div/div[2]/div/div/div[3]/div[2]/button[2]");

                        Selenium.Delay(9000);

                        //MOCK
                        // var sucesso = Selenium.GetTextByXPath("/html/body/div/div[2]/div/div[1]/div[1]/main/div/div/div/div[2]/div[3]/div/div/div/h2");
                        var sucesso = "Selecione o Produto";

                        if (sucesso.Contains("Selecione o Produto"))
                        {
                            FileHelpers.SetStatus(code, ".:: 15. Reservado com sucesso");

                            Mail.Send(); // enviar email
                        }
                    }
                }
                catch (Exception error)
                {
                    FileHelpers.SetErrors(error?.Message);

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
                FileHelpers.SetInfos(".:: 4. Segundo card achou RODOBENS...");
                Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[3]/div/div[2]/button");
                Selenium.Delay(9000);
                return;
            }

            rodobens = Selenium.GetTextByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[2]/div/p");
            if (rodobens.Contains("RODOBENS"))
            {
                FileHelpers.SetInfos(".:: 4. Primeiro card achou RODOBENS...");
                Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[2]/div/div[2]/button");
                Selenium.Delay(9000);
                return;
            }
        }
    }
}