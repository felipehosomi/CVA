using Newtonsoft.Json;
using Sap.Data.Hana;
using System.Collections.Generic;
using System.Data;

namespace CVA.View.Apetit.IntegracaoWMS.BLL
{
    public static class SAPConnector
    {
        public static bool ConectarSap(out SAPbobsCOM.Company oCompany)
        {
            bool conectado = false;
            oCompany = new SAPbobsCOM.Company();

            oCompany.Server = ParametrosConexao.param.servidor;
            oCompany.DbServerType = ParametrosConexao.param.tipo;

            oCompany.DbUserName = ParametrosConexao.param.usuario;
            oCompany.DbPassword = ParametrosConexao.param.senha;
            oCompany.LicenseServer = ParametrosConexao.param.licenca;

            oCompany.CompanyDB = ParametrosConexao.param.database;
            oCompany.UserName = ParametrosConexao.param.usuarioSAP;
            oCompany.Password = ParametrosConexao.param.senhaSAP;

            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Portuguese_Br;

            int retVal = oCompany.Connect();

            if (retVal != 0)
            {
                string retStr;
                oCompany.GetLastError(out retVal, out retStr);
                throw new System.Exception($"Error Code: {retVal}, mensagem: {retStr}");
            }
            else
            {
                conectado = true;
                return conectado;
            }
        }
    }
}
