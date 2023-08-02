
using System.IO;

namespace Bot.ReservaMultiBR.Util
{
    public static class FileHelpers
    {
        public static void CreateDirectorys()
        {
            Console.WriteLine("Criando diretórios...");

            Directory.CreateDirectory(Paths.INFOS);

            Directory.CreateDirectory(Paths.STATUS);

            Directory.CreateDirectory(Paths.ERRORS);

            Directory.CreateDirectory(Paths.INFOS);
        }
        public static List<string> Pendentes()
        {
            String line = string.Empty;

            using (StreamReader reader = new StreamReader(Paths.PENDENTES))
            {
                line = reader.ReadLine();
            }

            SetInfos(".:: 2. Para Processar: " + line);

            return line?.Split(';').ToList();
        }

        public static void SetReprocessar(string code, string status)
        {
            SetStatus(code, status);

            String line = string.Empty;

            using (StreamReader reader = new StreamReader("C:\\bots\\pendentes\\pendentes.txt"))
            {
                line = reader.ReadLine();
            }

            using (StreamWriter writer = new StreamWriter("C:\\bots\\pendentes\\pendentes.txt"))
            {
                line = line.Replace(code + ";", "");

                line += code + ";";

                writer.WriteLine(line);
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
        }

        public static void SetInfos(string infos)
        {
            string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Info: {infos}";

            Console.WriteLine(line);

            File.AppendAllText($@"{Paths.INFOS}\infos.txt", $@"{line}" + Environment.NewLine);
        }
    }
}
