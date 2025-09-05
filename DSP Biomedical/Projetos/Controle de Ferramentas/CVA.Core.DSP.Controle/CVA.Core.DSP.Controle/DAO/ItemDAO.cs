using CVA.Core.DSP.Controle.MODEL;
using CVA.Core.DSP.Controle.Resources.Select;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DSP.Controle.DAO
{
    public partial class ItemDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }
    }

    public partial class ItemDAO
    {
        public ItemDAO(BusinessOneDAO businessOneDAO)
        {
            _businessOneDAO = businessOneDAO;
        }

        public OITM GetCounter(string itemCode)
        {
            try
            {
                var query = String.Format(Query.GetCounterToolByItem, itemCode);
                return _businessOneDAO.ExecuteSqlForObject<OITM>(query);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
