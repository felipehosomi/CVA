using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmailAutorizacao.HELPER
{
    class SqlHelper
    {
        #region Properties
        private static SqlConnection Connection = new SqlConnection();
        private SqlDataAdapter DataAdapter;
        private SqlDataReader DataReader;
        private SqlCommand Command;
        private static SqlTransaction Transaction;

        public object Model { get; set; }

        private string ConnectionString;
        #endregion

        #region Constructor
        public SqlHelper()
        {
            ConnectionString = GetConnectionString(StaticKeys.ConfigModel.Servidor, StaticKeys.ConfigModel.Banco, StaticKeys.ConfigModel.Usuario, StaticKeys.ConfigModel.Senha);
        }

        public SqlHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public SqlHelper(string serverName, string dataBaseName, string userName, string userPassword)
        {
            ConnectionString = GetConnectionString(serverName, dataBaseName, userName, userPassword);
        }
        #endregion

        #region GetNextCode
        /// <summary>
        /// Retorna o próximo código
        /// </summary>
        /// <param name="tableName">Nome da tabela</param>
        /// <returns>Código</returns>
        public string GetNextCode(string tableName)
        {
            return GetNextCode(tableName, "Code", string.Empty);
        }

        /// <summary>
        /// Retorna o próximo código
        /// </summary>
        /// <param name="tableName">Nome da tabela</param>
        /// <param name="fieldName">Nome do campo</param>
        /// <returns>Código</returns>
        public string GetNextCode(string tableName, string fieldName)
        {
            return GetNextCode(tableName, fieldName, string.Empty);
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
            var sql = string.Format(" SELECT ISNULL(MAX(CAST({0} AS BIGINT)), 0) + 1 FROM [{1}] ", fieldName, tableName);

            if (!string.IsNullOrEmpty(where))
            {
                sql += string.Format(" WHERE {0} ", where);
            }

            var code = ExecuteScalar(sql);

            if (code != null)
            {
                return Convert.ToInt32(code).ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion GetNextCode

        #region Connection
        public static string GetConnectionString(string serverName, string dataBaseName, string userName, string userPassword)
        {
            var connectionString = String.Format(@" data source={0};initial catalog={1};persist security info=True;user id={2};password={3};",
                                                serverName,
                                                dataBaseName,
                                                userName,
                                                userPassword);
            return connectionString;
        }

        public void Connect()
        {
            if (Connection.State == ConnectionState.Broken || Connection.State == ConnectionState.Closed)
            {
                try
                {
                    Connection.ConnectionString = ConnectionString;
                    Connection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao conectar SQL: " + ex.Message);
                }
            }
        }

        public static void Close()
        {
            if (Connection.State == ConnectionState.Open || Connection.State == ConnectionState.Executing || Connection.State == ConnectionState.Fetching)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }
        #endregion

        #region Execute
        public SqlDataReader ExecuteReader(string sql)
        {
            try
            {
                if (Connection == null || Connection.State != ConnectionState.Open)
                {
                    Connect();
                }
                Command = new SqlCommand
                {
                    CommandText = sql,
                    Connection = Connection,
                    Transaction = Transaction
                };
                DataReader = Command.ExecuteReader();
                return DataReader;
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
                if (Connection == null || Connection.State != ConnectionState.Open)
                {
                    Connect();
                }
                Command = new SqlCommand
                {
                    CommandText = sql,
                    Connection = Connection,
                    Transaction = Transaction
                };
                return Command.ExecuteScalar();
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
                Connect();
                Command = new SqlCommand
                {
                    CommandText = sql,
                    Connection = Connection,
                    Transaction = Transaction
                };
                Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar ExecuteNonQuery: " + ex.Message);
            }
        }
        #endregion

        #region Transaction
        public void BeginTransaction()
        {
            Connect();
            Transaction = Connection.BeginTransaction();
        }

        public static void RollbackTransaction()
        {
            if (Transaction.Connection != null)
            {
                Transaction.Rollback();
            }
        }

        public static void CommitTransaction()
        {
            if (Transaction.Connection != null)
            {
                Transaction.Commit();
            }
        }
        #endregion

        #region GetRowCount
        public int GetRowCount(string sql)
        {
            var recordCount = 0;
            using (SqlDataReader dr = ExecuteReader(sql))
            {
                var dt = new DataTable();
                dt.Load(dr);
                recordCount = dt.Rows.Count;
                dt.Dispose();
            }

            return recordCount;
        }
        #endregion

        #region Fill
        public DataTable FillDataTable(string sql)
        {
            try
            {
                Connect();
                Command = new SqlCommand
                {
                    CommandText = sql,
                    Connection = Connection,
                    CommandType = CommandType.Text
                };
                DataAdapter = new SqlDataAdapter
                {
                    SelectCommand = Command
                };
                
                var dtb = new DataTable();

                DataAdapter.Fill(dtb);
                DataAdapter.Dispose();
                return dtb;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar FillDataTable: " + ex.Message);
            }
        }

        public List<string> FillStringList(string sql)
        {

            var list = new List<string>();
            using (SqlDataReader dr = ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                    {
                        list.Add(dr.GetValue(0).ToString());
                    }
                    else
                    {
                        list.Add(string.Empty);
                    }
                }
            }
            return list;
        }

        public T FillModel<T>(string sql)
        {
            var modelList = FillModelList<T>(sql);
            if (modelList.Count > 0)
            {
                return modelList[0];
            }
            else
            {
                return Activator.CreateInstance<T>();
            }
        }

        public List<T> FillModelList<T>(string sql)
        {
            var modelList = new List<T>();
            T model;

            try
            {
                using (SqlDataReader dr = ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        // Cria nova instância do model
                        model = Activator.CreateInstance<T>();
                        // Seta os valores no model
                        foreach (PropertyInfo property in model.GetType().GetProperties())
                        {
                            try
                            {
                                var index = dr.GetOrdinal(property.Name);
                                if (!dr.IsDBNull(index))
                                {
                                    var dbType = dr.GetFieldType(index);

                                    if (dbType == typeof(decimal) && property.PropertyType == typeof(double))
                                    {
                                        property.SetValue(model, Convert.ToDouble(dr.GetValue(index)), null);
                                    }
                                    else
                                    {
                                        property.SetValue(model, dr.GetValue(index), null);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                throw new Exception(string.Format("Erro ao setar propriedade {0}: {1}", property.Name, e.Message));
                            }
                        }
                        modelList.Add(model);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return modelList;
        }

        public T FillModelAccordingToSql<T>(string sql)
        {
            var modelList = FillModelListAccordingToSql<T>(sql);
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
            var modelList = new List<T>();
            T model;
            using (SqlDataReader dr = ExecuteReader(sql))
            {
                while (dr.Read())
                {
                    // Cria nova instância do model
                    model = Activator.CreateInstance<T>();

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        var property = model.GetType().GetProperty(dr.GetName(i));
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
    }
}
