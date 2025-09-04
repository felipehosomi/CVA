using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Apetit.IntegracaoWMS.BLL
{
    public class ParametrosConexao
    {
        public static ParametrosConexao param;
        public string connectionString { get; set; }

        public string servidor { get; set; }
        public string usuario { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string senha { get; set; }
        public string usuarioSAP { get; set; }
        public string senhaSAP { get; set; }
        public string database { get; set; }
        public SAPbobsCOM.BoDataServerTypes tipo { get; set; }
        public string licenca { get; set; }
        public bool isSandbox { get; set; }

        public string linkPegasus { get; set; }

        public ParametrosConexao(dynamic _s, bool isWebConfig = false)
        {
            if (isWebConfig)
            {
                var s = _s;
                servidor = s["Servidor"].ToString();
                usuario = s["UsuarioDB"].ToString();
                senha = s["SenhaDB"].ToString();
                usuarioSAP = s["UsuarioSAP"].ToString();
                senhaSAP = s["SenhaSAP"].ToString();
                database = s["Base"].ToString();
                tipo = (SAPbobsCOM.BoDataServerTypes)(int.Parse(s["ServidorTipo"].ToString()));
                licenca = s["ServidorLicenca"].ToString();
                connectionString = s["ConnectioString"].ToString();
                //isSandbox = s["IsSandbox"].ToString() == "true";
                //linkPegasus = isSandbox ? s["LinkPegasus_Sand"].ToString() : s["LinkPegasus_Prod"].ToString();
                //username = s["Username"].ToString();
                //password = s["Password"].ToString();
            }
            else
            {
                servidor = _s.Servidor == null ? "" : _s.Servidor;
                usuario = _s.UsuarioDB == null ? "" : _s.UsuarioDB;
                senha = _s.SenhaDB == null ? "" : _s.SenhaDB;
                usuarioSAP = _s.UsuarioSAP == null ? "" : _s.UsuarioSAP;
                senhaSAP = _s.SenhaSAP == null ? "" : _s.SenhaSAP;
                database = _s.Base == null ? "" : _s.Base;
                tipo = (SAPbobsCOM.BoDataServerTypes)(_s.ServidorTipo != null && _s.ServidorTipo > 0 ? _s.ServidorTipo : 9);
                licenca = _s.ServidorLicenca == null ? "" : _s.ServidorLicenca;
                connectionString = _s.ConnectioString ?? "";
                //isSandbox = _s.IsSandbox == true;
                //linkPegasus = isSandbox ? _s.LinkPegasus_Sand : _s.LinkPegasus_Prod;
                //username = _s.Username;
                //password = _s.Password;
            }
        }
    }
}
