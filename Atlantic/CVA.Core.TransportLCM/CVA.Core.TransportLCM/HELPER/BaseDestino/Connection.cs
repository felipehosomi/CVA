using CVA.Core.TransportLCM.MODEL.BaseConciliadora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.TransportLCM.HELPER.BaseDestino
{
    public class Connection
    {
        public static SAPbobsCOM.Company _company { get; set; }

        public static string Connect(Base baseModel)
        {
            string msg = String.Empty;
            if (_company == null || _company.CompanyDB != baseModel.BaseName || _company.Connected)
            {
                _company = new SAPbobsCOM.Company();
                _company.DbServerType = (SAPbobsCOM.BoDataServerTypes)baseModel.DBType;
                _company.LicenseServer = baseModel.LicenseServer;
                _company.Server = baseModel.DBServer;
                _company.CompanyDB = baseModel.BaseName;
                _company.UseTrusted = Convert.ToBoolean(baseModel.UseTrusted);
                _company.DbUserName = baseModel.DBUserName;
                _company.DbPassword = baseModel.DBPassword;
                _company.UserName = baseModel.UserName;
                _company.Password = baseModel.Password;
                _company.language = SAPbobsCOM.BoSuppLangs.ln_Portuguese_Br;

                int connected = _company.Connect();
                if (connected != 0)
                {
                    msg = _company.GetLastErrorDescription();
                }
            }
            return msg;
        }
    }
}
