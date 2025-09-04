using CVA.Portal.Producao.DAO;
using CVA.Portal.Producao.Model.Configuracoes;
using System;

namespace CVA.Portal.Producao.BLL
{
    public class BaseBLL
    {
        public IDAO DAO { get; set; }
        public static string Database = System.Configuration.ConfigurationManager.AppSettings["Database"];

        public BaseBLL()
        {
            if (!Static.Config.ServerType.HasValue)
            {
                int serverType;
                if (Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["ServerType"], out serverType))
                {
                    Static.Config.ServerType = (SAPbobsCOM.BoDataServerTypes)serverType;
                }
            }

            if (Static.Config.ServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
            {
                DAO = new HanaDAO();
            }
            else
            {
                DAO = new SqlDAO();
            }
        }
    }
}
