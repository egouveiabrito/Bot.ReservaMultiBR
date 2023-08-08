using System.Net.Mail;
using System.Net;

namespace Bot.ReservaMultiBR.Util
{
    public static class Mail
    {
        private static List<string> EMAILS = new List<string>();
        public static void Send(string code)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("multibrreservas@gmail.com");

                Email();

                #if DEBUG
                    mail.To.Add("edsongouveiabrito@gmail.com");
                #else
                    foreach (string email in EMAILS) mail.To.Add(email);
                #endif

                mail.Subject = $"[Bot] - Reserva realizada com sucesso: {code}"; 

                mail.Body = $"Olá, reserva realizada com sucesso: {code}";

                using (var smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("multibrreservas@gmail.com", "ribgpxyclxarrioq");
                    smtp.Send(mail);
                }
            }
            catch (Exception error)
            {
                FileHelpers.SetErrors(error?.Message);
            }
        }

        public static void Log(string message)
        {

            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("multibrreservas@gmail.com");
                mail.To.Add("edsongouveiabrito@gmail.com");
                mail.Subject = "[Bot] - Log";
                mail.Body = message;
                using (var smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("multibrreservas@gmail.com", "ribgpxyclxarrioq");
                    smtp.Send(mail);
                }
            }
            catch (Exception error)
            {
                FileHelpers.SetErrors(error?.Message);
            }
        }

        private static void Email()
        {
            try
            {
                string[] lines = File.ReadAllLines(Paths.EMAIL);

                for (int index = 0; index < lines.Count(); index++)
                {
                    if (index == 0) continue;

                    string[] columns = lines[index].Split(',');

                    foreach (string column in columns)
                    {
                        EMAILS.Add(column.Trim());
                    }
                }
            }
            catch
            {
                FileHelpers.SetErrors(".:: Erro ao obter os email de confirmação. Favor verificar o arquivo config.csv");

                EMAILS.Add("edsongouveiabrito@gmail.com");
                EMAILS.Add("brunolousada30@gmail.com");
                EMAILS.Add("bruno.lousada@multbr.com.br");
                EMAILS.Add("patricia@multbr.com.br");
                EMAILS.Add("rguedes.patricia@gmail.com");
                EMAILS.Add("juliana@multbr.com.br");
                EMAILS.Add("juliaalck@gmail.com");
            }
        }
    }
}
