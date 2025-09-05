using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using CVA.AddOn.Hub.Logic.MODEL;
using System;

namespace CVA.Hub.DAO.OITM
{
    public class ItemDAO
    {
        private CrudController _CrudController { get; set; }

        public ItemDAO()
        {
            _CrudController = new CrudController();
        }
        
        public T GetColumnValue<T>(string columnName, string itemCode)
        {
            return CrudController.GetColumnValue<T>(String.Format(Query.Item_GetGenericColumn, columnName, itemCode));
        }

        public bool ItemExiste(string itemCode)
        {
            var ret = CrudController.GetColumnValue<string>(String.Format(Query.Item_Exists, itemCode));
            return !string.IsNullOrEmpty(ret);
        }

        public DocumentItemModel GetCentroCusto(string itemCode)
        {
            return new CrudController().FillModelAccordingToSql<DocumentItemModel>(String.Format(Query.Item_GetCentroCusto, itemCode));
        }

        public string GetItemObs(string itemCode)
        {
            return CrudController.GetColumnValue<string>(String.Format(Query.Item_GetObs, itemCode));
        }
    }
}
