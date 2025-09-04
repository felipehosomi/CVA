using System;
using System.Configuration;
using System.Data.OleDb;

namespace DAL.Excel
{
    public class ExcelConnectionDao : IDisposable
    {
        public ExcelConnectionDao(string fileName)
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
            GC.SuppressFinalize(this);
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
