using CVA.View.ControleQualidade.MODEL;
using CVA.View.ControleQualidade.Resources.Query;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.ControleQualidade.DAO
{
    public class ItemDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }

        public ItemDAO(BusinessOneDAO businessOneDAO)
        {
            _businessOneDAO = businessOneDAO;
        }

        public Item GetCodigoInspecaoPorItem(string itemCode)
        {
            try
            {
                var query = String.Format(Select.GetInspecaoPorItem, itemCode);
                return _businessOneDAO.ExecuteSqlForObject<Item>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
