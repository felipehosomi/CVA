using CVA.AddOn.Common.Controllers;
using CVA.View.Hybel.DAO.Resources;
using CVA.View.Hybel.MODEL;
using System;
using System.Net;
using System.Net.Mail;

namespace CVA.View.Hybel.BLL
{
    public class EmailBLL
    {
        public static string SendEmail(string subject, string message, string email)
        {
            string msg = String.Empty;
            try
            {
                CrudController crudController = new CrudController();
                ConfiguracaoEmailModel emailConfigModel = new CrudController().FillModelAccordingToSql<ConfiguracaoEmailModel>(SQL.ConfiguracaoEmail_Get);
                if (emailConfigModel == null)
                {
                    return "Configuração de envio de e-mail não encontrada, favor verifique o cadastro";
                }

                MailMessage mail = new MailMessage();

                mail.To.Add(email);

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;

                msg = SendEmail(mail, emailConfigModel);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        public static string SendEmail(MailMessage mail, ConfiguracaoEmailModel emailModel)
        {
            var service = new SmtpClient();
            service.Host = emailModel.Servidor;
            service.Credentials = new NetworkCredential(emailModel.Email, EncryptController.Decrypt(emailModel.Senha, emailModel.Email));
            service.Port = emailModel.Porta;
            service.EnableSsl = emailModel.SSL == "Y";

            mail.From = new MailAddress(emailModel.Email);

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
