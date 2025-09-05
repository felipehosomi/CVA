using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conferencia.ConexaoSAP
{
    public class ParametrosConexao
    {

        public string servidor { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }
        public string databaseWebApi { get; set; }
        public string databaseSAP { get; set; }
        public SAPbobsCOM.BoDataServerTypes tipo { get; set; }
        public string licenca { get; set; }

        public ParametrosConexao()
        {
            

            servidor = Properties.Settings.Default.Servidor == null ? "" : Properties.Settings.Default.Servidor;
            usuario = Properties.Settings.Default.UsuarioDB == null ? "" : Properties.Settings.Default.UsuarioDB;
            senha = Properties.Settings.Default.SenhaDB == null ? "" : Properties.Settings.Default.SenhaDB;
            databaseSAP = Properties.Settings.Default.BaseSAP == null ? "" : Properties.Settings.Default.BaseSAP;
            tipo = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
            licenca = Properties.Settings.Default.ServidorLicenca == null ? "" : Properties.Settings.Default.ServidorLicenca;

            //servidor = Properties.Settings.Default.Servidor == null ? "" : Properties.Settings.Default.Servidor;
            //usuario = Properties.Settings.Default.UsuarioDB == null ? "" : Properties.Settings.Default.UsuarioDB;
            //senha = Properties.Settings.Default.SenhaDB == null ? "" : Properties.Settings.Default.SenhaDB;
            ////databaseWebApi = Properties.Settings.Default.BaseWebApi == null ? "" : Properties.Settings.Default.BaseWebApi;
            //databaseSAP = Properties.Settings.Default.BaseSAP == null ? "" : Properties.Settings.Default.BaseSAP;
            //tipo = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
            //licenca = Properties.Settings.Default.ServidorLicenca == null ? "" : Properties.Settings.Default.ServidorLicenca;

        }

        public override string ToString()
        {

            string stringDB = "";
            stringDB = "application name=Integrador;data source={0};initial catalog={1};password={2};persist security info=True;user id={3};packet size=8192";
            stringDB = string.Format(stringDB, servidor, senha, usuario, databaseSAP);
            return stringDB;

        }
    }
}
