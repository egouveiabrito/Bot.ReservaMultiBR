using System.IO;

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

            String line = string.Empty;

            using (StreamReader reader = new StreamReader("C:\\bots\\pendentes\\pendentes.txt"))
            {
                line = reader.ReadLine();
            }

            using (StreamWriter writer = new StreamWriter("C:\\bots\\pendentes\\sucesso.txt"))
            {
                line = line.Replace(code + ";", "");

                line += code + ";";

                writer.WriteLine(line);
            }
        }

        public static void SetProcessamentos(string code)
        {

            String line = string.Empty;

            using (StreamReader reader = new StreamReader("C:\\bots\\pendentes\\processamentos.txt"))
            {
                line = reader.ReadLine();
            }

            using (StreamWriter writer = new StreamWriter("C:\\bots\\processamentos\\processamentos.txt"))
            {
                line += DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + "/Grupo:" + code + "\n";

                writer.WriteLine(line);
            }
        }

        public static void SetErrors(string error)
        {
            String line = string.Empty;

            using (StreamReader reader = new StreamReader("C:\\bots\\errors\\error.txt"))
            {
                line = reader.ReadLine();
            }

            using (StreamWriter writer = new StreamWriter("C:\\bots\\errors\\error.txt"))
            {
                line += DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + "/Error:" + error + "\n";

                writer.WriteLine(line);
            }
        }

        public static void SetInfos(string infos)
        {
            String line = string.Empty;

            Console.WriteLine(infos);

            using (StreamReader readers = new StreamReader("C:\\bots\\infos\\infos.txt"))
            {
                line = readers.ReadLine();
            }

            using (StreamWriter writer = new StreamWriter("C:\\bots\\infos\\infos.txt"))
            {
                line += infos;

                writer.WriteLine(line);
            }
        }
    }
}
