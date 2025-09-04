using AUXILIAR;
using MODEL.Classes;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Classes
{
    public class EmailSenderBLL
    {
        #region Atributos
        public string senderEmail;
        public string senderPassword;
        Helper _helper { get; set; }
        #endregion

        #region Construtor
        public EmailSenderBLL()
        {
            this._helper = new Helper();
        }
        #endregion

        public MessageModel SendEmail(string emailAddress)
        {
            if (SendEmail(CreateEmail(emailAddress), emailAddress) == 0)
                return MessageBLL.Generate("Senha enviada por email com sucesso", 0);
            else
                return MessageBLL.Generate("Ocorreu um erro ao recuperar sua senha. Verifique o email informado", 99); ;
        }

        public MailMessage CreateEmail(string emailAddress)
        {
            this.senderEmail = _helper.readSenderEmail();
            this.senderPassword = Decrypt(_helper.readSenderPassword());
            MailMessage mail = new MailMessage();

            var senhaUsuario = _helper.ReadPassword(emailAddress);

            if (senhaUsuario.Equals(""))
                return null;
            else
                senhaUsuario = DecryptUserPassword(senhaUsuario, emailAddress);

            mail.From = new MailAddress(senderEmail);
            mail.Subject = _helper.readSubject();
            mail.Body = String.Format(_helper.readBody(), senhaUsuario, emailAddress);
            mail.IsBodyHtml = true;

            return mail;
        }

        public int SendEmail(MailMessage mail, string emailAddress)
        {
            var service = new SmtpClient();
            service.Host = _helper.readHost();
            service.Credentials = new NetworkCredential(senderEmail, senderPassword);
            service.Port = Convert.ToInt32(_helper.readPort());
            service.EnableSsl = true;

            try
            {
                mail.To.Add(emailAddress);
                service.Send(mail);
                return 0;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return 1;
            }
        }


        public void NotifyUser(string emailAddress)
        {
            SendNotification(CreateNotification(emailAddress), emailAddress);
        }

        public MailMessage CreateNotification(string emailAddress)
        {
            this.senderEmail = _helper.readSenderEmail();
            this.senderPassword = Decrypt(_helper.readSenderPassword());
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(senderEmail);
            mail.Subject = "[PORTAL CVA] Seu primeiro acesso";
            mail.Body = String.Format(_helper.readNotificationBody(), emailAddress);
            mail.IsBodyHtml = true;

            return mail;
        }

        public int SendNotification(MailMessage mail, string emailAddress)
        {
            var service = new SmtpClient();
            service.Host = _helper.readHost();
            service.Credentials = new NetworkCredential(senderEmail, senderPassword);
            service.Port = Convert.ToInt32(_helper.readPort());
            service.EnableSsl = true;

            try
            {
                mail.To.Add(emailAddress);
                service.Send(mail);
                return 0;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return 1;
            }
        }

        //Descriptografa a senha salva no arquivo XML
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        //Descriptografa a senha do usuário no banco
        public string DecryptUserPassword(string cipherText, string passPhrase)
        {
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(256 / 8).ToArray();
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(256 / 8).Take(256 / 8).ToArray();
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((256 / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((256 / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, 1000))
            {
                var keyBytes = password.GetBytes(256 / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }
    }
}