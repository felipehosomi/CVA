using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Control.Logic.DAO.Resources;
using SAPbobsCOM;
using System;

namespace CVA.AddOn.Control.Logic.DAO.OCRD
{
    public class BusinessPartnerDAO
    {
        public string BankAccountExists(string cardCode, string bankCountry, int bankCode, int branch, string account)
        {
            string sql = string.Format(Query.BP_AccountExists, cardCode.Trim(), bankCountry.Trim(), bankCode, branch, account.Trim());
            object pn = CrudController.ExecuteScalar(sql);
            return pn != null ? pn.ToString() : "";
        }

        public string SetDefaultBankAccount(string cardCode, string bankKey, string account, string branch)
        {
            BusinessPartners bp = SBOApp.Company.GetBusinessObject(BoObjectTypes.oBusinessPartners) as BusinessPartners;

            if (bp.GetByKey(cardCode))
            {
                bp.DefaultBankCode = bankKey;
                bp.DefaultAccount = account;
                bp.DefaultBranch = branch;
                
                if (bp.Update() != 0)
                {
                    return SBOApp.Company.GetLastErrorDescription();
                }
            }
            
            return String.Empty;
        }
    }
}
