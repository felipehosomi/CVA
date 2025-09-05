using CVA.AddOn.Common.Controllers;
using CVA.View.StockTransfer.DAO;
using CVA.View.StockTransfer.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.StockTransfer.BLL
{
    public class StockTransferBLL
    {
        public string Validate(List<int> baseDocsPO, List<int> baseDocsDelivery)
        {
            CrudController crudController = new CrudController();

            foreach (var baseDoc in baseDocsPO)
            {
                PurchaseOrderModel poModel = crudController.FillModelAccordingToSql<PurchaseOrderModel>(String.Format(Query.StockTransfer_GetByPurchaseOrder, baseDoc));
                if (poModel.ValidateTransfer == "Y" && poModel.StockTransferDocNum == 0)
                {
                    return "Transferência não efetuada para o pedido de venda " + poModel.PurchaseOrderDocNum;
                }
            }
            foreach (var baseDoc in baseDocsDelivery)
            {
                PurchaseOrderModel poModel = crudController.FillModelAccordingToSql<PurchaseOrderModel>(String.Format(Query.StockTransfer_GetByDelivery, baseDoc));
                if (poModel.ValidateTransfer == "Y" && poModel.StockTransferDocNum == 0)
                {
                    return "Transferência não efetuada para o pedido de venda " + poModel.PurchaseOrderDocNum;
                }
            }

            return String.Empty;
        }
    }
}
