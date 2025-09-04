using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace CVA.Cointer.BoePrinter.DAO
{
    public class HanaDAO
    {
        private static HanaConnection Connection = new HanaConnection();
        private HanaDataAdapter DataAdapter = new HanaDataAdapter();
        private HanaDataReader DataReader;
        private HanaCommand Command;
        private static HanaTransaction Transaction;

        public string TableName { get; set; }
        public object Model { get; set; }

        private static string ConnectionString;
        public static string Database { get; set; }

        public HanaDAO()
        {
            if (String.IsNullOrEmpty(ConnectionString))
            {
                Database = System.Configuration.ConfigurationManager.AppSettings["Database"];
                string server = System.Configuration.ConfigurationManager.AppSettings["Server"];
                string dbUser = System.Configuration.ConfigurationManager.AppSettings["User"];
                string dbPassword = System.Configuration.ConfigurationManager.AppSettings["Password"];

                ConnectionString = $"Server={server};UserID={dbUser};Password={dbPassword}";
            }
        }

        #region GetNextCode
        /// <summary>
        /// Retorna o próximo código
        /// </summary>
        /// <param name="tableName">Nome da tabela</param>
        /// <returns>Código</returns>
        public string GetNextCode(string tableName)
        {
            return GetNextCode(tableName, "Code", String.Empty);
        }

        /// <summary>
        /// Retorna o próximo código
        /// </summary>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="fieldName">Nome do campo</param>
        /// <returns>Código</returns>
        public string GetNextCode(string tableName, string fieldName)
        {
            return GetNextCode(tableName, fieldName, String.Empty);
        }

        /// <summary>
        /// Retorna o próximo código
        /// </summary>
        /// <param name="fieldName">Nome do campo</param>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="where">Where</param>
        /// <returns>Código</returns>
        public string GetNextCode(string tableName, string fieldName, string where)
        {
            string Hana = String.Format(" SELECT MAX(\"{0}\") + 1 FROM \"{1}\".\"{2}\" ", fieldName, Database, tableName);

            if (!String.IsNullOrEmpty(where))
            {
                Hana += String.Format(" WHERE {0} ", where);
            }

            object code = this.ExecuteScalar(Hana);

            if (code != null && code != DBNull.Value)
            {
                return Convert.ToInt32(code).ToString();
            }
            else
            {
                return "1";
            }
        }
        #endregion GetNextCode

        public string GetConnectedServer()
        {
            return Connection.DataSource;
        }

        public void Connect()
        {
            if (Connection == null || !ConnectionString.StartsWith(Connection.ConnectionString))
            {
                Connection = new HanaConnection();
            }

            if (Connection.State == ConnectionState.Broken || Connection.State == ConnectionState.Closed)
            {
                try
                {
                    Connection.ConnectionString = ConnectionString;
                    Connection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao conectar Hana: " + ex.Message);
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

        public HanaDataReader ExecuteReader(string Hana)
        {
            try
            {
                this.Connect();
                this.Command = new HanaCommand(Hana, Connection);
                this.Command.CommandTimeout = 120;
                this.Command.CommandType = CommandType.Text;
                this.Command.Transaction = Transaction;
                this.DataReader = Command.ExecuteReader();
                return this.DataReader;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar HanaDataReader: " + ex.Message);
            }
        }

        public object ExecuteScalar(string Hana)
        {
            try
            {
                this.Connect();
                this.Command = new HanaCommand(Hana, Connection);
                this.Command.CommandTimeout = 120;
                this.Command.CommandType = CommandType.Text;
                this.Command.Transaction = Transaction;
                return this.Command.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao executar ExecuteScalar: " + ex.Message);

            }
        }

        public void ExecuteNonQuery(string Hana)
        {
            try
            {
                this.Connect();
                this.Command = new HanaCommand(Hana, Connection);
                this.Command.CommandTimeout = 120;
                this.Command.CommandType = CommandType.Text;
                this.Command.Transaction = Transaction;
                this.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar ExecuteNonQuery: " + ex.Message);
            }
        }

        public DataTable FillDataTable(string Hana)
        {
            try
            {
                this.Connect();
                this.Command = new HanaCommand(Hana, Connection);
                this.Command.CommandTimeout = 120;
                this.Command.CommandType = CommandType.Text;
                this.Command.CommandText = Hana;
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

        public int GetRowCount(string Hana)
        {
            int recordCount = 0;
            using (HanaDataReader dr = this.ExecuteReader(Hana))
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                recordCount = dt.Rows.Count;
            }

            return recordCount;
        }

        public bool HasRows(string command)
        {
            bool hasRows;
            using (HanaDataReader dr = this.ExecuteReader(command))
            {
                hasRows = dr.HasRows;
            }

            return hasRows;
        }

        public bool Exists(string where)
        {
            string command = $"SELECT 1 FROM \"{Database}\".\"{TableName}\" WHERE {where} ";
            return this.HasRows(command);
        }

        public List<string> FillStringList(string Hana)
        {

            List<string> list = new List<string>();
            using (HanaDataReader dr = this.ExecuteReader(Hana))
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

        public T FillModel<T>(string Hana)
        {
            List<T> modelList = this.FillModelList<T>(Hana);
            if (modelList.Count > 0)
            {
                return modelList[0];
            }
            else
            {
                return Activator.CreateInstance<T>();
            }
        }

        public List<T> FillModelList<T>(string Hana)
        {
            List<T> modelList = new List<T>();
            T model;
            using (HanaDataReader dr = this.ExecuteReader(Hana))
            {
                while (dr.Read())
                {
                    // Cria nova instância do model
                    model = Activator.CreateInstance<T>();

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        PropertyInfo property = model.GetType().GetProperty(dr.GetName(i));
                        if (property == null)
                        {
                            throw new Exception($"Propriedade {dr.GetName(i)} não encontrada no model");
                        }

                        if (!dr.IsDBNull(i))
                        {
                            if (dr.GetFieldType(i) == typeof(Decimal))
                            {
                                property.SetValue(model, Convert.ToDouble(dr.GetValue(i).ToString()), null);
                            }
                            else
                            {
                                property.SetValue(model, dr.GetValue(i), null);
                            }
                        }
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }
    }
}
