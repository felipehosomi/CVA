using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FixNFModel
{
    class Program
    {
        static void Main(string[] args)
        {
            var lst = new string[] { "SBODemoBR" };

//            var lst = new string[] { "SBO_CON0001",
//"SBO_CON0002",
//"SBO_PRD_AND0001",
//"SBO_PRD_ATL0001",
//"SBO_PRD_CBE0001",
//"SBO_PRD_CCF0001",
//"SBO_PRD_CFI0001",
//"SBO_PRD_CFI0002",
//"SBO_PRD_CLB0001",
//"SBO_PRD_CLB0002",
//"SBO_PRD_CMO0001",
//"SBO_PRD_CMO0002",
//"SBO_PRD_EUR0002",
//"SBO_PRD_LGB0001",
//"SBO_PRD_LGB0002",
//"SBO_PRD_LGB0003",
//"SBO_PRD_LGB0004",
//"SBO_PRD_LGB0005",
//"SBO_PRD_LGB0006",
//"SBO_PRD_LGB0007",
//"SBO_PRD_LGB0008",
//"SBO_PRD_MOR0001",
//"SBO_PRD_PAE0002",
//"SBO_PRD_REV0001",
//"SBO_PRD_RON0001",
//"SBO_PRD_SER0001",
//"SBO_PRD_SVP0001",
//"SBO_PRD_SVP0002",
//"SBO_PRD_SVP0003",
//"SBO_PRD_SVP0004",
//"SBO_PRD_SVP0005",
//"SBO_PRD_SVP0006",
//"SBO_PRD_SVP0007",
//"SBO_PRD_SVP0008",
//"SBO_PRD_SVP0009",
//"SBO_PRD_SVP0010",
//"SBO_PRD_SVP0011",
//"SBO_PRD_SVP0012",
//"SBO_PRD_SVP0013",
//"SBO_PRD_SVP0014",
//"SBO_PRD_VBR0001",
//"SBO_PRD_VDG0001",
//"SBO_PRD_VDG0002", };

            ////atualizacao PN 02/01 - "SBO_PRD_MOR0001", "SBO_PRD_REV0001", "SBO_PRD_SER0001", "SBO_PRD_VDG0001", "SBO_PRD_CBE0001", "SBO_PRD_CLB0001", "SBO_PRD_CMO0001", "SBO_PRD_PAE0002", "SBO_PRD_SVP0002", "SBO_PRD_SVP0003", "SBO_PRD_SVP0004", "SBO_PRD_SVP0005", "SBO_PRD_SVP0006", "SBO_PRD_SVP0007", "SBO_PRD_SVP0008", "SBO_PRD_SVP0009", "SBO_PRD_SVP0010", "SBO_PRD_SVP0011", "SBO_PRD_SVP0012", "SBO_PRD_SVP0013", "SBO_PRD_VBR0001", "SBO_PRD_CCF0001", "SBO_PRD_VDG0002", "SBO_PRD_CLB0002", "SBO_PRD_LGB0002", "SBO_PRD_SVP0014", "SBO_PRD_LGB0003", "SBO_PRD_CMO0002", "SBO_PRD_LGB0004", "SBO_PRD_LGB0005", "SBO_PRD_LGB0007", "SBO_PRD_LGB0006", "SBO_PRD_LGB0008"
            ///*"SBO_REP_PRE0001", "SBO_REP_PRE0002", "SBO_REP_REA0001", "SBO_REP_REA0002", "SBO_PRD_AND0001", "SBO_PRD_EUR0002", "SBO_PRD_LGB0001", "SBO_PRD_RON0001", "SBO_PRD_SVP0001", "SBO_PRD_CFI0001", "SBO_PRD_CFI0002",*/
            ////"SBO_PRD_MOR0001", "SBO_PRD_REV0001", "SBO_PRD_SER0001", "SBO_PRD_VDG0001", "SBO_PRD_CBE0001", "SBO_PRD_CLB0001", "SBO_PRD_CMO0001", "SBO_PRD_PAE0002", "SBO_PRD_SVP0002", "SBO_PRD_SVP0003", "SBO_PRD_SVP0004", "SBO_PRD_SVP0005", "SBO_PRD_SVP0006", "SBO_PRD_SVP0007", "SBO_PRD_SVP0008", "SBO_PRD_SVP0009", "SBO_PRD_SVP0010", "SBO_PRD_SVP0011", "SBO_PRD_SVP0012", "SBO_PRD_SVP0013", "SBO_PRD_VBR0001", "SBO_PRD_CCF0001", "SBO_PRD_VDG0002", "SBO_PRD_CLB0002", "SBO_PRD_LGB0002", "SBO_PRD_SVP0014", "SBO_PRD_LGB0003",
            try
            {
                foreach (var l in lst)
                {
                    var c = new B1Connection("manager", "manager", l, "FELIPE-PC", false, "sa", "sa@123", BoDataServerTypes.dst_MSSQL2014, "FELIPE-PC");
                    //var c = new B1Connection("manager", "CVA$sap16", l, "localhost:30000", false, "sa", "sa@#Atlantic", BoDataServerTypes.dst_MSSQL2014, "SERVERSAP");
                    Console.WriteLine("Conectando " + l);
                    c.Connect();
                    Console.WriteLine("Conectado " + l);

                    Documents nf = c.oCompany.GetBusinessObject(BoObjectTypes.oInvoices) as Documents;
                    nf.GetByKey(Convert.ToInt32(Console.ReadLine()));
                    Documents cancel = nf.CreateCancellationDocument();
                    if (cancel.Add() != 0)
                    {
                        string error = c.oCompany.GetLastErrorDescription();
                    }

                    Documents pedido = c.oCompany.GetBusinessObject(BoObjectTypes.oOrders) as Documents;
                    pedido.GetByKey(nf.Lines.BaseEntry);
                    if (pedido.Cancel() != 0)
                    {
                        string error = c.oCompany.GetLastErrorDescription();
                    }
                    //Recordset rst = (Recordset)c.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                    ////rst.DoQuery("SELECT DocEntry FROM OINV");
                    //rst.DoQuery("SELECT DocEntry FROM OINV WHERE Model <> 39 AND ISNULL(U_ChaveAcesso, '') <> ''");

                    //Console.WriteLine("Registros encontrados: " + rst.RecordCount);

                    //rst.DoQuery("SELECT DocEntry FROM OINV WHERE Model <> 39");

                    //Console.WriteLine("Model <> 39 - Registros encontrados: " + rst.RecordCount);

                    //rst.DoQuery("SELECT DocEntry FROM OINV WHERE ISNULL(U_ChaveAcesso, '') <> ''");

                    //Console.WriteLine("ISNULL(U_ChaveAcesso, '') <> '' - Registros encontrados: " + rst.RecordCount);

                    //rst.DoQuery("SELECT DocEntry FROM OINV");

                    //Console.WriteLine("Total NF - Registros encontrados: " + rst.RecordCount);

                    //while (!rst.EoF)
                    //{
                    //    var nf = (Documents)c.oCompany.GetBusinessObject(BoObjectTypes.oInvoices);

                    //    nf.GetByKey((int)rst.Fields.Item(0).Value);

                    //    nf.SequenceModel = "39";

                    //    if (nf.Update() != 0)
                    //        Console.WriteLine(c.oCompany.GetLastErrorDescription());

                    //    rst.MoveNext();
                    //}

                    c.oCompany.Disconnect();
                    Console.WriteLine("Desconectado " + l);
                    c.oCompany = null;
                    c = null;
                    GC.Collect();
                    GC.WaitForFullGCApproach();
                    GC.WaitForPendingFinalizers();
                    GC.WaitForFullGCComplete();

                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Fim");
            Console.ReadKey();
        }
    }

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
            oCompany.DbServerType = BoDataServerTypes.dst_MSSQL2014;
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
