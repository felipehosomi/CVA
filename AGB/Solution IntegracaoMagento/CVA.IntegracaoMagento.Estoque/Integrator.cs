using ServiceLayerHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CVA.IntegracaoMagento.Estoque
{
    public class Integrator
    {
        internal static SLConnection SLConnection;
        //internal static IntegrationConfig Config;
        private static readonly string ServiceLayerURL = ConfigurationManager.AppSettings["ServiceLayerURL"];
        internal static readonly string Database = ConfigurationManager.AppSettings["Database"];
        private static readonly string B1User = ConfigurationManager.AppSettings["B1User"];
        private static readonly string B1Password = ConfigurationManager.AppSettings["B1Password"];

        internal static string sTokenMagento;
        private static readonly string MagentoURL = ConfigurationManager.AppSettings["MagentoURL"];
        private static readonly string MagentoUser = ConfigurationManager.AppSettings["MagentoUser"];
        private static readonly string MagentoPassword = ConfigurationManager.AppSettings["MagentoPassword"];
    }
}
