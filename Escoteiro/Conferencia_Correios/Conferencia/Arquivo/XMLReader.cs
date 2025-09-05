using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Conferencia.Arquivo
{
    public class XMLReader
    {
        #region Atributos

        XDocument doc = XDocument.Load(@"C:\CVA Consultoria\Relatórios\Config.xml");

        #endregion


        public string Server()
        {
            return doc.Element("DataBase").Element("Server").Value;
        }
        public string UserID()
        {
            return doc.Element("DataBase").Element("UserID").Value;
        }
        public string Password()
        {
            return doc.Element("DataBase").Element("Password").Value;
        }
        public string Base()
        {
            return doc.Element("DataBase").Element("Base").Value;
        }
    }
}
