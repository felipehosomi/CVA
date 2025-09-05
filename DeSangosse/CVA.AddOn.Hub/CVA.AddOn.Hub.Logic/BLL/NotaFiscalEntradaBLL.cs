using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.BLL
{
    public class NotaFiscalEntradaBLL
    {
        public bool ValidaTotalDocumentoBase(SAPbobsCOM.BoObjectTypes docTypeBase, List<int> docEntryBase, double docTotal)
        {
            double docTotalBase = 0;
            string sql;
            if (docTypeBase == SAPbobsCOM.BoObjectTypes.oPurchaseOrders)
            {
                sql = String.Format(DAO.Resource.Query.PedidoCompra_GetSumTotal, String.Join(",", docEntryBase));
            }
            else
            {
                sql = String.Format(DAO.Resource.Query.Recebimento_GetSumTotal, String.Join(",", docEntryBase));
            }
            docTotalBase = Convert.ToDouble(Common.Controllers.CrudController.ExecuteScalar(sql));
            if (docTotalBase != docTotal)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
