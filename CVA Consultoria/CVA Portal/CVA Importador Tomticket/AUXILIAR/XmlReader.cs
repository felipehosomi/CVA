using System.Xml.Linq;

namespace AUXILIAR
{
    public class XmlReader
    {
        XDocument doc = XDocument.Load(@"C:\CVA Consultoria\CVA Importador Tomticket\Resource.xml");

        public string readConnectionString()
        {
            return doc.Element("importador").Element("database").Value;
        }

        public string readTokenA1()
        {
            return doc.Element("importador").Element("tokenA1").Value;
        }

        public string readTokenB1()
        {
            return doc.Element("importador").Element("tokenB1").Value;
        }
    }
}