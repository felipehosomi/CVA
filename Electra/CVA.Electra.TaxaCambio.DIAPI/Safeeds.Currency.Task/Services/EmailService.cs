using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Electra.Currency.Task.Services
{
    public static class EmailService
    {
        public static void SendingMail(string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["SmtpServer"]);

                mail.From = new MailAddress(ConfigurationManager.AppSettings["FromMail"]);
                mail.To.Add(ConfigurationManager.AppSettings["ToMail"]);
                mail.Subject = $"Erro na atualização taxa de câmbio - data: {DateTime.Today.ToString("dd/MM/yyyy")}";
                mail.Body = body;

                SmtpServer.Port = Convert.ToInt32(ConfigurationManager.AppSettings["PortMail"]);
                SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["UserMail"], ConfigurationManager.AppSettings["PasswordMail"]);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                LogService.GravarException($"Erro ao enviar email {ex.Message}");
            }
        }
    }
}
