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
    public class MedidaItemBLL
    {
        public MedidasItemModel GetPesoBrutoItem(string ItemCode)
        {
            CrudController crudController = new CrudController();
            MedidasItemModel MedidaItem = crudController.FillModelAccordingToSql<MedidasItemModel>(String.Format(Query.MedidasItem, ItemCode));
            return MedidaItem;
        }
    }
}
