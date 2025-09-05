using CVA.Escoteiro.Magento.DAO.Util;
using Escoteiro.Magento.Models;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace CVA.Escoteiro.Magento.BLL
{
    public class CVA_MAGENTO_PARAM_BLL : BaseBLL
    {
        public string DataBase;

        public CVA_MAGENTO_PARAM_BLL()
        {
            DataBase = ConfigurationManager.AppSettings["Database"];
        }

        public async Task<CVA_MAGENTO_PARAM_Model> GetById(string code)
        {
            ServiceLayerUtil sl = new ServiceLayerUtil();

            await sl.Login(DataBase);

            return await sl.GetByIDAsync<CVA_MAGENTO_PARAM_Model>("U_CVA_MAGENTO_PARAM", Convert.ToInt32(code));
        }
    }
}
