using CVA.Escoteiro.Magento.DAO.Resources;
using CVA.Escoteiro.Magento.DAO.Util;
using Escoteiro.Magento.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace CVA.Escoteiro.Magento.BLL
{
    public class CVA_MAGENTO_DT_LAST_BLL : BaseBLL
    {
        public string DataBase;

        public CVA_MAGENTO_DT_LAST_BLL()
        {
            DataBase = ConfigurationManager.AppSettings["Database"];
        }

        public async Task<CVA_MAGENTO_LAST_DT_Model> GetLastCreateDate(string endpoint)
        {
            ServiceLayerUtil sl = new ServiceLayerUtil();
            string retorno = string.Empty;

            string sql = String.Format(HanaCommands.LastDate_GetCode, DataBase, endpoint);
            object code = DAO.ExecuteScalar(sql);

            await sl.Login(DataBase);

            return await sl.GetByIDAsync<CVA_MAGENTO_LAST_DT_Model>("U_CVA_MAGENTO_DT", (int)code);

        }

        public async Task<string> SaveOrdersLastCreateDate(DateTime data, string endpoint)
        {
            List<string> returnList = new List<string>();
            ServiceLayerUtil sl = new ServiceLayerUtil();
            string retorno = string.Empty;

            string sql = String.Format(HanaCommands.LastDate_GetCode, DataBase, endpoint);
            object code = DAO.ExecuteScalar(sql);

            CVA_MAGENTO_LAST_DT_Model magentoSaveDateLast = new CVA_MAGENTO_LAST_DT_Model();
            magentoSaveDateLast.U_DataCreate = data.ToString("yyyy-MM-dd");
            magentoSaveDateLast.U_HoraCreate = data.ToString("HH:mm");
            magentoSaveDateLast.U_SegundoCreate = data.Second;

            await sl.Login(DataBase);

            if (code == null)
            {
                magentoSaveDateLast.Code = (int)code;
                returnList = await sl.PostAsyncReturnList<CVA_MAGENTO_LAST_DT_Model>("U_CVA_MAGENTO_DT", magentoSaveDateLast);
            }
            else
                returnList = await sl.PatchAsyncReturnList<CVA_MAGENTO_LAST_DT_Model>("U_CVA_MAGENTO_DT", (int)code, magentoSaveDateLast);

            if (returnList[0] == "NOK")
                retorno = "Erro ao gravar data last create magento na tabela CVA_MAGENTO_LAST_DT: " + returnList[1];

            return retorno;
        }

        public async Task<string> SaveOrdersLastUpdateDate(DateTime data, string endpoint)
        {
            List<string> returnList = new List<string>();
            ServiceLayerUtil sl = new ServiceLayerUtil();
            string retorno = string.Empty;

            string sql = String.Format(HanaCommands.LastDate_GetCode, DataBase, "Orders");
            object code = DAO.ExecuteScalar(sql);

            CVA_MAGENTO_LAST_DT_Model magentoSaveDateLast = new CVA_MAGENTO_LAST_DT_Model();
            magentoSaveDateLast.U_DataUpdate = data.ToString("yyyy-MM-dd");
            magentoSaveDateLast.U_HoraUpdate = data.ToString("HH:mm");
            magentoSaveDateLast.U_SegundoUpdate = data.Second;

            await sl.Login(DataBase);

            if (code == null)
            {
                magentoSaveDateLast.Code = (int)code;
                returnList = await sl.PostAsyncReturnList<CVA_MAGENTO_LAST_DT_Model>("U_CVA_MAGENTO_DT", magentoSaveDateLast);
            }
            else
            {
                returnList = await sl.PatchAsyncReturnList<CVA_MAGENTO_LAST_DT_Model>("U_CVA_MAGENTO_DT", (int)code, magentoSaveDateLast);
            }

            if (returnList[0] == "NOK")
                retorno = "Erro ao gravar data last create magento na tabela CVA_MAGENTO_LAST_DT: " + returnList[1];

            return retorno;
        }
    }
}
