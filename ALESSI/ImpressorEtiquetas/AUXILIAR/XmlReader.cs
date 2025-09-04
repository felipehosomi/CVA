using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AUXILIAR
{
    public class XMLReader
    {
        XDocument doc = XDocument.Load(@"C:\CVA Consultoria\Impressor de Etiqueta\ImpressorEtiqueta.xml");

        public string ReadConnectionString()
        {
            return doc.Element("alessi").Element("connection").Value;
        }

        public string ReadServerName()
        {
            return doc.Element("alessi").Element("serverName").Value;
        }

        public string ReadDatabaseName()
        {
            return doc.Element("alessi").Element("databaseName").Value;
        }

        public string ReadUserName()
        {
            return doc.Element("alessi").Element("userName").Value;
        }

        public string ReadPassword()
        {
            return doc.Element("alessi").Element("password").Value;
        }
    }
}
