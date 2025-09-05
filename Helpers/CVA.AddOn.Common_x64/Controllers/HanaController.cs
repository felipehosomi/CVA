using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Common.Controllers
{
    public class HanaController
    {
        private static HanaConnection Connection = new HanaConnection();
        private HanaDataReader DataReader;
        private HanaCommand Command;
        private HanaTransaction Transaction;

        private string ConnectionString;

        #region [ Construtor ]

        public HanaController()
        {
            this.ConnectionString = GetConnectionString(CVAApp.ServerName, CVAApp.DBUserName, CVAApp.DBPassword);
        }

        public HanaController(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public HanaController(string serverName, string dataBaseName, string userName, string userPassword)
        {
            this.ConnectionString = GetConnectionString(serverName, userName, userPassword);
        }

        #endregion

        #region [ Conexão ]

        public static HanaConnection GetConnection()
        {
            HanaConnection _conn = new HanaConnection();
            _conn = Connection;
            return _conn;
        }

        public void Connect()
        {
            if (Connection == null || Connection.ConnectionString != this.ConnectionString)
            {
                Connection = new HanaConnection();
            }

            if (Connection.State == System.Data.ConnectionState.Broken || Connection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    Connection.ConnectionString = this.ConnectionString;
                    Connection.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao conectar HANA: " + ex.Message);
                }
            }
        }

        public void Close()
        {
            if (Connection.State == System.Data.ConnectionState.Open || Connection.State == System.Data.ConnectionState.Executing || Connection.State == System.Data.ConnectionState.Fetching)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }

        public static string GetConnectionString(string serverName, string userName, string userPassword)
        {
            string connectionString = String.Format(@"Server={0};UserID={1};Password={2}", serverName, userName, userPassword);
            return connectionString;
        }

        #endregion

        public object ExecuteScalar(string sQuery)
        {
            try
            {
                this.Connect();
                this.Command = new HanaCommand(sQuery, Connection);
                this.Command.CommandTimeout = 120;
                this.Command.Transaction = Transaction;
                return this.Command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar ExecuteScalar: " + ex.Message);
            }
        }

        public void ExecuteNonQuery(string sQuery)
        {
            try
            {
                this.Connect();
                this.Command = new HanaCommand(sQuery, Connection);
                this.Command.CommandTimeout = 120;
                this.Command.Transaction = Transaction;
                this.Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar ExecuteNonQuery: " + ex.Message);
            }
        }

        public HanaDataReader ExecuteReader(string sql)
        {
            try
            {
                this.Connect();
                this.Command = new HanaCommand(sql, Connection);
                this.Command.CommandTimeout = 120;
                this.Command.Transaction = Transaction;
                this.DataReader = Command.ExecuteReader();

                //HanaCommand cmdPDV = new HanaCommand(sQuery, _conn);
                //HanaDataReader oDataHeader = cmdPDV.ExecuteReader();
                /*
                string sCardCode = String.Empty;

                while (DataReader.Read())
                {
                    sCardCode = DataReader.GetString(0).ToString();
                    Util.GravarLog(sCaminho, String.Format(@"[PROCESSO] - Atualizando Parceiro {0}.", sCardCode));

                    string sQueryTemp = String.Format(@"UPDATE ""{0}"".""OCRD"" SET ""U_Categoria"" = 'Não Associado'
                                                        WHERE ""CardCode"" = '{1}'", sBaseSAP, sCardCode);

                    HanaCommand hcQuery = new HanaCommand(sQueryTemp, _conn);
                    int iRetorno = hcQuery.ExecuteNonQuery();

                    //objRS.DoQuery(sQueryTemp);                    
                }
                */
                
                return this.DataReader;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar HanaDataReader: " + ex.Message);
            }
        }

        public List<T> FillModelList<T>(string sql)
        {
            List<T> modelList = new List<T>();
            T model;
            ModelControllerAttribute modelController;
            try
            {
                this.Connect();
                HanaCommand cmdPDV = new HanaCommand(sql, Connection);
                HanaDataReader dr = cmdPDV.ExecuteReader();

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
                                    if (!modelController.DataBaseFieldYN && !modelController.FillOnSelect)
                                    {
                                        break;
                                    }

                                    int index = dr.GetOrdinal(modelController.ColumnName);
                                    if (!dr.IsDBNull(index))
                                    {
                                        Type dbType = dr.GetFieldType(index);

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
            catch (Exception e)
            {

            }
            return modelList;
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

            this.Connect();
            HanaCommand cmdPDV = new HanaCommand(sql, Connection);
            HanaDataReader dr = cmdPDV.ExecuteReader();

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
            return modelList;
        }

    }
}
