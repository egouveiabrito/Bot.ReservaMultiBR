using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace Bot.ReservaMultiBR.Util
{
    public static class FileHelpers
    {
        public static List<string> Pendentes()
        {
            String line = string.Empty;

            using (StreamReader reader = new StreamReader("C:\\bots\\pendentes\\pendentes.txt"))
            {
                line = reader.ReadLine();
            }

            return line?.Split(';').ToList();
        }

        public static void SetReprocessar(string code)
        {
            SetProcessamentos(code);

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

        public static void SetSucesso(string code)
        {
            SetProcessamentos(code);

            string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Sucesso: {code}" + Environment.NewLine;

            File.AppendAllText("C:\\bots\\sucesso\\sucesso.txt", line);
        }

        public static void SetProcessamentos(string code)
        {
            string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Grupo: {code}" + Environment.NewLine;

            File.AppendAllText("C:\\bots\\processamentos\\processamentos.txt", line);
        }

        public static void SetErrors(string error)
        {
            string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Errors: {error}" + Environment.NewLine;

            File.AppendAllText("C:\\bots\\errors\\error.txt", line);
        }

        public static void SetInfos(string infos)
        {
            string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Info: {infos}" + Environment.NewLine;

            File.AppendAllText("C:\\bots\\infos\\infos.txt", line);
        }
    }
}
