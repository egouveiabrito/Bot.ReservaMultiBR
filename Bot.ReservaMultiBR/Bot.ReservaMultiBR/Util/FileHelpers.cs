
using System.IO;

namespace Bot.ReservaMultiBR.Util
{
    public static class FileHelpers
    {
        public static void CreateDirectorys()
        {

            Directory.CreateDirectory(Paths.INFOS);

            Directory.CreateDirectory(Paths.STATUS);

            Directory.CreateDirectory(Paths.ERRORS);

            Directory.CreateDirectory(Paths.INFOS);
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

                String line = string.Empty;

                line = code + ";";
                
                writer.WriteLine(line);

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

            Mail.Log(error);
        }

        public static void SetInfos(string infos)
        {
            string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Info: {infos}";

            Console.WriteLine(line);

            File.AppendAllText($@"{Paths.INFOS}\infos.txt", $@"{line}" + Environment.NewLine);
        }
    }
}
