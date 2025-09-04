using CVA.NF.Import.MODEL;
using SAPbobsCOM;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CVA.NF.Import.DAO
{
    public class SBOConnectionDao
    {
        private static readonly ConciliatorEDM edm = new ConciliatorEDM();
        //public static Company _Company { get; set; }
        public static Dictionary<int, Company> Companies { get; set; } = new Dictionary<int, Company>();

        public static string ConnectToCompany(int companyId, string companyName)
        {
            var baseModel = edm.CVA_BASES.FirstOrDefault(b => b.ID == companyId);
            if (baseModel == null)
            {
                return $"Base de ID {companyId} não encontrada! Por favor verifique o cadastro de parâmetros";
            }

            Company company = new Company();
            company.DbServerType = (BoDataServerTypes)baseModel.DB_TYPE;
            company.Server = baseModel.DB_SERVER;
            company.CompanyDB = baseModel.BASE;
            company.UserName = baseModel.USERNAME;
            company.Password = baseModel.PASSWD;
            company.DbUserName = baseModel.DB_USERNAME;
            company.DbPassword = baseModel.DB_PASSWD;

            if (company.Connect() != 0)
                return company.GetLastErrorDescription();

            Companies.Add(companyId, company);

            return string.Empty;
        }
    }
}
