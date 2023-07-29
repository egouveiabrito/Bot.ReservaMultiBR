
namespace Bot.ReservaMultiBR.Util
{
    public static class FileHelpers
    {
        public static void CreateDirectorys()
        {
            Console.WriteLine("Criando diretórios...");

            Directory.CreateDirectory(Paths.INFOS);

            Directory.CreateDirectory(Paths.PROCESSAMENTOS);

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

            Console.WriteLine("Para Processar:" + line);

            return line?.Split(';').ToList();
        }

        public static void SetReprocessar(string code)
        {
            SetProcessamentos(code);

            String line = string.Empty;

            using (StreamReader reader = new StreamReader(Paths.PENDENTES))
            {
                line = reader.ReadLine();
            }

            // remover do inicio da fila
            line = line.Replace(code + ";", "");

            // jogar no final da fila
            line += code + ";";

            File.AppendAllText($@"{Paths.PENDENTES}", $@"{line}" + Environment.NewLine);
        }

        public static void SetSucesso(string code)
        {

            SetProcessamentos(code);

            string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Sucesso: {code}";

            Console.WriteLine(line);

            File.AppendAllText($@"{Paths.INFOS}\sucesso.txt", $@"{line}" + Environment.NewLine);
        }

        public static void SetProcessamentos(string code)
        {
            string line = DateTime.Now.ToString("dd/MM/yyyy HH:ss:mm").ToString() + $"/Grupo: {code}";

            Console.WriteLine(line);

            File.AppendAllText($@"{Paths.PROCESSAMENTOS}\processamentos.txt", $@"{line}" + Environment.NewLine);
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
