using System;
using System.Configuration;
using System.Data.OleDb;

namespace CVA.NF.Import.HELPER
{
    public class ExcelConnection : IDisposable
    {
        public ExcelConnection(string fileName)
        {
            var connectionString = ConfigurationManager.AppSettings["ExcelConnectionString"];
            Connection = new OleDbConnection(string.Format(connectionString, fileName));
            Open();
        }

        public OleDbConnection Connection { get; set; }

        public void Dispose()
        {
            Connection.Close();
            Connection = null;
            GC.Collect();
        }

        private void Open()
        {
            Connection.Open();
        }

        private void Close()
        {
            Connection.Close();
            GC.Collect();
        }
    }
}
