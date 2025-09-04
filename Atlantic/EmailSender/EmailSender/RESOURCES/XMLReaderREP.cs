using EmailSender.DAO;
using System;
using System.Xml.Linq;

namespace EmailSender.RESOURCES
{
    public class XMLReaderREP
    {
        XDocument doc = XDocument.Load(@"C:\Users\loure\Desktop\TFS\Atlantic\EmailSender\EmailSender\RESOURCES\ResourceREP.xml");

        public string readConnectionString()
        {
            return doc.Element("emailSender").Element("database").Value;
        }

        public string readSenderEmail()
        {
            return doc.Element("emailSender").Element("EmailCVA").Element("email").Value;
        }

        public string readSenderPassword()
        {
            return doc.Element("emailSender").Element("EmailCVA").Element("password").Value;
        }

        public string readHost()
        {
            return doc.Element("emailSender").Element("conexao").Element("host").Value;
        }

        public string readPort()
        {
            return doc.Element("emailSender").Element("conexao").Element("port").Value;
        }

        public string readSubject()
        {
            return doc.Element("emailSender").Element("modeloEmail").Element("subject").Value;
        }

        public string readBody()
        { 
            var body = doc.Element("emailSender").Element("modeloEmail").Element("body").Value;
            return body;          
        }
    }
}
