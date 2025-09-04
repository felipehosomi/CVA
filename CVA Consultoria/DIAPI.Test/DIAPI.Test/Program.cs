using SAPbobsCOM;
using System;
using System.Configuration;

namespace DIAPI.Test
{
    class Program
    {
        private static readonly string server = ConfigurationManager.AppSettings["Server"];
        private static readonly string companyDB = ConfigurationManager.AppSettings["CompanyDB"];
        private static readonly BoDataServerTypes dbServerType = (BoDataServerTypes)Enum.Parse(typeof(BoDataServerTypes), ConfigurationManager.AppSettings["DbServerType"], true);
        private static readonly string userName = ConfigurationManager.AppSettings["UserName"];
        private static readonly string password = ConfigurationManager.AppSettings["Password"];
        private static readonly string sldServer = ConfigurationManager.AppSettings["SLDServer"];

        static void Main(string[] args)
        {
            try
            {
                var company = new Company();
                company.Server = server;
                company.CompanyDB = companyDB;
                company.DbServerType = dbServerType;
                company.UserName = userName;
                company.Password = password;

                if (!string.IsNullOrWhiteSpace(sldServer))
                {
                    company.SLDServer = sldServer;
                }

                if (company.Connect() == 0)
                {
                    Console.WriteLine($"Conexão bem-sucedida! Versão {company.Version}");
                }
                else
                {
                    company.GetLastError(out int errCode, out string errMsg);
                    Console.WriteLine($"Erro: {errCode} - {errMsg}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}