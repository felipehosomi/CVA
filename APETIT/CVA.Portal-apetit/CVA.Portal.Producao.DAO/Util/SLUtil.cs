using B1SLayer;

namespace CVA.Portal.Producao.DAO.Util
{
    public sealed class SLUtil
    {
        private static readonly SLConnection slConnection = new SLConnection(
            System.Configuration.ConfigurationManager.AppSettings["ServiceLayerURL"],
            System.Configuration.ConfigurationManager.AppSettings["Database"],
            System.Configuration.ConfigurationManager.AppSettings["B1User"],
            System.Configuration.ConfigurationManager.AppSettings["B1Password"], 29);

        static SLUtil() { }

        private SLUtil() { }

        public static SLConnection Connection { get { return slConnection; } }
    }
}
