using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CVA.PreenchimentoDeLote.Auxiliar
{
    public class XMLReader
    {
        XDocument doc = XDocument.Load(@"C:\CVA Consultoria\Resources.xml");
        
        public string readConnectionString()
        {
            return doc.Element("resources").Element("database").Value;
        }


    }
}

