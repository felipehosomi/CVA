using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CVA.View.BancadaTeste.Util
{
    public class SqlUtil
    {
        private static SqlConnection Connection = new SqlConnection();
        private SqlDataAdapter DataAdapter = new SqlDataAdapter();
        private SqlDataReader DataReader;
        private SqlCommand Command;
        private static SqlTransaction Transaction;

        public string TableName { get; set; }
        public object Model { get; set; }

        private string ConnectionString;

        public SqlUtil(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public SqlUtil(string serverName, string dataBaseName, string userName, string userPassword)
        {
            this.ConnectionString = GetConnectionString(serverName, dataBaseName, userName, userPassword);
        }

        public void Connect()
        {
            if (Connection == null || Connection.ConnectionString != this.ConnectionString)
            {
                Connection = new SqlConnection();
            }

            if (Connection.State == ConnectionState.Broken || Connection.State == ConnectionState.Closed)
            {
                try
                {
                    Connection.ConnectionString = this.ConnectionString;
                    Connection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao conectar SQL: " + ex.Message);
                }
            }
        }

        public void Close()
        {
            if (Connection.State == ConnectionState.Open || Connection.State == ConnectionState.Executing || Connection.State == ConnectionState.Fetching)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }

        public SqlDataReader ExecuteReader(string sql)
        {
            try
            {
                this.Connect();
                this.Command = new SqlCommand(sql, Connection);
                this.Command.Transaction = Transaction;
                this.DataReader = Command.ExecuteReader();
                return this.DataReader;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar SqlDataReader: " + ex.Message);
            }
        }

        public object ExecuteScalar(string sql)
        {
            try
            {
                this.Connect();
                this.Command = new SqlCommand(sql, Connection);
                this.Command.Transaction = Transaction;
                return this.Command.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao executar ExecuteScalar: " + ex.Message);

            }
        }

        public void ExecuteNonQuery(string sql)
        {
            try
            {
                this.Connect();
                this.Command = new SqlCommand(sql, Connection);
                this.Command.Transaction = Transaction;
                this.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar ExecuteNonQuery: " + ex.Message);
            }
        }

        public DataTable FillDataTable(string sql)
        {
            try
            {
                this.Connect();
                this.Command = new SqlCommand(sql, Connection);
                this.Command.CommandType = CommandType.Text;
                this.Command.CommandText = sql;
                DataAdapter.SelectCommand = this.Command;

                DataTable dtb = new DataTable();

                DataAdapter.Fill(dtb);
                DataAdapter.Dispose();
                return dtb;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar FillDataTable: " + ex.Message);
            }
        }

        public void BeginTransaction()
        {
            this.Connect();
            Transaction = Connection.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            if (Transaction.Connection != null)
            {
                Transaction.Rollback();
            }
        }

        public void CommitTransaction()
        {
            if (Transaction.Connection != null)
            {
                Transaction.Commit();
            }
        }

        public static string GetConnectionString(string serverName, string dataBaseName, string userName, string userPassword)
        {
            string connectionString = String.Format(@" data source={0};initial catalog={1};persist security info=True;user id={2};password={3};",
                                                serverName,
                                                dataBaseName,
                                                userName,
                                                userPassword);
            return connectionString;
        }

        public int GetRowCount(string sql)
        {
            int recordCount = 0;
            using (SqlDataReader dr = this.ExecuteReader(sql))
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                recordCount = dt.Rows.Count;
            }

            return recordCount;
        }

        public List<string> FillStringList(string sql)
        {

            List<string> list = new List<string>();
            using (SqlDataReader dr = this.ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                    {
                        list.Add(dr.GetValue(0).ToString());
                    }
                    else
                    {
                        list.Add(String.Empty);
                    }
                }
            }
            return list;
        }

        public T FillModelAccordingToSql<T>(string sql)
        {
            List<T> modelList = this.FillModelListAccordingToSql<T>(sql);
            if (modelList.Count > 0)
            {
                return modelList[0];
            }
            else
            {
                return Activator.CreateInstance<T>();
            }
        }

        public List<T> FillModelListAccordingToSql<T>(string sql)
        {
            List<T> modelList = new List<T>();
            T model;
            using (SqlDataReader dr = this.ExecuteReader(sql))
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
    }
}
