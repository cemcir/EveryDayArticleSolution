﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;

namespace EveryDayArticle.Web.Helper
{
    public class PasswordReset
    {
        public static void PasswordResetSendEmail(string email, string subject, string message) {
            
            MailMessage myMessage = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("gönderenin e-posta adresi", "gönderenin e-posta şifresi");
            client.Port = 587;
            client.Host = "smtp.live.com";
            client.EnableSsl = true;

            myMessage.To.Add(email);
            myMessage.From = new MailAddress("gönderenin e-posta adresi");
            myMessage.Subject = subject;
            myMessage.IsBodyHtml = true;
            myMessage.Body = message;
            try {
                client.Send(myMessage);
            }
            catch (Exception ex) {
                throw ex;
            }           
        }
    }
}
