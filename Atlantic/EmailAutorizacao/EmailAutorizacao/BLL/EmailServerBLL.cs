using System;
using System.Net;
using System.Net.Mail;
using EmailAutorizacao.SERVICE.Portal;

namespace EmailAutorizacao.BLL
{
    public class EmailServerBLL
    {
        public static string SendEmail(MailMessage mail)
        {
            var emailServerModel = EmailServerDAO.GetEmailServer();

            if (emailServerModel != null)
            {
                var service = new SmtpClient
                {
                    Host = emailServerModel.Server,
                    Credentials = new NetworkCredential(emailServerModel.Email, EncryptBLL.Decrypt(emailServerModel.Password, emailServerModel.Email)),
                    Port = emailServerModel.Port,
                    EnableSsl = true
                };
                mail.From = new MailAddress(emailServerModel.Email);

                try
                {
                    service.Send(mail);
                }
                catch (Exception ex)
                {
                    service.Dispose();
                    return "Erro ao enviar e-mail: " + ex.Message;
                }

                service.Dispose();
            }
            else
            {
                return "Configuração de envio de e-mail não encontrada, favor verifique o cadastro no portal";
            }

            return string.Empty;
        }
    }
}
