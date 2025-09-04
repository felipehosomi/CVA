using CVA.View.EmailAutorizacao.MODEL;
using CVA.View.EmailAutorizacao.SERVICE.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.EmailAutorizacao.BLL
{
    public class EmailServerBLL
    {
        EmailServerDAO EmailServerDAO { get; }

        public EmailServerBLL(EmailServerDAO emailServerDAO)
        {
            EmailServerDAO = emailServerDAO;
        }

        public string SendEmail(MailMessage mail)
        {
            EmailServerModel emailServerModel = EmailServerDAO.GetEmailServer();

            if (emailServerModel != null)
            {
                var service = new SmtpClient();
                service.Host = emailServerModel.Server;
                service.Credentials = new NetworkCredential(emailServerModel.Email, EncryptBLL.Decrypt(emailServerModel.Password, emailServerModel.Email));
                service.Port = emailServerModel.Port;
                service.EnableSsl = true;

                mail.From = new MailAddress(emailServerModel.Email);

                try
                {
                    service.Send(mail);
                }
                catch (Exception ex)
                {
                    return "Erro ao enviar e-mail: " + ex.Message;
                }
            }
            else
            {
                return "Configuração de envio de e-mail não encontrada, favor verifique o cadastro no portal";
            }

            return String.Empty;
        }
    }
}
