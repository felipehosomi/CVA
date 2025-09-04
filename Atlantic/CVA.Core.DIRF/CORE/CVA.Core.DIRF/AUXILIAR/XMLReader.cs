using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CVA.Core.DIRF.AUXILIAR
{
    public class XMLReader
    {
        XDocument doc = XDocument.Load(@"C:\CVA Consultoria\Resources\GeraDirf.xml");

        public string readConnectionString()
        {
            return doc.Element("resources").Element("database").Value;
        }
    }
}
