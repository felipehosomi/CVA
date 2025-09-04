using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace CVA.Core.CriadorDeCampos
{
    internal static class Connect
    {
        internal static Company ConnectToCompany(Connection conn)
        {
            var Company = new Company()
            {
                Server = conn.DbServer,
                CompanyDB = conn.CompanyDb,
                UserName = conn.Username,
                Password = conn.Password,
                language = BoSuppLangs.ln_Portuguese_Br,
                DbUserName = conn.DbUsername,
                DbPassword = conn.DbPassword,
                DbServerType = (BoDataServerTypes)Enum.Parse(typeof(BoDataServerTypes), string.Format("dst_{0}", conn.DbServerType)),
                UseTrusted = false,
                LicenseServer = conn.LicenseServer
            };

            if (Company.Connect() != 0)
                throw new Exception(Company.GetLastErrorDescription());

            return Company;
        }
    }
}
