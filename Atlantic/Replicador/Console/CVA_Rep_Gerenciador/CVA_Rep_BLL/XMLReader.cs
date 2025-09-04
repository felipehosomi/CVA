using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CVA_Rep_BLL
{
    public class XMLReader
    {
        XDocument doc = XDocument.Load(@"C:\Users\CVA4\Desktop\Desenvolvimento - Não apagar\Replicador\Gerenciador\CVA_Rep_Gerenciador\CVA_RepConfig\Resources\Resource.xml");

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
            return doc.Element("emailSender").Element("modeloEmail").Element("body").Value;
        }
    }
}
