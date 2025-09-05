using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SapKsaWs.BLL.HELPER
{
    public class SqlHelper : IDisposable
    {
        private static ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string ConnectionString { get; set; }
        private SqlConnection Connection { get; set; }
        bool disposed = false;

        public SqlHelper(string conn)
        {
            log4net.Config.XmlConfigurator.Configure();
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

        public SqlConnection GetConnection()
        {
            return GetOpenConnection();
        }

        public void CloseConnection()
        {
            if (Connection != null && Connection.State == ConnectionState.Open)
            Connection.Close();
        }

        public int? ExecuteNonQuery(string command)
        {
            Logger.Debug(command);
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

        public object ExecuteScalar(string command)
        {
            try
            {
                Logger.Debug(command);
                object ret;
                if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
                {
                    using (var cmd = new SqlCommand(command, Connection))
                    {
                        ret = cmd.ExecuteScalar();
                    }
                }
                else
                {
                    var conn = GetOpenConnection();
                    using (var cmd = new SqlCommand(command, conn))
                    {
                        ret = cmd.ExecuteScalar();
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar ExecuteScalar: " + ex.Message);
            }
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

        #region Fill
        public List<string> FillStringList(string sql)
        {
            var list = new List<string>();
            using (var dr = ExecuteQuery(sql))
            {
                while (dr.Read())
                    if (!dr.IsDBNull(0))
                        list.Add(dr.GetValue(0).ToString());
                    else
                        list.Add(string.Empty);
            }
            return list;
        }

        public List<T> FillModelList<T>(string sql)
        {
            List<T> modelList = new List<T>();
            T model;
            using (SqlDataReader dr = ExecuteQuery(sql))
            {
                while (dr.Read())
                {
                    // Cria nova instância do model
                    model = Activator.CreateInstance<T>();

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        PropertyInfo property = model.GetType().GetProperty(dr.GetName(i));
                        if (!dr.IsDBNull(i))
                        {
                            property.SetValue(model, dr.GetValue(i), null);
                        }
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }
        #endregion

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
