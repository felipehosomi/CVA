using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;


namespace CVA.Apetit.Servico.Consolidacao.Class
{
    class Acesso
    {
        static public string Server { get; set; }
        static public string SLDServer { get; set; }
        static public string CompanyDB { get; set; }
        static public string DbUserName { get; set; }
        static public string DbPassword { get; set; }
        static public string UserName { get; set; }
        static public string Password { get; set; }
        static public string Language { get; set; }

        static public string LerConfiguracao()
        {
            string msg = "";

            try
            {
                SLDServer = ConfigurationManager.AppSettings["SLDServer"].ToString();
                CompanyDB = ConfigurationManager.AppSettings["Database"].ToString();
                UserName = ConfigurationManager.AppSettings["B1User"].ToString();
                Password = ConfigurationManager.AppSettings["B1Password"].ToString();
                Server = ConfigurationManager.AppSettings["Server"].ToString();
                DbUserName = ConfigurationManager.AppSettings["DbUserName"].ToString();
                DbPassword = ConfigurationManager.AppSettings["DbPassword"].ToString();
                Language = ConfigurationManager.AppSettings["Language"].ToString().ToUpper();
                if (Language != "PT")
                    Language = "EN";

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
    }
}


