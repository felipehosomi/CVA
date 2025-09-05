using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Monitor.WBC.Excel.Files
{
    public class ParametrosConexao
    {

        public string servidor { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }
        public string database { get; set; }
        public SAPbobsCOM.BoDataServerTypes tipo { get; set; }
        public string licenca { get; set; }

        public ParametrosConexao()
        {

            servidor = Properties.Settings.Default.Servidor == null ? "" : Properties.Settings.Default.Servidor;
            usuario = Properties.Settings.Default.UsuarioDB == null ? "" : Properties.Settings.Default.UsuarioDB;
            senha = Properties.Settings.Default.SenhaDB == null ? "" : Properties.Settings.Default.SenhaDB;
            database = Properties.Settings.Default.Base == null ? "" : Properties.Settings.Default.Base;
            tipo = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
            licenca = Properties.Settings.Default.ServidorLicenca == null ? "" : Properties.Settings.Default.ServidorLicenca;

        }

        public override string ToString()
        {

            string stringDB = "";
            stringDB = "application name=Integrador;data source={0};initial catalog={1};password={2};persist security info=True;user id={3};packet size=8192";
            stringDB = string.Format(stringDB, servidor, database, senha, usuario);
            return stringDB;

        }

    }
}
