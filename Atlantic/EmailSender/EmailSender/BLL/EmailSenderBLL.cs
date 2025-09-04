using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using EmailSender.DAO;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using EmailSender.RESOURCES;
using System.Diagnostics;
using Quartz;
using Quartz.Impl;

namespace EmailSender.BLL
{

    public class EmailSenderBLL
    {
        EmailDAO _emailDAO = new EmailDAO();
        ReplicadorDAO _replicadorDAO = new ReplicadorDAO();
        ConsolidadorDAO _consolidadorDAO = new ConsolidadorDAO();

        public string senderEmail;
        public string senderPassword;

        #region REP
        XMLReaderREP readerREP = new XMLReaderREP();
        public int flowControllerREP = 5;

        public void CheckREP()
        {
            if (flowControllerREP == 5)
                CheckForErrorsREP();
            if (flowControllerREP == 2)
                CheckForCorrectionsREP();
        }
        
        public void CheckForErrorsREP()
        {
            if (_replicadorDAO.CheckStatus() == 5)
                {
                    SendEmail(CreateEmail("R"));
                    flowControllerREP = 2;
                }
        }

        public void CheckForCorrectionsREP()
        {
            if (_replicadorDAO.CheckStatus() == 2)
                {
                flowControllerREP = 5;
                }
        }

        #endregion

        #region CON
        XMLReaderCON readerCON = new XMLReaderCON();
        public int flowControllerCON = 2;

        public void CheckCON()
        {
            if (flowControllerCON == 2)
                CheckForErrorsCON();
            if (flowControllerCON == 3)
                CheckForCorrectionsCON();
        }

        public void CheckForErrorsCON()
        {
            if (_consolidadorDAO.CheckStatus() == 2)
            {
                SendEmail(CreateEmail("C"));
                flowControllerCON = 3;
            }
        }

        public void CheckForCorrectionsCON()
        {
            if (_consolidadorDAO.CheckStatus() == 3)
            {
                flowControllerCON = 2;
            }
        }

        #endregion

        public MailMessage CreateEmail(string process)
        {
            MailMessage mail = new MailMessage();
            if (process.Equals("R"))
            {
                this.senderEmail = readerREP.readSenderEmail();
                this.senderPassword = Decrypt(readerREP.readSenderPassword());                

                mail.From = new MailAddress(senderEmail);
                mail.Subject = readerREP.readSubject();

                var aux = _replicadorDAO.ReadLog();

                mail.Body = String.Format(readerREP.readBody(), aux[0].ToString(), aux[1].ToString(), aux[2].ToString(), aux[3].ToString());
                mail.IsBodyHtml = true;
            }
            else
            {
                this.senderEmail = readerCON.readSenderEmail();
                this.senderPassword = Decrypt(readerCON.readSenderPassword());

                mail.From = new MailAddress(senderEmail);
                mail.Subject = readerCON.readSubject();

                var aux = _consolidadorDAO.ReadLog();

                mail.Body = String.Format(readerCON.readBody(), aux[0].ToString(), aux[1].ToString(), aux[2].ToString());
                mail.IsBodyHtml = true;
            }

            return mail;
        }

        private void SendEmail(MailMessage mail)
        {
            var service = new SmtpClient();
            service.Host = readerREP.readHost();
            service.Credentials = new NetworkCredential(senderEmail, senderPassword);
            service.Port = Convert.ToInt32(readerREP.readPort());
            service.EnableSsl = true;

            List<string> emails = _emailDAO.GetEmails();
            try
            {
                foreach (var e in emails)
                    mail.To.Add(e);
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
