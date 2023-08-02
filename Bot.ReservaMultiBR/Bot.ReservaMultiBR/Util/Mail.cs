using System.Net.Mail;
using System.Net;

namespace Bot.ReservaMultiBR.Util
{
    public static class Mail
    {
        public static void Send()
        {
            try
            {
                MailMessage mail = new MailMessage();

                FileHelpers.SetInfos(".:: 16. Enviar e-mail");

                mail.From = new MailAddress("multibrreservas@gmail.com");
                mail.To.Add("edsongouveiabrito@gmail.com"); // para
                mail.Subject = "Relatório das reservas"; // assunto
                mail.Body = "Testando mensagem de e-mail"; // mensagem

                mail.Attachments.Add(new Attachment(@"C:\Bots\status\status.txt"));

                using (var smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.EnableSsl = true;
                    smtp.Port = 587;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("multibrreservas@gmail.com", "ribgpxyclxarrioq");
                    smtp.Send(mail);

                    FileHelpers.SetInfos(".:: 17. Fim: Enviado com sucesso");
                }
            }
            catch (Exception error)
            {
                FileHelpers.SetErrors(error?.Message);
            }
        }
    }
}
