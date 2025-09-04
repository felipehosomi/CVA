using System;
using SAPbouiCOM;
using Company = SAPbobsCOM.Company;
using DAL.Data;
using System.Linq;
using System.Runtime.InteropServices;
using SAPbobsCOM;
using DAL.DataInterface;

namespace DAL.Connection
{
    public class ConnectionDao
    {
        private static readonly Lazy<ConnectionDao> _instance = new Lazy<ConnectionDao>(() => new ConnectionDao());
        public Company Company { get; private set; }
        public Application Application { get; private set; }
        public EventFilters Filters { get; private set; }
        public static ConnectionDao Instance { get { return _instance.Value; } }

        public static Company ExternalCompany { get; private set; }

        private ConnectionDao()
        {
            Connect();
        }

        private void Connect()
        {
            SetApplication();
            Company = (Company)Application.Company.GetDICompany();
            Filters = new EventFilters();
        }

        public static string ConnectExternal(BaseModel baseModel)
        {
            if (ExternalCompany != null && ExternalCompany.Connected && ExternalCompany.CompanyDB == baseModel.BASE)
            {
                return string.Empty;
            }

            if (ExternalCompany != null)
            {
                /*trello 656 - João claudino INI*/
                if (ExternalCompany.Connected)
                {
                    ExternalCompany.Disconnect();
                }
                /*trello 656 - João claudino FIM*/
                Marshal.ReleaseComObject(ExternalCompany);
                ExternalCompany = null;
            }

            ExternalCompany = new Company();
            ExternalCompany.DbServerType = (BoDataServerTypes)baseModel.DB_TYPE;
            ExternalCompany.Server = baseModel.DB_SERVER;
            ExternalCompany.CompanyDB = baseModel.BASE;
            ExternalCompany.UserName = baseModel.USERNAME;
            ExternalCompany.Password = baseModel.PASSWD;
            ExternalCompany.DbUserName = baseModel.DB_USERNAME;
            ExternalCompany.DbPassword = baseModel.DB_PASSWD;

            if (ExternalCompany.Connect() != 0)
            {
                return ExternalCompany.GetLastErrorDescription();
            }
            return string.Empty;
        }

        private void SetApplication()
        {
            var sboGuiApi = new SboGuiApi();
            var str = Environment.GetCommandLineArgs().GetValue(1).ToString();
            sboGuiApi.Connect(Environment.GetCommandLineArgs().GetValue(1).ToString());
            Application = sboGuiApi.GetApplication();
        }
    }
}
