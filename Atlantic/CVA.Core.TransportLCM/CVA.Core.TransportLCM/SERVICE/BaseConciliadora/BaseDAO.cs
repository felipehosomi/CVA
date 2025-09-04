using CVA.Core.TransportLCM.HELPER;
using CVA.Core.TransportLCM.MODEL.BaseConciliadora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.TransportLCM.SERVICE.BaseConciliadora
{
    public class BaseDAO
    {
        public Base Get(int id)
        {
            SqlHelper sqlHelper = new SqlHelper(StaticKeys.ConfigConciliadoraModel.Servidor, StaticKeys.ConfigConciliadoraModel.Banco, StaticKeys.ConfigConciliadoraModel.Usuario, StaticKeys.ConfigConciliadoraModel.Senha);
            string query = String.Format(Resources.Query.Base_GetById, id);
            Base baseModel = sqlHelper.FillModel<Base>(query);
            return baseModel;
        }
    }
}
