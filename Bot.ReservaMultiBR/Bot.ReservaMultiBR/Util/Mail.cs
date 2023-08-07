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
                mail.From = new MailAddress("multibrreservas@gmail.com");
                
                mail.To.Add("edsongouveiabrito@gmail.com"); 
                mail.To.Add("brunolousada30@gmail.com"); 
                mail.To.Add("bruno.lousada@multbr.com.br");
                mail.To.Add("patricia@multbr.com.br");
                mail.To.Add("rguedes.patricia@gmail.com"); 
                mail.To.Add("juliana@multbr.com.br"); 
                mail.To.Add("juliaalck@gmail.com "); 
                
                mail.Subject = "[Bot] - Relatório de status das tentativas de reservas"; // assunto
                mail.Body = "Segue em anexo";

                mail.Attachments.Add(new Attachment(@"C:\Bots\status\status.txt"));
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
    }
}
