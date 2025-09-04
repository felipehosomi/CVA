using SAPbobsCOM;
using System;
using System.Configuration;

namespace ResetWarehouseOnHand.Controllers
{
    internal class SBOController
    {
        public static Company Company;

        public SBOController()
        {
        }

        public static void Connect()
        {
            try
            {
                Company = new Company();
                Company.LicenseServer = "";
                Company.CompanyDB = ConfigurationManager.AppSettings["CompanyDB"].ToString();
                Company.UserName = ConfigurationManager.AppSettings["UserName"].ToString();
                Company.Password = ConfigurationManager.AppSettings["Password"].ToString();
                Company.Server = ConfigurationManager.AppSettings["Server"].ToString();
                Company.DbUserName = ConfigurationManager.AppSettings["DbUserName"].ToString();
                Company.DbPassword = ConfigurationManager.AppSettings["DbPassword"].ToString();
                Company.UseTrusted = true;
                Company.DbServerType = BoDataServerTypes.dst_HANADB;
                Company.language = BoSuppLangs.ln_Portuguese_Br;

                if (Company.Connect() != 0)
                {
                    throw new Exception(Company.GetLastErrorDescription());
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}