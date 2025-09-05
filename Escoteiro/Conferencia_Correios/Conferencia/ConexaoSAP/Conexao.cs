using SAPbobsCOM;

namespace Conferencia.ConexaoSAP
{
    class Conexao
    {
        public ParametrosSAP ps;
        public ParametrosConexao pc = new ParametrosConexao();
        
        public static Company oCompany;
        public static string ErrMsg { get; set; }
        public static int RetCode { get; set; }
        private SAPbobsCOM.Recordset oRecordSet;

        public Conexao()
        {
            int retVal;
            string retStr;
            //var xml = new XMLReader();

            //this.aLog = new Log();

            ps.Company =  Properties.Settings.Default.BaseSAP;
            ps.Usuario = Properties.Settings.Default.UsuarioSAP;
            ps.Senha = Properties.Settings.Default.SenhaSAP;
            

            pc.databaseSAP = Properties.Settings.Default.BaseSAP;
            pc.usuario = Properties.Settings.Default.UsuarioDB;
            pc.senha = Properties.Settings.Default.SenhaDB;
            pc.servidor = Properties.Settings.Default.Servidor;
            pc.tipo = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
            pc.licenca = Properties.Settings.Default.ServidorLicenca;

            if (ConectarSap(ps, pc, out retVal, out retStr, out oCompany))
            {
                //LogInfo("Conexão SAP realizada com sucesso em " + oCompany.CompanyDB + " como " + oCompany.UserName);

                #region Campo Usuario
                //if (xml.CriaUDF() == "S")
                //{
                //    AdcionarCampo("ORDR", "CVA_Token", "Token E-Commerce", BoFieldTypes.db_Alpha, BoFldSubTypes.st_None, 35);
                //    AdcionarCampoComboBoxFormulario("OINV", "CVA_STatus_NF", "Status de Envio");
                //}

                #endregion

                //var Integrador = new IntegradorController();
            }
            else
            {
                //retStr;

                //Me("Falha na conexão SAP: " + retVal + " - " + retStr);
            }

            RetCode = retVal;
            ErrMsg = retStr;
        }

        public bool ConectarSap(ParametrosSAP ps, ParametrosConexao pc, out int retVal, out string retStr, out SAPbobsCOM.Company oCompany)
        {

            bool conectado = false;
            oCompany = new SAPbobsCOM.Company();
            retVal = 0;
            retStr = "";

            oCompany.Server = pc.servidor;
            oCompany.DbServerType = pc.tipo;

            oCompany.DbUserName = pc.usuario;
            oCompany.DbPassword = pc.senha;
            oCompany.LicenseServer = pc.licenca;

            oCompany.CompanyDB = ps.Company;
            oCompany.UserName = ps.Usuario;
            oCompany.Password = ps.Senha;
            
            
            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Portuguese_Br;

            retVal = oCompany.Connect();

            if (retVal != 0)
            {
                oCompany.GetLastError(out retVal, out retStr);
                return conectado;
            }
            else
            {
                conectado = true;
                return conectado;
            }

        }
    }
}
