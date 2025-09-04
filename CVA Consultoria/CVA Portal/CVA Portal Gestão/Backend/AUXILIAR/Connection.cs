using System;
using System.Data;
using System.Data.SqlClient;

namespace AUXILIAR
{
    public class Connection : IDisposable
    {
        public static Helper _helper = new Helper();
        private string _connectionString = _helper.readConnectionString();
        SqlConnection conn = null;

        internal void CreateDataAdapter(object storeProcedure)
        {
            throw new NotImplementedException();
        }

        SqlDataAdapter adapter = null;

        public Connection()
        {
            OpenConnection();
        }

        private void OpenConnection()
        {
            try
            {
                CloseConnection();
                conn = new SqlConnection(_connectionString);
                conn.Open();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void CloseConnection()
        {
            if (conn != null)
            {
                conn.Close();
                conn = null;
            }
            if (adapter != null)
                adapter = null;
        }
        public void CreateDataAdapter(string procedureName)
        {
            try
            {
                adapter = new SqlDataAdapter(procedureName, conn);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void InsertParameter(string parameterName, object value)
        {
            try
            {
                if (value is string)
                    adapter.SelectCommand.Parameters.Add("@" + parameterName, SqlDbType.VarChar).Value = value;
                if (value is DateTime)
                    adapter.SelectCommand.Parameters.Add("@" + parameterName, SqlDbType.DateTime).Value = value;
                if (value is Int32)
                    adapter.SelectCommand.Parameters.Add("@" + parameterName, SqlDbType.Int).Value = value;
                if (value is decimal)
                    adapter.SelectCommand.Parameters.Add("@" + parameterName, SqlDbType.Decimal).Value = value;
                if (value is double)
                    adapter.SelectCommand.Parameters.Add("@" + parameterName, SqlDbType.Float).Value = value;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void InserParameter(string parameter, SqlDbType type, object value)
        {
            if (value != null)
                adapter.SelectCommand.Parameters.Add("@" + parameter, type).Value = value;
        }
        public DataTable GetResult()
        {
            DataTable data = new DataTable();
            adapter.Fill(data);
            CloseConnection();

            return data;
        }
        public DataSet GetResult(DataSet data)
        {
            adapter.Fill(data);
            CloseConnection();

            return data;
        }
        public DataSet GetResultAsDataSet()
        {
            DataSet data = new DataSet();
            adapter.Fill(data);
            CloseConnection();

            return data;
        }
        public string GetConnectionString()
        {
            return _connectionString;
        }


        public void Dispose()
        {
            CloseConnection();
            GC.Collect();
        }
    }
}
