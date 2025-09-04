using CVA.Core.ControlCommon.SERVICE.OCRD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.ControlCommon.BLL
{
    public class BusinessPartnerBLL
    {
        private BusinessPartnerDAO _businessPartnerDAO { get; set; }

        public BusinessPartnerBLL(BusinessPartnerDAO businessPartnerDAO)
        {
            this._businessPartnerDAO = businessPartnerDAO;
        }

        public string BankAccountExists(string cardCode, string bankCountry, int bankCode, int branch, string account)
        {
            return this._businessPartnerDAO.BankAccountExists(cardCode, bankCountry, bankCode, branch, account);
        }
    }
}
