using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace PLANTIILLA.DEDOMENA.Helpers
{
    public class MailService
    {

        IConfiguration configuration;
        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public bool SendMail(
            string destino, string asunto, string mensaje, string path)
        {
            try
            {
                MailMessage mail = new MailMessage();
                string usermail = configuration["usuariomail"];
                string passwordmail = configuration["passwordmail"];
                mail.From = new MailAddress(usermail);
                mail.To.Add(new MailAddress(destino));
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;
                if (path != null)
                {
                    Attachment att = new Attachment(path);
                    mail.Attachments.Add(att);
                }
                string smtpserver = configuration["host"];
                int port = int.Parse(configuration["port"]);
                bool ssl = bool.Parse(configuration["ssl"]);
                bool defaultcredentials = bool.Parse(configuration["defaultcredentials"]);
                SmtpClient client = new SmtpClient();
                client.Host = smtpserver;
                client.Port = port;
                client.EnableSsl = ssl;
                client.UseDefaultCredentials = defaultcredentials;
                NetworkCredential credentials = new NetworkCredential(usermail, passwordmail);
                client.Credentials = credentials;
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                string m = ex.Message;
                return false;
            }

        }
    }
}
