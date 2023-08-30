
namespace Bot.ReservaMultiBR.Util
{
    public static class FileHelpers
    {
        public static void CreateDirectorys()
        {
            Directory.CreateDirectory(Paths.INFOS);

            Directory.CreateDirectory(Paths.STATUS);

            Directory.CreateDirectory(Paths.ERRORS);
        }
        public static string TemplateReserva(string code, AdministradoraEnum administradora)
        {
            StreamReader str = new StreamReader(Paths.TEMPLATE_RESERVA);

            string MailText = str.ReadToEnd();

            MailText = MailText.Replace("#CODE", code);

            MailText = MailText.Replace("#DataHora", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            switch (administradora)
            {
                case AdministradoraEnum.RODOBENS:
                    MailText = MailText.Replace("#Administradora", "RODOBENS A DE CONSORCIOS LTDA");
                    MailText = MailText.Replace("#reservas", "https://rodobens.escritoriodigitalparceiro.com.br/Sistema/#/reservas");
                    break;

                case AdministradoraEnum.PORTOBENS:
                    MailText = MailText.Replace("#Administradora", "PORTOBENS A DE CONSORCIOS LTDA");
                    MailText = MailText.Replace("#reservas", "https://cmb.escritoriodigitalparceiro.com.br/Sistema/#/reservas");
                    break;

                case AdministradoraEnum.CNF:
                    MailText = MailText.Replace("#Administradora", "CNF A DE CONSORCIOS LTDA");
                    MailText = MailText.Replace("#reservas", "https://cnf.escritoriodigitalparceiro.com.br/Sistema/#/reservas");
                    break;

                case AdministradoraEnum.BRQUALY:
                    MailText = MailText.Replace("#Administradora", "BRQUALY A DE CONSORCIOS LTDA");
                    MailText = MailText.Replace("#reservas", "https://brqualy.escritoriodigitalparceiro.com.br/Sistema/#/reservas");
                    break;
            }

            str.Close();

            return MailText;
        }

        public static string TemplateInfo(string alert, string titulo)
        {
            StreamReader str = new StreamReader(Paths.TEMPLATE_ALERT);

            string MailText = str.ReadToEnd();

            MailText = MailText.Replace("#titulo", titulo);

            MailText = MailText.Replace("#alert", alert);

            MailText = MailText.Replace("#DataHora", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            str.Close();

            return MailText;
        }

        public static List<string> Pendentes(AdministradoraEnum administradora)
        {
            List<string> pendentes = new List<string>();

            try
            {
                string[] lines = File.ReadAllLines(PendentesPath(administradora));

                for (int index = 0; index < lines.Count(); index++)
                {
                    string[] columns = lines[index].Split(',');

                    foreach (string code in columns)
                    {
                        if (code.Length <= 2) continue;

                        pendentes.Add(code.Trim());
                    }
                }
            }
            catch
            {
                SetErrors(".:: Erro ao obter os grupos pendentes. Favor verificar o arquivo pendentes.csv");
            }

            return pendentes;
        }

        public static List<string> Sucesso(AdministradoraEnum administradora)
        {
            List<string> sucesso = new List<string>();

            try
            {
                string[] lines = File.ReadAllLines(SucessoPath(administradora));

                for (int index = 0; index < lines.Count(); index++)
                {
                    string[] columns = lines[index].Split(',');

                    foreach (string column in columns)
                    {
                        sucesso.Add(column.Trim());
                    }
                }
            }
            catch
            {
                SetErrors(".:: Erro ao obter os grupos pendentes. Favor verificar o arquivo sucesso.csv");
            }

            return sucesso;
        }

        public static void SetSucesso(string code, AdministradoraEnum administradora)
        {
            List<string> pendentes = Sucesso(administradora);

            using (StreamWriter writer = new StreamWriter(SucessoPath(administradora)))
            {
                foreach (var item in pendentes)
                {
                    if (item == code) continue;

                    writer.WriteLine(item);
                }

                writer.WriteLine(code);

                writer.Close();
            }
        }

        public static void SetReprocessar(string code, string status, AdministradoraEnum administradora)
        {
            SetStatus(code, status);

            List<string> pendentes = Pendentes(administradora);

            using (StreamWriter writer = new StreamWriter(PendentesPath(administradora)))
            {
                foreach (var item in pendentes)
                {
                    if (item == code) continue;

                    writer.WriteLine(item);
                }

                writer.WriteLine(code);

                writer.Close();
            }
        }

        public static void SetStatus(string code, string status)
        {
            try
            {
                string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Grupo: {code} /Status: {status}";

                SetInfos(status);

                File.AppendAllText($@"{Paths.STATUS}\status.txt", $@"{line}" + Environment.NewLine);

            }
            catch (Exception erro)
            {
                Console.WriteLine(erro.Message);
            }
        }

        public static void SetErrors(string error)
        {
            string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Error: {error}";

            try
            {
                Console.WriteLine(line);

                File.AppendAllText($@"{Paths.ERRORS}\errors.txt", $@"{line}" + Environment.NewLine);

                Mail.Error(error);
            }
            catch (Exception erro)
            {
                Console.WriteLine(erro.Message);
            }
        }

        public static void SetInfos(string infos)
        {
            try
            {
                string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Info: {infos}";

                Console.WriteLine(line);

                File.AppendAllText($@"{Paths.INFOS}\infos.txt", $@"{line}" + Environment.NewLine);
            }
            catch (Exception erro)
            {
                Console.WriteLine(erro.Message);
            }
        }

        public static string SucessoPath(AdministradoraEnum administradora)
        {
            switch (administradora)
            {
                case AdministradoraEnum.RODOBENS:
                    return Paths.SUCESSO_RODOBENS;

                case AdministradoraEnum.PORTOBENS:
                    return Paths.SUCESSO_PORTOBENS;

                case AdministradoraEnum.CNF:
                    return Paths.SUCESSO_CNF;

                case AdministradoraEnum.BRQUALY:
                    return Paths.SUCESSO_BRQUALY;

                default:
                    return "";
            }
        }

        public static string PendentesPath(AdministradoraEnum administradora)
        {
            switch (administradora)
            {
                case AdministradoraEnum.RODOBENS:
                    return Paths.PENDENTES_RODOBENS;

                case AdministradoraEnum.PORTOBENS:
                    return Paths.PENDENTES_PORTOBENS;

                case AdministradoraEnum.CNF:
                    return Paths.PENDENTES_CNF;

                case AdministradoraEnum.BRQUALY:
                    return Paths.PENDENTES_BRQUALY;

                default:
                    return "";
            }
        }
    }
}
