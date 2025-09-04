using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SAPbobsCOM;
using SAPbouiCOM;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using SAPbouiCOM.Framework;

namespace CVA.Apetit.Addon.Class
{
    public class Conexao
    {
        public static SAPbobsCOM.Company oCompany;
        public static SqlConnection SqlConn = new SqlConnection();
        public static SqlConnection SqlConnDR = new SqlConnection();
        public static string sConnectionContext = null;
        public static SAPbouiCOM.Application sbo_application = SAPbouiCOM.Framework.Application.SBO_Application;

        public Conexao()
        {
            try
            {
                this.SetApplicationDI();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void SetApplicationDI()
        {
            try
            {
                int num;
                oCompany = (SAPbobsCOM.Company)SAPbouiCOM.Framework.Application.SBO_Application.Company.GetDICompany();
                string errMsg = "";
                oCompany.GetLastError(out num, out errMsg);
                if (num != 0)
                {
                    throw new Exception(errMsg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






        public static System.Data.DataTable ExecuteSqlDataTable(string Query)
        {
            System.Data.DataTable table2;
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();
                Recordset businessObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                businessObject.DoQuery(Query);
                if (!businessObject.EoF)
                {
                    businessObject.MoveFirst();
                    int index = 0;
                    while (index < businessObject.Fields.Count)
                    {
                        table.Columns.Add(businessObject.Fields.Item(index).Name);
                        index++;
                    }
                    for (int i = 0; i < businessObject.RecordCount; i++)
                    {
                        DataRow row = table.NewRow();
                        for (index = 0; index < businessObject.Fields.Count; index++)
                        {
                            row[index] = Convert.ToString((dynamic)businessObject.Fields.Item(index).Value);
                        }
                        table.Rows.Add(row);
                        businessObject.MoveNext();
                    }
                }
                Marshal.ReleaseComObject(businessObject);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                table2 = table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return table2;
        }

        public static object ExecuteSqlScalar(string Query)
        {
            object obj3;
            try
            {
                object obj2 = null;
                Recordset businessObject = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                businessObject.DoQuery(Query);
                if (!businessObject.EoF)
                {
                    obj2 = businessObject.Fields.Item(0).Value;
                }
                Marshal.ReleaseComObject(businessObject);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                obj3 = obj2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obj3;
        }


    }
}







