using System;

namespace EnvioNfLote.Helpers
{
    public class Mailer
    {
        public static void SendMail(string emailPadrao, string emailsCopia, string chaveAcesso, string caminhoXml, string caminhoDanfe, string pn, string numeroNF, string empresa)
        {
            try
            {
                var conf = DI.BuscaConf();

                var pdf = $@"{caminhoDanfe}\NFe{chaveAcesso}.pdf";

                var xml = $@"{caminhoXml}\NFe{chaveAcesso}.xml";

                System.Net.NetworkCredential credentials;

                credentials = new System.Net.NetworkCredential("cva", "cva@2017");

                var message = new System.Net.Mail.MailMessage();
                message.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnSuccess;
                message.Headers.Add("Return-Receipt-To", conf.Value.EmailCopia);
                message.Headers.Add("Disposition-Notification-To", conf.Value.EmailCopia);
                message.To.Add(emailPadrao);

                var splitEmails = emailsCopia.Split(';');

                foreach (var item in splitEmails)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        message.CC.Add(item);
                    }
                }

                message.Bcc.Add(conf.Value.EmailCopia);
                message.Subject = $"{conf.Value.Subject} - {pn} - NF: {numeroNF} - {empresa}";
                message.From = new System.Net.Mail.MailAddress(conf.Value.Email);

                message.Body = conf.Value.Mensagem;
                message.IsBodyHtml = false;
                
                if (!string.IsNullOrEmpty(caminhoDanfe))
                {
                    using (new NetworkConnection(caminhoDanfe, credentials))
                    {
                        if (System.IO.File.Exists(pdf))
                            message.Attachments.Add(new System.Net.Mail.Attachment(pdf, "application/pdf"));
                        else
                            throw new Exception($"DANFE não encontrado no diretório {caminhoDanfe}");
                    }                
                }

                if (!string.IsNullOrEmpty(caminhoXml))
                {
                    using (new NetworkConnection(caminhoXml, credentials))
                    {
                        if (System.IO.File.Exists(xml))
                            message.Attachments.Add(new System.Net.Mail.Attachment(xml, "application/xml"));
                        else
                            throw new Exception($"XML não encontrado no diretório {caminhoDanfe}");
                    }                
                }

                var smtp = new System.Net.Mail.SmtpClient(conf.Value.Smtp, conf.Value.Porta);
                var credential = new System.Net.NetworkCredential(conf.Value.Usuario, conf.Value.Senha);
                smtp.EnableSsl = conf.Value.Ssl.Equals("Y");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Timeout = 100000000;                
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                System.Net.ServicePointManager.Expect100Continue = false;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;

                var T1 = new System.Threading.Thread(() => smtp.Send(message));
                T1.Start();

                System.Threading.Thread.Sleep(1);

                T1.Join();

                message.Dispose();
                smtp.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
