using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Console.Tust.InsertSkillData
{
    class Program
    {
        static void Main(string[] args)
        {
            //var lst = new string[] { "SBODemoBR" };
            
            var lst = new string[] { "SBO_PRD_SVP0001", "SBO_PRD_SVP0002", "SBO_PRD_SVP0003", "SBO_PRD_SVP0004", "SBO_PRD_SVP0005", "SBO_PRD_SVP0006", "SBO_PRD_SVP0007", "SBO_PRD_SVP0008", "SBO_PRD_SVP0009", "SBO_PRD_SVP0010", "SBO_PRD_SVP0011", "SBO_PRD_SVP0012", "SBO_PRD_SVP0013", "SBO_PRD_SVP0014" };

            ////atualizacao PN 02/01 - "SBO_PRD_MOR0001", "SBO_PRD_REV0001", "SBO_PRD_SER0001", "SBO_PRD_VDG0001", "SBO_PRD_CBE0001", "SBO_PRD_CLB0001", "SBO_PRD_CMO0001", "SBO_PRD_PAE0002", "SBO_PRD_SVP0002", "SBO_PRD_SVP0003", "SBO_PRD_SVP0004", "SBO_PRD_SVP0005", "SBO_PRD_SVP0006", "SBO_PRD_SVP0007", "SBO_PRD_SVP0008", "SBO_PRD_SVP0009", "SBO_PRD_SVP0010", "SBO_PRD_SVP0011", "SBO_PRD_SVP0012", "SBO_PRD_SVP0013", "SBO_PRD_VBR0001", "SBO_PRD_CCF0001", "SBO_PRD_VDG0002", "SBO_PRD_CLB0002", "SBO_PRD_LGB0002", "SBO_PRD_SVP0014", "SBO_PRD_LGB0003", "SBO_PRD_CMO0002", "SBO_PRD_LGB0004", "SBO_PRD_LGB0005", "SBO_PRD_LGB0007", "SBO_PRD_LGB0006", "SBO_PRD_LGB0008"
            ///*/*"SBO_REP_PRE0001", "SBO_REP_PRE0002", "SBO_REP_REA0001", "SBO_REP_REA0002", "SBO_PRD_AND0001", "SBO_PRD_EUR0002", "SBO_PRD_LGB0001", "SBO_PRD_RON0001", "SBO_PRD_SVP0001", "SBO_PRD_CFI0001", "SBO_PRD_CFI0002",*/
            ////"SBO_PRD_MOR0001", "SBO_PRD_REV0001", "SBO_PRD_SER0001", "SBO_PRD_VDG0001", "SBO_PRD_CBE0001", "SBO_PRD_CLB0001", "SBO_PRD_CMO0001", "SBO_PRD_PAE0002", "SBO_PRD_SVP0002", "SBO_PRD_SVP0003", "SBO_PRD_SVP0004", "SBO_PRD_SVP0005", "SBO_PRD_SVP0006", "SBO_PRD_SVP0007", "SBO_PRD_SVP0008", "SBO_PRD_SVP0009", "SBO_PRD_SVP0010", "SBO_PRD_SVP0011", "SBO_PRD_SVP0012", "SBO_PRD_SVP0013", "SBO_PRD_VBR0001", "SBO_PRD_CCF0001", "SBO_PRD_VDG0002", "SBO_PRD_CLB0002", "SBO_PRD_LGB0002", "SBO_PRD_SVP0014", "SBO_PRD_LGB0003",
            try
            {
                foreach (var l in lst)
                {
                    var c = new B1Connection("manager", "CVA$sap16", l, "SERVERSAP:30000", false, "sa", "sa@#Atlantic", BoDataServerTypes.dst_MSSQL2014, "SERVERSAP");
                    //var c = new B1Connection("manager", "manager", l, "localhost:30000", false, "sa", "sa@123", BoDataServerTypes.dst_MSSQL2014, "FELIPE-PC");
                    System.Console.WriteLine("Conectando " + l);
                    c.Connect();

                    Recordset rst = (Recordset)c.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                    rst.DoQuery("SELECT ISNULL(MAX(CAST(Code AS INTEGER)), 0) FROM [@SKILL_INF_ITEM_DOC]");
                    int lastCode = Convert.ToInt32(rst.Fields.Item(0).Value);

                    string sql = @"SELECT OPCH.DocEntry,  PAR.ITEMCODE, PAR.NAT_BC, PAR.REG_PC, PAR.IND_OPER, PAR.ORIG_CRED, PAR.DESC_DOC FROM OPCH
	                                    INNER JOIN OBPL
		                                    ON OBPL.BPlId = OPCH.BPlId
	                                    INNER JOIN [CVA_ATL_PORTAL].[dbo].CVA_PAR_TUST PAR
		                                    ON PAR.CNPJ = OBPL.TaxIdNum COLLATE DATABASE_DEFAULT
                                    WHERE Comments LIKE 'TUST%'
                                    AND NOT EXISTS
                                    (
	                                    SELECT TOP 1 1 FROM [@SKILL_INF_ITEM_DOC]
	                                    WHERE U_SKILL_DOCUMENTO = 'OPCH'
	                                    AND U_SKILL_DOCENTRY = OPCH.DocEntry
                                    )";

                    rst.DoQuery(sql);

                    while (!rst.EoF)
                    {
                        lastCode++;
                        UserTable table = c.oCompany.UserTables.Item("SKILL_INF_ITEM_DOC");
                        table.Code = lastCode.ToString();
                        table.Name = lastCode.ToString();
                        table.UserFields.Fields.Item("U_SKILL_DOCUMENTO").Value = "OPCH";
                        table.UserFields.Fields.Item("U_SKILL_DOCENTRY").Value = (int)rst.Fields.Item("DocEntry").Value;
                        table.UserFields.Fields.Item("U_SKILL_LINHA").Value = 0;
                        table.UserFields.Fields.Item("U_SKILL_ITEMCODE").Value = rst.Fields.Item("ItemCode").Value.ToString();
                        table.UserFields.Fields.Item("U_SKILL_NAT_BC_CRED").Value = rst.Fields.Item("NAT_BC").Value.ToString();
                        table.UserFields.Fields.Item("U_RegPC").Value = rst.Fields.Item("REG_PC").Value.ToString();
                        table.UserFields.Fields.Item("U_IND_OPER").Value = rst.Fields.Item("IND_OPER").Value.ToString();
                        table.UserFields.Fields.Item("U_IND_ORIG_CRED").Value = rst.Fields.Item("ORIG_CRED").Value.ToString();
                        table.UserFields.Fields.Item("U_DESC_DOC_OPER").Value = rst.Fields.Item("DESC_DOC").Value.ToString();
                        if (table.Add() != 0)
                        {
                            System.Console.WriteLine("Erro ao adicionar: " + c.oCompany.GetLastErrorDescription());
                        }

                        rst.MoveNext();
                    }

                    System.Console.WriteLine("Conectado " + l);
                    c.oCompany.Disconnect();
                    System.Console.WriteLine("Desconectado " + l);
                    c.oCompany = null;
                    c = null;
                    GC.Collect();
                    GC.WaitForFullGCApproach();
                    GC.WaitForPendingFinalizers();
                    GC.WaitForFullGCComplete();

                    System.Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            System.Console.WriteLine("Fim");
            System.Console.ReadKey();
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
