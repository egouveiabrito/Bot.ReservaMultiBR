namespace Bot.ReservaMultiBR.Util
{
    public static class Shared
    {
        public static void Inicializar()
        {
            Random rand = new Random();

            for (int i = 0; i < 50; i++)
            {
                for (int j = 1; j <= 7; j++)
                {
                    for (int s = 1; s <= 8; s++)
                    {
                        if (rand.Next(10) > 5)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        }
                         
                        Console.Write(" MULT BR ");
                    }
                    if (j != 7)
                    {
                        Console.Write("    ");
                    }
                }
            }

        }
        public static string Avisos()
        {
            Random random = new Random();

            int number = random.Next(1, 10);

            switch (number)
            {
                case 1:
                    return "Dicas: Não deixe o mesmo número de grupo em maquinas diferentes processando." + Environment.NewLine;

                case 2:
                    return "Dicas: Feche todas as coisas do seu computador antes de deixar o BOT rodando." + Environment.NewLine;

                case 3:
                    return "Dicas: Caso não tenha recebido o e-mail de reservas olhe na sua caixa de span." + Environment.NewLine;

                case 4:
                    return "Dicas: É possivel olhar todo os resultados de sucesso na planilha sucesso.csv \n na pasta sucesso." + Environment.NewLine;

                case 5:
                    return "Dicas: É possivel adicionar mais e-mails de recebimento de confirmações de \n reserva na pasta config no arquivo config.csv." + Environment.NewLine;

                case 6:
                    return "Dicas: O Bot é inteligênte para identificar o erro registrar, pular para o \n próximo processamento em caso de falhas." + Environment.NewLine;

                default:
                    return "Dicas: As pastas de instalação: Bot, sucesso, pendentes, infos, erros não podem ser excluídas" + Environment.NewLine;
            }
        }
    }

}
