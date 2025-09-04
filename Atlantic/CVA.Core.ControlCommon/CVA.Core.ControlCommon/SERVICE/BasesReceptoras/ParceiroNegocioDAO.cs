using CVA.Core.ControlCommon.HELPER;
using CVA.Core.ControlCommon.MODEL.CVACommon;
using CVA.Core.ControlCommon.SERVICE.BaseReplicadora.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.SERVICE.BasesReceptoras
{
    public class ParceiroNegocioDAO
    {
        public object ValidaParceiroNegocio(Base baseModel, string cardCode)
        {
            string sql = String.Format(Select.ValidateBusinessPartner, cardCode);
            SqlHelper sqlHelper = new SqlHelper(baseModel.SRVR, baseModel.COMP, baseModel.DB_UNAME, baseModel.DB_PAS);
            return sqlHelper.ExecuteScalar(sql);
        }
    }
}
