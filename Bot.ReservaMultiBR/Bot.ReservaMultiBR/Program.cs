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
            FileHelpers.SetProcessamentos("teste");
        }
        public static void Start()
        {
            try
            {
                Console.Title = "..:::: MULT BR SERVICOS FINANCEIROS LTDA ::::..";
                Selenium.Delay(9000);
                Console.Clear();

                FileHelpers.SetInfos(".:: 1. Login");
                Selenium.GoToUrl("https://edigital.rodobens.com.br/parceiros/home");
                Selenium.FillTextBoxById("signInName", "23539897000121");
                Selenium.FillTextBoxById("password", "Lousada@0409");
                Selenium.ClickById("next");

                Selenium.Delay(9000);
                FileHelpers.SetInfos(".:: 2. Acessar Consorsio");
                Selenium.ExecuteScript();

                FileHelpers.SetInfos(".:: 3. Obter Arquivo");
                CODES_ARRAY = FileHelpers.Pendentes();

                Selenium.Delay(2000);
                FileHelpers.SetInfos(".:: 4. Selecionar Tab");
                Selenium.SetTab();

                FileHelpers.SetInfos(".:: 5. Token");
                URL_TOKEN = Selenium.GetUrl();

                FileHelpers.SetInfos(".:: 6. Feito");
                Selenium.Dispose();

                WorkFlow();
            }
            catch (Exception error)
            {
                FileHelpers.SetErrors(error.Message);
            }
            finally
            {
                Start();
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
                        FileHelpers.SetInfos(".:: Inicio --> " + code);
                        FileHelpers.SetInfos(".:: 1. Selecionar consorcio");
                        Selenium.GoToUrl(URL_TOKEN);

                        FileHelpers.SetInfos(".:: 2. Procurando o Card RODOBENS...");
                        Selenium.Delay(9000);
                        Procurar_RODOBENS(Selenium);

                        FileHelpers.SetInfos(".:: 4. Acessar Reserva");
                        Selenium.Delay(9000);
                        Selenium.ClickByXPath("/html/body/div/div[2]/div/div[1]/div[1]/nav/div[1]/div[3]/div[5]/div[2]/div");

                        FileHelpers.SetInfos(".:: 5. Nova Reserva");
                        Selenium.Delay(9000);
                        Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/main/div/div/div[1]/div/div[3]/div/button");

                        var existeReserva = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]");
                        if (existeReserva.Contains("Não foi possível buscar as reservas"))
                        {
                            FileHelpers.SetInfos(".:: 5. Não foi possível buscar as reservas?");
                            Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[3]/div[2]/button");
                        }
                        else
                        {
                            FileHelpers.SetInfos(".:: 6. Buscar as reservas");
                        }

                        FileHelpers.SetInfos(".:: 7. Grupo Disponíveis");
                        Selenium.Delay(9000);
                        Selenium.FillTextBoxByXPath("/html/body/div/div[1]/div/div/div[2]/div[1]/div/div/div[1]/input", code);

                        FileHelpers.SetInfos(".:: 8. Buscar reserva");
                        Selenium.Delay(9000);
                        var vendaDisponivel = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div");
                        if (vendaDisponivel.Contains("Condições de venda não disponíveis"))
                        {
                            FileHelpers.SetInfos(code.ToString() + ".:: Fim: Condições de venda não disponíveis\n");
                            Selenium.Dispose();
                            FileHelpers.SetReprocessar(code);
                            continue;
                        }

                        FileHelpers.SetInfos(".:: 9. Existe Grupo?");
                        Selenium.Delay(9000);
                        var existeGrupo = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div/div");
                        if (existeGrupo.Contains("Nenhum resultado"))
                        {
                            FileHelpers.SetInfos(code.ToString() + ".:: Fim: Nenhum resultado\n");
                            Selenium.Dispose();
                            FileHelpers.SetReprocessar(code);
                            continue;
                        }

                        FileHelpers.SetInfos(".:: 10. Clique na lista");
                        Selenium.Delay(1000);
                        Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div");

                        FileHelpers.SetInfos(".:: 11. Reservar conta");
                        Selenium.Delay(1000);
                        Selenium.ClickById("ButtonReservarCota");
                        Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[3]/div[2]/button[2]");


                        FileHelpers.SetInfos(".:: 12. Validar limite maximo de reservas");
                        Selenium.Delay(9000);
                        var utrapassouLimte = Selenium.GetTextByXPath("/html/body/div/div[2]/div/div/div[1]");
                        if (utrapassouLimte.Contains("ATENÇÃO"))
                        {
                            FileHelpers.SetInfos(code.ToString() + ".:: Fim: Foi ultrapassada a quantidade máxima de reservas de cotas em estoque para este usuário.\n");
                            Selenium.Dispose();
                            FileHelpers.SetReprocessar(code);
                            continue;
                        }

                        FileHelpers.SetInfos(".:: 13. Confirmar");
                        Selenium.Delay(1000);
                        Selenium.ClickByXPath("/html/body/div/div[2]/div/div/div[3]/div[2]/button[2]");

                        Selenium.Delay(9000);
                        var sucesso = Selenium.GetTextByXPath("/html/body/div/div[2]/div/div[1]/div[1]/main/div/div/div/div[2]/div[3]/div/div/div/h2");

                        if (sucesso.Contains("Selecione o Produto"))
                        {
                            FileHelpers.SetInfos(code.ToString() + ".:: Sucesso");
                            Selenium.Delay(9000);
                            Selenium.Dispose();
                            FileHelpers.SetSucesso(code);
                            continue;
                        }
                    }
                }
                catch (Exception error)
                {
                    FileHelpers.SetErrors(error.Message);
                }
                finally
                {
                    Selenium.Dispose();
                    Console.Clear();
                    FileHelpers.SetInfos("Proxímo processamento:" + DateTime.Now.AddMilliseconds(30 * 60 * 1000));
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
                FileHelpers.SetInfos(".:: 3. Segundo card achou RODOBENS...");
                Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[3]/div/div[2]/button");
                Selenium.Delay(9000);
                return;
            }

            rodobens = Selenium.GetTextByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[2]/div/p");
            if (rodobens.Contains("RODOBENS"))
            {
                FileHelpers.SetInfos(".:: 3. Primeiro card achou RODOBENS...");
                Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[2]/div/div[2]/button");
                Selenium.Delay(9000);
                return;
            }

        }
    }
}