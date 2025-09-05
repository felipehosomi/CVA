using System;
using System.Configuration;
using System.Data.SqlClient;

namespace SkaSapWs.Data
{
    public class SqlHelper : IDisposable
    {
        private string ConnectionString { get; set; }
        private SqlConnection Connection { get; set; }
        bool disposed = false;

        public SqlHelper(string conn)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[conn].ConnectionString;
        }

        private SqlConnection GetOpenConnection()
        {
            if (Connection != null)
            {
                if (Connection.State == System.Data.ConnectionState.Closed)
                    Connection.Open();
            }
            else
            {
                Connection = new SqlConnection(ConnectionString);
                Connection.Open();
            }

            return Connection;
        }

        public int? ExecuteNonQuery(string command)
        {
            int? ret = null;

            if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
            {
                using (var cmd = new SqlCommand(command, Connection))
                {
                    ret = cmd.ExecuteNonQuery();
                }
            }
            else
            {
                var conn = GetOpenConnection();
                using (var cmd = new SqlCommand(command, conn))
                {
                    ret = cmd.ExecuteNonQuery();
                }
            }

            return ret;
        }

        public SqlDataReader ExecuteQuery(string command)
        {
            SqlDataReader ret = null;

            if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
            {
                using (var cmd = new SqlCommand(command, Connection))
                {
                    ret = cmd.ExecuteReader();
                }
            }
            else
            {
                var conn = GetOpenConnection();
                using (var cmd = new SqlCommand(command, conn))
                {
                    ret = cmd.ExecuteReader();
                }
            }
            return ret;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (Connection != null)
                    Connection.Dispose();
            }
            disposed = true;
        }
    }
}
