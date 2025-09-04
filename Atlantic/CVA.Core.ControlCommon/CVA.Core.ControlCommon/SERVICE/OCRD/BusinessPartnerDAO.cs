using CVA.Core.ControlCommon.SERVICE.Resource;
using Dover.Framework.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.SERVICE.OCRD
{
    public class BusinessPartnerDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }

        public BusinessPartnerDAO(BusinessOneDAO businessOneDAO)
        {
            this._businessOneDAO = businessOneDAO;
        }

        public string BankAccountExists(string cardCode, string bankCountry, int bankCode, int branch, string account)
        {
            string sql = string.Format(Query.BP_AccountExists, cardCode, bankCountry, bankCode, branch, account);
            return this._businessOneDAO.ExecuteSqlForObject<string>(sql);
        }
    }
}
