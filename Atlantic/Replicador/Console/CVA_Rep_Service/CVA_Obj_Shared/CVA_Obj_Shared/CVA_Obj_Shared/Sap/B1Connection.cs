using CVA_Rep_Exception;
using SAPbobsCOM;
using System;

namespace CVA_Obj_Shared.Sap
{
    public class B1Connection
    {
        private static readonly Lazy<B1Connection> _instance = new Lazy<B1Connection>(() => new B1Connection());

        private B1Connection()
        {
            oCompany = new Company();
        }

        public Company oCompany { get; }

        public Company Connect(string username, string password, string companyDB, string licenseServer, bool useTrusted,
            string dbUsername, string dbPassword, BoDataServerTypes dbServerType, string serverAddress)
        {
            oCompany.UserName = username;
            oCompany.Password = password;
            oCompany.CompanyDB = companyDB;
            oCompany.LicenseServer = licenseServer;
            oCompany.UseTrusted = useTrusted;
            oCompany.DbUserName = dbUsername;
            oCompany.DbPassword = dbPassword;
            oCompany.DbServerType = dbServerType;
            oCompany.Server = serverAddress;                        

            if (oCompany.Connect() != 0)
            {
                int errCode;
                string errMsg;

                oCompany.GetLastError(out errCode, out errMsg);

                throw new ReplicadorException($"{errCode} - {errMsg}");
            }            

            return oCompany;
        }

        public static B1Connection Instance
        {
            get { return _instance.Value; }
        }
    }
}