using AutomationTest.Core;
using Bot.ReservaMultiBR.Util;

namespace Factory.Rodobens
{
    public static class BRQualy
    {
        private static List<string> CODES_ARRAY = new List<string>();

        private static string URL_TOKEN = "";

        private static SeleniumHelper Selenium = new SeleniumHelper(new ConfigurationHelper());

        private static int retry_workflow = 0;

        public static void Start()
        {
            try
            {
                CODES_ARRAY = FileHelpers.Pendentes(AdministradoraEnum.BRQUALY);

                if (CODES_ARRAY?.Count > 0)
                {

                    Mail.Info(".:: Inicializado Rodobens ::.");

                    Selenium.Delay(1000);
                    Console.Clear();

                    Console.WriteLine("..:::: MULT BR SERVICOS FINANCEIROS LTDA ::::.. BRQualy" + Environment.NewLine);
                    Console.WriteLine(Shared.Avisos());

                    FileHelpers.SetInfos(".:: Tem grupos pendentes?");
                    FileHelpers.SetInfos(".:: Grupos para procurar: " + string.Join(",", CODES_ARRAY));
                    Selenium.GoToUrl("https://edigital.rodobens.com.br/parceiros/home");

                    Selenium.Delay(9000);
                    FileHelpers.SetInfos(".:: Login");
                    Selenium.FillTextBoxById("signInName", "23539897000121");
                    Selenium.FillTextBoxById("password", "Lousada@0409");
                    Selenium.ClickById("next");

                    Selenium.Delay(9000);
                    FileHelpers.SetInfos(".:: Acessar Consorcio");
                    Selenium.ExecuteScript();

                    Selenium.Delay(2000);
                    FileHelpers.SetInfos(".:: Selecionar Tab");
                    Selenium.SetTab();

                    FileHelpers.SetInfos(".:: Token");
                    URL_TOKEN = Selenium.GetUrl();

                    FileHelpers.SetInfos(".:: Feito");
                    Selenium.Finalizar();

                    WorkFlow();

                    Restart(30 * 60 * 1000);
                }
                else
                {
                    FileHelpers.SetInfos(".:: 4. Sem grupos no arquivo de pendentes");

                    Selenium.Finalizar();

                    return;
                }
            }
            catch (Exception error)
            {
                FileHelpers.SetErrors($"BRQualy - Start() - {error?.Message}");

                Restart();

                Start();
            }
        }

        private static void Restart(int timer = 5000)
        {
            Selenium.Finalizar();
            
            FileHelpers.SetInfos(".:: Proxímo processamento:" + DateTime.Now.AddMilliseconds(timer));

            Selenium.Delay(timer);
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

                        FileHelpers.SetInfos(".:: Inicio de tentativa de reserva: " + code + " na BRQualy");
                        FileHelpers.SetInfos(".:: Selecionar consorcio");
                        Selenium.GoToUrl(URL_TOKEN);


                        Selenium.Delay(9000);
                        FileHelpers.SetInfos(".:: Procurando o Card BRQUALY...");
                        Procurar_BRQUALY(Selenium);

                        Selenium.Delay(9000);
                        FileHelpers.SetInfos(".:: Acessar Reserva");
                        Selenium.ClickByXPath("/html/body/div/div[2]/div/div[1]/div[1]/nav/div[1]/div[3]/div[5]/div[2]/div");


                        Selenium.Delay(9000);
                        FileHelpers.SetInfos(".:: Analisar reservas");
                        InicializarEnvioMail(code);

                        Selenium.Delay(9000);
                        FileHelpers.SetInfos(".:: Nova Reserva");
                        Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/main/div/div/div[1]/div/div[3]/div/button");

                        var existeReserva = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]");
                        if (existeReserva.Contains("Não foi possível buscar as reservas"))
                        {
                            FileHelpers.SetInfos(".:: Não foi possível buscar as reservas?");
                            Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[3]/div[2]/button");
                        }
                        else
                        {
                            FileHelpers.SetInfos(".:: Buscar as reservas");
                        }

                        Selenium.Delay(9000);
                        FileHelpers.SetInfos(".:: Grupo Disponíveis");
                        Selenium.FillTextBoxByXPath("/html/body/div/div[1]/div/div/div[2]/div[1]/div/div/div[1]/input", code);


