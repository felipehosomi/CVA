using CVA.Tust.Import.ConciliatorDAO;
using SAPbobsCOM;
using System.Linq;
using System.Runtime.InteropServices;

namespace CVA.Tust.Import.DAO
{
    public class SBOConnectionDao
    {
        private static readonly ConciliatorEDM edm = new ConciliatorEDM();
        public static Company _Company { get; set; }

        public static string ConnectToCompany(int companyId, string companyName)
        {
            if ((_Company != null) && _Company.Connected && (_Company.CompanyName == companyName))
                return string.Empty;
            var baseModel = edm.CVA_BASES.FirstOrDefault(b => b.ID == companyId);
            if (baseModel == null)
            {
                return $"Base de ID {companyId} não encontrada! Por favor verifique o cadastro de parâmetros";
            }

            if (_Company != null)
            {
                Marshal.ReleaseComObject(_Company);
                _Company = null;
            }

            _Company = new Company();
            _Company.DbServerType = (BoDataServerTypes)baseModel.DB_TYPE;
            _Company.Server = baseModel.DB_SERVER;
            _Company.CompanyDB = baseModel.BASE;
            _Company.UserName = baseModel.USERNAME;
            _Company.Password = baseModel.PASSWD;
            _Company.DbUserName = baseModel.DB_USERNAME;
            _Company.DbPassword = baseModel.DB_PASSWD;

            if (_Company.Connect() != 0)
                return _Company.GetLastErrorDescription();
            return string.Empty;
        }

        public static string ConnectToCompany(string companyName)
        {
            if ((_Company != null) && _Company.Connected && (_Company.CompanyName == companyName))
                return string.Empty;
            var baseModel = edm.CVA_BASES.FirstOrDefault(b => b.BASE == companyName);
            if (baseModel == null)
            {
                return $"Base {companyName} não encontrada! Por favor verifique o cadastro de parâmetros";
            }

            if (_Company != null)
            {
                Marshal.ReleaseComObject(_Company);
                _Company = null;
            }

            _Company = new Company();
            _Company.DbServerType = (BoDataServerTypes)baseModel.DB_TYPE;
            _Company.Server = baseModel.DB_SERVER;
            _Company.CompanyDB = baseModel.BASE;
            _Company.UserName = baseModel.USERNAME;
            _Company.Password = baseModel.PASSWD;
            _Company.DbUserName = baseModel.DB_USERNAME;
            _Company.DbPassword = baseModel.DB_PASSWD;

            if (_Company.Connect() != 0)
                return _Company.GetLastErrorDescription();
            return string.Empty;
        }
    }
}
