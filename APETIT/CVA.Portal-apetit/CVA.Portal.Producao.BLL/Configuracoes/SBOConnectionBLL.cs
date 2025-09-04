using SAPbobsCOM;
using System;
using System.Configuration;

namespace CVA.Portal.Producao.BLL.Configuracoes
{
    public class SBOConnectionBLL
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

        public static void ConnectToCompany()
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
                    throw new Exception(company.GetLastErrorDescription());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao conectar na DI: " + ex.Message);
            }
        }
    }
}
