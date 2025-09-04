using CVA.Cointer.Megasul.API.Attributes;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace CVA.Cointer.Megasul.API.DAO
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
                string dbUser = System.Configuration.ConfigurationManager.AppSettings["DBUser"];
                string dbPassword = System.Configuration.ConfigurationManager.AppSettings["DBPassword"];

                ConnectionString = $"Server={server};UserID={dbUser};Password={dbPassword};CS=\"{Database}\"";
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

        #region CreateModel
        /// <summary>
        /// Salva o model no banco de dados
        /// </summary>
        public void CreateModel()
        {
            try
            {
                ModelControllerAttribute modelController;

                string HanaColumns = String.Empty;
                string HanaValues = String.Empty;

                List<Type> typesList = new List<Type>();
                List<object> valuesList = new List<object>();

                PropertyInfo propCode = Model.GetType().GetProperty("Code");
                if (propCode != null)
                {
                    if (propCode.GetValue(Model, null) == null)
                    {
                        propCode.SetValue(Model, GetNextCode(TableName).PadLeft(8, '0'));
                    }
                }

                //Dictionary<Type, object> values = new Dictionary<Type, object>();
                //List<object> values = new List<object>();
                // Percorre as propriedades do Model
                foreach (PropertyInfo property in Model.GetType().GetProperties())
                {
                    try
                    {
                        // Busca os Custom Attributes
                        foreach (Attribute attribute in property.GetCustomAttributes(true))
                        {
                            modelController = attribute as ModelControllerAttribute;

                            if (modelController != null)
                            {
                                // Se não for DataBaseField não seta nas properties
                                if (!modelController.DataBaseFieldYN)
                                {
                                    break;
                                }

                                typesList.Add(property.PropertyType);
                                valuesList.Add(property.GetValue(Model, null));

                                if (String.IsNullOrEmpty(modelController.ColumnName))
                                {
                                    modelController.ColumnName = property.Name;
                                }

                                HanaColumns += String.Format(", \"{0}\"", modelController.ColumnName);
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(String.Format("Erro ao setar propriedade {0}: {1}", property.Name, e));
                    }
                }
                if (String.IsNullOrEmpty(HanaColumns))
                {
                    throw new Exception("Nenhuma coluna informada. Informe a propriedade ColumnName no Model");
                }

                for (int i = 0; i < typesList.Count; i++)
                {
                    if (valuesList[i] == null || valuesList[i] == DBNull.Value)
                    {
                        HanaValues += ", NULL";
                    }
                    else if (typesList[i] == typeof(string) || typesList[i] == typeof(String) || typesList[i] == typeof(char))
                    {
                        HanaValues += String.Format(", '{0}'", valuesList[i].ToString().Replace("'", "''"));
                    }
                    else if (typesList[i] == typeof(DateTime) || typesList[i] == typeof(Nullable<DateTime>))
                    {
                        HanaValues += String.Format(", '{0}' ", ((DateTime)valuesList[i]).ToString("yyyyMMdd"));
                    }
                    else
                    {
                        HanaValues += String.Format(", '{0}' ", valuesList[i].ToString().Replace(",", "."));
                    }
                }

                string Hana = String.Format(" INSERT INTO \"{0}\".\"{1}\" ({2}) VALUES ({3}) ", Database, TableName, HanaColumns.Substring(2), HanaValues.Substring(2));

                this.ExecuteNonQuery(Hana);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region UpdateModel
        public void UpdateModel()
        {
            try
            {
                ModelControllerAttribute modelController;

                string where = String.Empty;
                string HanaValues = String.Empty;
                object value;

                Dictionary<Type, object> values = new Dictionary<Type, object>();
                //List<object> values = new List<object>();
                // Percorre as propriedades do Model
                foreach (PropertyInfo property in Model.GetType().GetProperties())
                {
                    try
                    {
                        // Busca os Custom Attributes
                        foreach (Attribute attribute in property.GetCustomAttributes(true))
                        {
                            modelController = attribute as ModelControllerAttribute;

                            if (modelController != null)
                            {
                                // Se não for DataBaseField não seta nas properties
                                if (!modelController.DataBaseFieldYN)
                                {
                                    break;
                                }
                                if (String.IsNullOrEmpty(modelController.ColumnName))
                                {
                                    modelController.ColumnName = property.Name;
                                }

                                value = property.GetValue(Model, null);
                                if (modelController.IsPK || property.Name == "Code")
                                {
                                    if (value == null)
                                    {
                                        where += String.Format(" AND \"{0}\" = NULL", modelController.ColumnName);
                                    }
                                    else if (property.PropertyType == typeof(string))
                                    {
                                        where += String.Format(" AND \"{0}\" = '{1}'", modelController.ColumnName, value);
                                    }
                                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(Nullable<DateTime>))
                                    {
                                        where += String.Format(" AND \"{0}\" = CONVERT(DATETIME, '{1}')", modelController.ColumnName, Convert.ToDateTime(value).ToString("yyyyMMdd"));
                                    }
                                    else
                                    {
                                        where += String.Format(" AND \"{0}\" = {1}", modelController.ColumnName, value.ToString().Replace(",", "."));
                                    }
                                }
                                else
                                {
                                    if (value == null)
                                    {
                                        HanaValues += String.Format(", \"{0}\" = NULL", modelController.ColumnName);
                                    }
                                    else if (property.PropertyType == typeof(string))
                                    {
                                        HanaValues += String.Format(", \"{0}\" = '{1}'", modelController.ColumnName, value.ToString().Replace("'", "''"));
                                    }
                                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(Nullable<DateTime>))
                                    {
                                        HanaValues += String.Format(", \"{0}\" = '{1}'", modelController.ColumnName, Convert.ToDateTime(value).ToString("yyyyMMdd"));
                                    }
                                    else
                                    {
                                        HanaValues += String.Format(", \"{0}\" = {1}", modelController.ColumnName, value.ToString().Replace(",", "."));
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(String.Format("Erro ao setar propriedade {0}: {1}", property.Name, e));
                    }
                }

                if (String.IsNullOrEmpty(where))
                {
                    throw new Exception("Nenhuma coluna PK informada. Informe a propriedade IsPK no Model ou crie um campo chamado 'Code' (será utilizado como PK por default)");
                }
                if (String.IsNullOrEmpty(HanaValues))
                {
                    throw new Exception("Nenhuma coluna informada para atualizar. Informe a propriedade ColumnName no Model");
                }

                string Hana = String.Format(" UPDATE \"{0}\".\"{1}\" SET {2} WHERE {3} ", Database, TableName, HanaValues.Substring(2), where.Substring(4));
                this.ExecuteNonQuery(Hana);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        public void DeleteModel()
        {

            try
            {
                ModelControllerAttribute modelController;

                string where = String.Empty;
                object value;

                Dictionary<Type, object> values = new Dictionary<Type, object>();
                //List<object> values = new List<object>();
                // Percorre as propriedades do Model
                foreach (PropertyInfo property in Model.GetType().GetProperties())
                {
                    try
                    {
                        // Busca os Custom Attributes
                        foreach (Attribute attribute in property.GetCustomAttributes(true))
                        {
                            modelController = attribute as ModelControllerAttribute;

                            if (modelController != null)
                            {
                                // Se não for DataBaseField não seta nas properties
                                if (!modelController.DataBaseFieldYN)
                                {
                                    break;
                                }
                                if (String.IsNullOrEmpty(modelController.ColumnName))
                                {
                                    modelController.ColumnName = property.Name;
                                }

                                value = property.GetValue(Model, null);
                                if (modelController.IsPK || property.Name == "Code")
                                {
                                    if (value == null)
                                    {
                                        where += String.Format(" AND \"{0}\" = NULL", modelController.ColumnName);
                                    }
                                    else if (property.PropertyType == typeof(string))
                                    {
                                        where += String.Format(" AND \"{0}\" = '{1}'", modelController.ColumnName, value);
                                    }
                                    else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(Nullable<DateTime>))
                                    {
                                        where += String.Format(" AND \"{0}\" = CONVERT(DATETIME, '{1}')", modelController.ColumnName, Convert.ToDateTime(value).ToString("yyyyMMdd"));
                                    }
                                    else
                                    {
                                        where += String.Format(" AND \"{0}\" = {1}", modelController.ColumnName, value.ToString().Replace(",", "."));
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(String.Format("Erro ao setar propriedade {0}: {1}", property.Name, e));
                    }
                }

                if (String.IsNullOrEmpty(where))
                {
                    throw new Exception("Nenhuma coluna PK informada. Informe a propriedade IsPK no Model ou crie um campo chamado 'Code' (será utilizado como PK por default)");
                }

                string Hana = String.Format(" DELETE FROM \"{0}\".\"{1}\" WHERE {2} ", Database, TableName, where.Substring(4));
                this.ExecuteNonQuery(Hana);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetConnectedServer()
        {
            return Connection.DataSource;
        }

        public void Connect()
        {
            if (Connection == null)
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
            ModelControllerAttribute modelController;
            try
            {
                using (HanaDataReader dr = this.ExecuteReader(Hana))
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
                                // Busca os Custom Attributes
                                foreach (Attribute attribute in property.GetCustomAttributes(true))
                                {
                                    modelController = attribute as ModelControllerAttribute;
                                    if (modelController != null)
                                    {
                                        // Se propriedade "ColumnName" estiver vazia, pega o nome da propriedade
                                        if (String.IsNullOrEmpty(modelController.ColumnName))
                                        {
                                            modelController.ColumnName = property.Name;
                                        }
                                        if (!modelController.FillOnSelect)
                                        {
                                            break;
                                        }

                                        int index = dr.GetOrdinal(modelController.ColumnName);
                                        if (!dr.IsDBNull(index))
                                        {
                                            Type dbType = dr.GetFieldType(index);

                                            if (dbType == typeof(decimal) && property.PropertyType == typeof(double))
                                            {
                                                property.SetValue(model, Convert.ToDouble(dr.GetValue(index).ToString()), null);
                                            }
                                            else
                                            {
                                                property.SetValue(model, dr.GetValue(index), null);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                throw new Exception(String.Format("Erro ao setar propriedade {0}: {1}", property.Name, e.Message));
                            }
                        }
                        modelList.Add(model);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return modelList;
        }

        public T FillModelFromCommand<T>(string Hana)
        {
            List<T> modelList = this.FillListFromCommand<T>(Hana);
            if (modelList.Count > 0)
            {
                return modelList[0];
            }
            else
            {
                return Activator.CreateInstance<T>();
            }
        }

        public List<T> FillListFromCommand<T>(string Hana)
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
                        try
                        {
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
                        catch (Exception ex)
                        {
                            throw new Exception($"{property.Name}: {ex.Message}");
                        }
                    }
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        public T RetrieveModel<T>(string where = "", string orderBy = "")
        {
            List<T> modelList = this.RetrieveModelList<T>(where, orderBy);
            if (modelList.Count > 0)
                return modelList[0];
            else
                return Activator.CreateInstance<T>();
        }

        public List<T> RetrieveModelList<T>(string where = "", string orderBy = "")
        {
            StringBuilder command = new StringBuilder();
            // Inicia o SELECT
            command.Append(" SELECT ");

            Type modelType = typeof(T);
            ModelControllerAttribute modelController;

            string fields = String.Empty;
            string fieldTableName;
            // Percorre as propriedades do Model para montar o SELECT
            foreach (PropertyInfo property in modelType.GetProperties())
            {
                // Busca os Custom Attributes
                foreach (Attribute attribute in property.GetCustomAttributes(true))
                {
                    modelController = attribute as ModelControllerAttribute;
                    if (modelController == null)
                    {
                        continue;
                    }
                    // Se propriedade "ColumnName" estiver vazia, pega o nome da propriedade
                    if (String.IsNullOrEmpty(modelController.ColumnName))
                        modelController.ColumnName = property.Name;
                    if (modelController != null)
                    {
                        // Se não for DataBaseField não adiciona no select
                        if (!modelController.DataBaseFieldYN)
                        {
                            break;
                        }
                        fieldTableName = String.IsNullOrEmpty(modelController.TableName) ? TableName : modelController.TableName;
                        fields += String.Format(", \"{0}\".\"{1}\" ", fieldTableName, modelController.ColumnName);
                    }
                    break;
                }
            }

            if (String.IsNullOrEmpty(fields))
            {
                throw new Exception("Nenhuma propriedade do tipo ModelController encontrada no Model");
            }

            // Campos a serem retornados
            command.Append(fields.Substring(1));

            command.AppendFormat(" FROM \"{0}\".\"{1}\" ", Database, TableName);


            // Condição WHERE
            if (!String.IsNullOrEmpty(where))
            {
                command.AppendFormat(" WHERE {0} ", where);
            }

            // Condição ORDER BY
            if (!String.IsNullOrEmpty(orderBy))
            {
                command.AppendFormat(" ORDER BY {0} ", orderBy);
            }

            return FillModelList<T>(command.ToString());
        }
    }
}
