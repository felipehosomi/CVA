using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyDocuments
{
    public class B1Connection
    {
        public B1Connection()
        {
            oCompany = new Company();
        }

        public B1Connection(string username, string password, string companyDB, string licenseServer, bool useTrusted,
            string dbUsername, string dbPassword, BoDataServerTypes dbServerType, string serverAddress)
        {
            oCompany = new Company();
            Username = username;
            Password = password;
            CompanyDB = companyDB;
            LicenseServer = licenseServer;
            UseTrusted = useTrusted;
            DbUsername = dbUsername;
            DbPassword = dbPassword;
            DbServerType = dbServerType;
            ServerAddress = serverAddress;
        }

        public Company oCompany { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string CompanyDB { get; set; }
        public bool UseTrusted { get; set; }
        public string ServerAddress { get; set; }
        public string DbPassword { get; set; }
        public string DbUsername { get; set; }
        public string LicenseServer { get; set; }
        public BoDataServerTypes DbServerType { get; set; }

        public Company Connect()
        {
            oCompany.UserName = Username;
            oCompany.Password = Password;
            oCompany.CompanyDB = CompanyDB;
            oCompany.LicenseServer = LicenseServer;
            oCompany.UseTrusted = false;
            oCompany.DbUserName = DbUsername;
            oCompany.DbPassword = DbPassword;
            oCompany.DbServerType = DbServerType;
            oCompany.Server = ServerAddress;

            if (oCompany.Connect() != 0)
            {
                int errCode;
                string errMsg;

                oCompany.GetLastError(out errCode, out errMsg);

                throw new Exception($"{errCode} - {errMsg}");
            }

            return oCompany;
        }
    }
}
