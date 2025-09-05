using CVA.Hub.SERVICE.Resource;
using Dover.Framework.DAO;
using System;

namespace CVA.Hub.SERVICE.OITM
{
    public class ItemDAO
    {
        private BusinessOneDAO _BusinessOneDAO { get; set; }

        public ItemDAO(BusinessOneDAO businessOneDAO)
        {
            _BusinessOneDAO = businessOneDAO;
        }
        
        public T GetColumnValue<T>(string columnName, string itemCode)
        {
            return _BusinessOneDAO.ExecuteSqlForObject<T>(String.Format(Query.Item_GetGenericColumn, columnName, itemCode));
        }

        public bool ItemExiste(string itemCode)
        {
            var ret = _BusinessOneDAO.ExecuteSqlForObject<string>($"SELECT ItemCode FROM OITM WHERE ItemCode = '{itemCode}'");
            return !string.IsNullOrEmpty(ret);
        }

        public string GetCentroCusto(string itemCode)
        {
            return _BusinessOneDAO.ExecuteSqlForObject<string>(String.Format(Query.Item_GetCentroCusto, itemCode));
        }

        public string GetItemObs(string itemCode)
        {
            return _BusinessOneDAO.ExecuteSqlForObject<string>(String.Format(Query.Item_GetObs, itemCode));
        }
    }
}
