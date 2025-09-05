using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electra.Currency.BLL
{
    public class ConnectionBLL
    {
        private static Company company;

        public static Company Company
        {
            get
            {
                if (company == null)
                {
                    ConnectToCompany();
                }
                return company;
            }
        }

        public static string ConnectToCompany()
        {
            try
            {
                company = new Company();
                company.DbServerType = (BoDataServerTypes)Convert.ToInt32(ConfigurationManager.AppSettings["ServerType"]);
                company.Server = ConfigurationManager.AppSettings["Server"];
                company.CompanyDB = ConfigurationManager.AppSettings["Database"];
                company.UseTrusted = false;
                company.DbUserName = ConfigurationManager.AppSettings["DBUser"];
                company.DbPassword = ConfigurationManager.AppSettings["DBPassword"];
                company.UserName = ConfigurationManager.AppSettings["B1User"];
                company.Password = ConfigurationManager.AppSettings["B1Password"];
                company.language = BoSuppLangs.ln_Portuguese_Br;

                if (company.Connect() != 0)
                {
                    return company.GetLastErrorDescription();
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                return "Erro geral ao conectar na DI: " + ex.Message;
            }
        }
    }
}
