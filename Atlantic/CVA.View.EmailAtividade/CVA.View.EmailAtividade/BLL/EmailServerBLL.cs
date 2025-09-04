using CVA.View.EmailAtividade.MODEL;
using System;
using System.Net;
using System.Net.Mail;

namespace CVA.View.EmailAtividade.BLL
{
    public class EmailServerBLL
    {
        public static string SendEmail(MailMessage mail, EmailConfigModel emailServerModel)
        {
            var service = new SmtpClient();
            service.Host = emailServerModel.Server;
            service.Credentials = new NetworkCredential(emailServerModel.User, EncryptBLL.Decrypt(emailServerModel.Password, emailServerModel.User));
            service.Port = emailServerModel.Port;
            service.EnableSsl = emailServerModel.SSL == "Y";

            mail.From = new MailAddress(emailServerModel.User);

            try
            {
                service.Send(mail);
            }
            catch (Exception ex)
            {
                return "Erro ao enviar e-mail: " + ex.Message;
            }

            return String.Empty;
        }
    }
}
