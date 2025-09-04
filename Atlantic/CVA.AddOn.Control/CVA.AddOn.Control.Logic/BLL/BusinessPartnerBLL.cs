using CVA.AddOn.Control.Logic.DAO.OCRD;
using System;

namespace CVA.AddOn.Control.Logic.BLL
{
    public class BusinessPartnerBLL
    {
        private BusinessPartnerDAO _businessPartnerDAO { get; set; }

        public BusinessPartnerBLL()
        {
            this._businessPartnerDAO = new BusinessPartnerDAO();
        }

        public string BankAccountExists(string cardCode, string bankCountry, int bankCode, int branch, string account)
        {
            return this._businessPartnerDAO.BankAccountExists(cardCode, bankCountry, bankCode, branch, account);
        }

        public string SetDefaultBankAccount(string cardCode, string bankKey, string account, string branch)
        {
            return this._businessPartnerDAO.SetDefaultBankAccount(cardCode, bankKey, account, branch);
        }
    }
}