                        Selenium.Delay(9000);
                        FileHelpers.SetInfos(".:: Buscar reserva");
                        var vendaDisponivel = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div");
                        if (vendaDisponivel.Contains("Condições de venda não disponíveis"))
                        {
                            FileHelpers.SetReprocessar(code, ".:: Condições de venda não disponíveis\n", AdministradoraEnum.BRQUALY);
                            Selenium.Finalizar();
                            continue;
                        }

                        Selenium.Delay(9000);
                        FileHelpers.SetInfos(".:: Existe Resultado...?");
                        var existeGrupo = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div/div");
                        if (existeGrupo.Contains("Nenhum resultado"))
                        {
                            FileHelpers.SetReprocessar(code, ".:: Nenhum resultado", AdministradoraEnum.BRQUALY);
                            Selenium.Finalizar();
                            continue;
                        }


                        Selenium.Delay(9000);
                        FileHelpers.SetInfos(".:: Existe Contas para esse grupo...?");
                        var oGrupoNaoTem = Selenium.GetTextByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div/div");
                        if (existeGrupo.Contains("Grupo não"))
                        {
                            FileHelpers.SetReprocessar(code, ".:: O Grupo não tem contas\n", AdministradoraEnum.BRQUALY);
                            Selenium.Finalizar();
                            continue;
                        }

                        Selenium.Delay(1000);
                        FileHelpers.SetInfos(".:: Clique na lista");
                        Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[2]/div[2]/div");


                        Selenium.Delay(1000);
                        FileHelpers.SetInfos(".:: Reservar cota");
                        Selenium.ClickById("ButtonReservarCota");
                        Selenium.ClickByXPath("/html/body/div/div[1]/div/div/div[3]/div[2]/button[2]");

                        Selenium.Delay(1000);
                        FileHelpers.SetInfos(".:: Confirmar");
                        Selenium.ClickByXPath("/html/body/div/div[2]/div/div/div[3]/div[2]/button[2]");

                        Selenium.Delay(1000);
                        FileHelpers.SetInfos(".:: Enviar e-mails...");
                        Mail.Reserva(code, AdministradoraEnum.BRQUALY);

                        Selenium.Delay(1000);
                        FileHelpers.SetSucesso(code, AdministradoraEnum.BRQUALY);
                        FileHelpers.SetReprocessar(code, ".:: Reservado com sucesso", AdministradoraEnum.BRQUALY);

                        Selenium.Dispose();

                        continue;
                    }
                }
                catch (Exception error)
                {
                    FileHelpers.SetErrors($"WorkFlow(): {error?.Message}");

                    Selenium.Finalizar();

                    retry_workflow += 1;

                    if (retry_workflow < 3)
                    {
                        FileHelpers.SetInfos($"Nova tentativa no workflow...{retry_workflow}");

                        CODES_ARRAY = FileHelpers.Pendentes(AdministradoraEnum.BRQUALY);

                        WorkFlow();
                    }
                    else
                    {
                        retry_workflow = 0;
                    }
                }
            }
        }

        private static void Procurar_BRQUALY(SeleniumHelper Selenium)
        {
            var BRQualy = string.Empty;

            BRQualy = Selenium.GetTextByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[4]/div/p");

            if (BRQualy.Contains("BRQUALY"))
            {
                FileHelpers.SetInfos(".:: Segundo card achou BRQUALY");
                Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[3]/div/div[2]/button");
                Selenium.Delay(9000);
                return;
            }

            BRQualy = Selenium.GetTextByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[5]/div/p");
            if (BRQualy.Contains("BRQUALY"))
            {
                FileHelpers.SetInfos(".:: Primeiro card achou BRQUALY");
                Selenium.ClickByXPath("/html/body/div/div/div/div[1]/div[1]/div/div[2]/div/div[2]/button");
                Selenium.Delay(9000);
                return;
            }
        }

        private static void InicializarEnvioMail(string code)
        {
            var buscar = string.Empty;

            var sucesso = FileHelpers.Sucesso(AdministradoraEnum.BRQUALY);

            buscar = Selenium.GetTextByXPath("/html/body/div/div/div/div[1]/div[1]/main/div/div/div[1]/div/div[2]");

            if (buscar.Contains(code))
            {
                if (sucesso?.Where(s => s.Contains(code))?.Count() == 0)
                {
                    FileHelpers.SetInfos(".:: Enviar e-mails...");

                    FileHelpers.SetSucesso(code, AdministradoraEnum.BRQUALY);

                    Mail.Reserva(code, AdministradoraEnum.BRQUALY);
                }
            }
        }

    }
}