using CVA.NF.Import.HELPER;
using System;
using System.Data;
using System.Data.OleDb;

namespace CVA.NF.Import.DAO
{
    public class ExcelFileDAO
    {
        public DataTable ReadFile(string fileName, string sheetName)
        {
            try
            {
                using (var conn = new ExcelConnection(fileName))
                {
                    //DataTable dt = conn.Connection.GetSchema("Tables");
                    //string firstSheet = dt.Rows[0]["TABLE_NAME"].ToString();

                    string selectCommand = $" SELECT * FROM [{sheetName}$]";

                    using (var adapter = new OleDbDataAdapter(selectCommand, conn.Connection))
                    {
                        var ds = new DataSet();
                        adapter.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
