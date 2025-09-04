using CVA.Core.TransportLCM.HELPER;
using CVA.Core.TransportLCM.MODEL.BaseConciliadora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.TransportLCM.SERVICE.BaseConciliadora
{
    public class BaseDeParaDAO
    {
        public BaseDePara GetByCnpj(string cnpj)
        {
            SqlHelper sqlHelper = new SqlHelper(StaticKeys.ConfigConciliadoraModel.Servidor, StaticKeys.ConfigConciliadoraModel.Banco, StaticKeys.ConfigConciliadoraModel.Usuario, StaticKeys.ConfigConciliadoraModel.Senha);
            string query = String.Format(Resources.Query.BaseDePara_GetByCnpj, cnpj);
            BaseDePara model = sqlHelper.FillModel<BaseDePara>(query);

            return model;
        }

        public List<BaseDePara> GetFilialList()
        {
            SqlHelper sqlHelper = new SqlHelper(StaticKeys.ConfigConciliadoraModel.Servidor, StaticKeys.ConfigConciliadoraModel.Banco, StaticKeys.ConfigConciliadoraModel.Usuario, StaticKeys.ConfigConciliadoraModel.Senha);
            List<BaseDePara> list = sqlHelper.FillModelList<BaseDePara>(Resources.Query.BaseDePara_GetAll);
            
            return list;
        }
    }
}
