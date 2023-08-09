using System.Net.Mail;
using System.Net;

namespace Bot.ReservaMultiBR.Util
{
    public static class Mail
    {
        private static List<string> EMAILS = new List<string>();
        public static void Reserva(string code, AdministradoraEnum administradora)
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

                mail.IsBodyHtml = true;
                mail.Subject = $"[Bot] MULT BR - Reserva realizada com sucesso: {code}"; 
                mail.Body = FileHelpers.TemplateReserva(code, administradora);

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

        public static void Error(string message)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("multibrreservas@gmail.com");
                mail.To.Add("edsongouveiabrito@gmail.com");
                mail.Subject = $"[Bot] MULT BR - Erro";
                mail.Body = FileHelpers.TemplateInfo(message, "ERROR");
                mail.IsBodyHtml = true;

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


        public static void Info(string message)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress("multibrreservas@gmail.com");
                
                #if DEBUG
                    mail.To.Add("edsongouveiabrito@gmail.com");
                #else
                    mail.To.Add("edsongouveiabrito@gmail.com");
                    mail.To.Add("rguedes.patricia@gmail.com");
                #endif
               
                mail.Subject = $"[Bot] MULT BR - INFO";
                mail.Body = FileHelpers.TemplateInfo(message, "INFO");
                mail.IsBodyHtml = true;

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
