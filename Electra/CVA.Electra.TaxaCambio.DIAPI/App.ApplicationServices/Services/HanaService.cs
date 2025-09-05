using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ApplicationServices.Services
{
    public class HanaService
    {
        HanaConnection _conn;
        private string _Database;

        private string database;

        public string Database
        {
            get
            {
                return database;
            }

            set
            {
                database = value;
            }
        }

        public HanaService()
        {
            try
            {
                _conn = new HanaConnection(FrameworkService.GetHanaConnectionString());
                _conn.Open();
                Database = FrameworkService.Database;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public object ExecuteScalar(string querySql)
        {
            try
            {
                 var cmd = new HanaCommand(querySql, _conn);
                return cmd.ExecuteScalar();
            }
            catch (global::System.Exception)
            {

                throw;
            }

        }

        public DataTable ExecuteDataTable(string querySql)
        {
            try
            {
                DataTable dt = new DataTable("Table1");
                HanaDataAdapter da = new HanaDataAdapter(querySql, _conn);
                da.Fill(dt);
                _conn.Close();
                return dt;
            }
            catch (global::System.Exception)
            {

                throw;
            }

        }
    }
}
