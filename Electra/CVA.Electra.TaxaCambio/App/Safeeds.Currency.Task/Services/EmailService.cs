using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Electra.Currency.Task.Services
{
    public static class EmailService
    {
        public static void SeendingMail(string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(CurrencyTaskHelper._configurationRepository.GetConfigurationValue<string>("SmtpServer"));

                mail.From = new MailAddress(CurrencyTaskHelper._configurationRepository.GetConfigurationValue<string>("FromMail"));
                mail.To.Add(CurrencyTaskHelper._configurationRepository.GetConfigurationValue<string>("ToMail"));
                mail.Subject = $"Erro na atualização taxa de câmbio - data: {DateTime.Today.ToString("dd/MM/yyyy")}";
                mail.Body = body;

                SmtpServer.Port = Convert.ToInt32(CurrencyTaskHelper._configurationRepository.GetConfigurationValue<string>("PortMail"));
                SmtpServer.Credentials = new System.Net.NetworkCredential(CurrencyTaskHelper._configurationRepository.GetConfigurationValue<string>("UserMail"), CurrencyTaskHelper._configurationRepository.GetConfigurationValue<string>("PasswordMail"));
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
