using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteSql
{
    class Program
    {
        static void Main(string[] args)
        {
            //var lst = new string[] { "SBODemoBR", "SBODemoUS" };

            var lst = new string[] { "SBO_CON0001",
                                    "SBO_CON0002",
                                    "SBO_PRD_AND0001",
                                    "SBO_PRD_ATL0001",
                                    "SBO_PRD_CBE0001",
                                    "SBO_PRD_CCF0001",
                                    "SBO_PRD_CFI0001",
                                    "SBO_PRD_CFI0002",
                                    "SBO_PRD_CLB0001",
                                    "SBO_PRD_CLB0002",
                                    "SBO_PRD_CMO0001",
                                    "SBO_PRD_CMO0002",
                                    "SBO_PRD_EUR0002",
                                    "SBO_PRD_LGB0001",
                                    "SBO_PRD_LGB0002",
                                    "SBO_PRD_LGB0003",
                                    "SBO_PRD_LGB0004",
                                    "SBO_PRD_LGB0005",
                                    "SBO_PRD_LGB0006",
                                    "SBO_PRD_LGB0007",
                                    "SBO_PRD_LGB0008",
                                    "SBO_PRD_MOR0001",
                                    "SBO_PRD_PAE0002",
                                    "SBO_PRD_REV0001",
                                    "SBO_PRD_RON0001",
                                    "SBO_PRD_SER0001",
                                    "SBO_PRD_SVP0001",
                                    "SBO_PRD_SVP0002",
                                    "SBO_PRD_SVP0003",
                                    "SBO_PRD_SVP0004",
                                    "SBO_PRD_SVP0005",
                                    "SBO_PRD_SVP0006",
                                    "SBO_PRD_SVP0007",
                                    "SBO_PRD_SVP0008",
                                    "SBO_PRD_SVP0009",
                                    "SBO_PRD_SVP0010",
                                    "SBO_PRD_SVP0011",
                                    "SBO_PRD_SVP0012",
                                    "SBO_PRD_SVP0013",
                                    "SBO_PRD_SVP0014",
                                    "SBO_PRD_VBR0001",
                                    "SBO_PRD_VDG0001",
                                    "SBO_PRD_VDG0002", };

            try
            {
                StreamReader sr = new StreamReader("c:\\temp\\ODOC_PayableReceivable.sql");
                string sql = sr.ReadToEnd();
                sr.Close();

                foreach (var l in lst)
                {
                    SqlHelper sqlHelper = new SqlHelper("SERVERSAP", l, "sa", "sa@#Atlantic");

                    Console.WriteLine("Conectando " + l);
                    sqlHelper.Connect();

                    Console.WriteLine("Executando");
                    sqlHelper.ExecuteNonQuery(sql);
                    sqlHelper.Close();
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
}
