using CVA.Escoteiro.Magento.DAO.Util;
using CVA.Escoteiro.Magento.Models.SAP;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace CVA.Escoteiro.Magento.BLL.BLL
{
    public class CVA_OICT : BaseBLL
    {
        public string DataBase;

        public CVA_OICT()
        {
            DataBase = ConfigurationManager.AppSettings["Database"];
        }

        public async Task<string> SetItemCategories(Models.SAP.Value oict)
        {
            var returnList = new List<string>();
            var serviceLayer = new ServiceLayerUtil();
            var retorno = string.Empty;

            await serviceLayer.Login(DataBase);

            returnList = await serviceLayer.PostAsyncReturnList("U_CVA_OICT", oict);

            if (returnList[0] == "NOK")
            {
                throw new Exception("Erro ao gravar dados na tabela CVA_OICT: " + returnList[1]);
            }

            return retorno;
        }
    }
}
