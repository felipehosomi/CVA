using System;
using System.Data;
using System.Data.OleDb;

namespace DAL.Excel
{
    public class ExcelFileDao
    {
        public ExcelFileDao()
        {

        }

        public DataTable ReadFile(string fileName)
        {
            using (var conn = new ExcelConnectionDao(fileName))
            {
                var dt = conn.Connection.GetSchema("Tables");
                var firstSheet = dt.Rows[0]["TABLE_NAME"].ToString();

                var selectCommand = $" SELECT * FROM [{firstSheet}$]";

                var adapter = new OleDbDataAdapter(selectCommand, conn.Connection);
                var ds = new DataSet();
                adapter.Fill(ds);
                adapter.Dispose();
                return ds.Tables[0];
            }
        }

        public DataTable ReadFile(string fileName, string sheetName)
        {
            using (var conn = new ExcelConnectionDao(fileName))
            {
                //DataTable dt = conn.Connection.GetSchema("Tables");
                //string firstSheet = dt.Rows[0]["TABLE_NAME"].ToString();

                var selectCommand = $" SELECT * FROM [{sheetName}$]";

                var adapter = new OleDbDataAdapter(selectCommand, conn.Connection);
                var ds = new DataSet();
                adapter.Fill(ds);
                adapter.Dispose();
                return ds.Tables[0];
            }
        }
    }
}
