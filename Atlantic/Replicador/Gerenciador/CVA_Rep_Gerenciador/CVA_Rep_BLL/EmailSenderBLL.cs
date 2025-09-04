using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace CVA_Rep_BLL
{
    public class EmailSenderBLL
    {

        XMLReader reader = new XMLReader();

        public string senderEmail;
        public string senderPassword;


        public MailMessage CreateEmail(string newPassword)
        {
            this.senderEmail = reader.readSenderEmail();
            this.senderPassword = Decrypt(reader.readSenderPassword());
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(senderEmail);
            mail.Subject = reader.readSubject();
            mail.Body = reader.readBody() + newPassword;
            mail.IsBodyHtml = true;

            return mail;
        }

        public void SendEmail(string email, MailMessage mail)
        {
            var service = new SmtpClient();
            service.Host = reader.readHost();
            service.Credentials = new NetworkCredential(senderEmail, senderPassword);
            service.Port = Convert.ToInt32(reader.readPort());
            service.EnableSsl = true;

            try
            {
                mail.To.Add(email);
                service.Send(mail);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
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
    }
}
