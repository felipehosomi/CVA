using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.Model.Estoque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Estoque
{
    public class TransferenciaLogBLL : BaseBLL
    {
        public TransferenciaLogBLL()
        {
            DAO.TableName = "@CVA_PED_TRANSF_OP";
        }

        public TransferenciaLogModel GetValidTransfer(int opDocNum, int stageId)
        {
            var model = DAO.FillModel<TransferenciaLogModel>(string.Format(Commands.Resource.GetString("PedidoTransferencia_GetByOP"), BaseBLL.Database, opDocNum, stageId));
            return model;
        }

        public void Create(TransferenciaLogModel model)
        {
            DAO.Model = model;
            DAO.CreateModel();
        }
    }
}