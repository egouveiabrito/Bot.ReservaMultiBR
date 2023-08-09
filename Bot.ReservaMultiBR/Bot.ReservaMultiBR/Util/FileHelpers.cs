
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
                    break;
                case AdministradoraEnum.MECEDES_BENS:
                    break;
                default:
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

        public static List<string> Pendentes()
        {
            List<string> pendentes = new List<string>();

            try
            {
                string[] lines = File.ReadAllLines(Paths.PENDENTES);

                for (int index = 0; index < lines.Count(); index++)
                {
                    string[] columns = lines[index].Split(',');

                    foreach (string column in columns)
                    {
                        pendentes.Add(column.Trim());
                    }
                }
            }
            catch
            {
                SetErrors(".:: Erro ao obter os grupos pendentes. Favor verificar o arquivo pendentes.csv");
            }

            return pendentes;
        }

        public static List<string> Sucesso()
        {
            List<string> sucesso = new List<string>();

            try
            {
                string[] lines = File.ReadAllLines(Paths.SUCESSO);

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

        public static void SetSucesso(string code)
        {
            List<string> pendentes = Sucesso();

            using (StreamWriter writer = new StreamWriter(Paths.SUCESSO))
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

        public static void SetReprocessar(string code, string status)
        {
            SetStatus(code, status);

            List<string> pendentes = Pendentes();

            using (StreamWriter writer = new StreamWriter(Paths.PENDENTES))
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
            string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Grupo: {code} /Status: {status}";

            SetInfos(status);

            File.AppendAllText($@"{Paths.STATUS}\status.txt", $@"{line}" + Environment.NewLine);
        }

        public static void SetErrors(string error)
        {
            string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Error: {error}";

            Console.WriteLine(line);

            File.AppendAllText($@"{Paths.ERRORS}\errors.txt", $@"{line}" + Environment.NewLine);

            Mail.Error(error);
        }

        public static void SetInfos(string infos)
        {
            string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Info: {infos}";

            Console.WriteLine(line);

            File.AppendAllText($@"{Paths.INFOS}\infos.txt", $@"{line}" + Environment.NewLine);
        }
    }
}
