using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Helper
{
    public static class EmailConfirmation
    {
        public static void PasswordResetSendEmail(string email, string subject, string message)
        {

            MailMessage myMessage = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("gönderenin e-postası", "gönderenin şifresi");
            client.Port = 587;
            client.Host = "smtp.live.com";
            client.EnableSsl = true;

            myMessage.To.Add(email);
            myMessage.From = new MailAddress("gönderenin e-postası");
            myMessage.Subject = subject;
            myMessage.IsBodyHtml = true;
            myMessage.Body = message;
            try {
                client.Send(myMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
