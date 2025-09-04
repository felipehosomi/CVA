using CVA.Portal.Producao.DAO.Resources;
using CVA.Portal.Producao.DAO.Util;
using CVA.Portal.Producao.Model;
using CVA.Portal.Producao.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Portal.Producao.BLL.Apetit
{
    public class BatchesBLL : BaseBLL
    {
        public List<BatchesModel> GetItemBatches(string itemCode, string whs)
        {
            try
            {
                return DAO.FillListFromCommand<BatchesModel>(string.Format(Commands.Resource.GetString("Batches_FIFO"), Database, itemCode, whs));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public BatchesControlModel BatchControl(string itemCode)
        {
            try
            {
                return DAO.FillModelFromCommand<BatchesControlModel>(string.Format(Commands.Resource.GetString("Batches_Control"), Database, itemCode));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
