using Sap.Data.Hana;
using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Console.MRP.Infra
{
    public class Hana
    {
        private static HanaConnection Connection = new HanaConnection();
        private HanaDataReader DataReader;
        private HanaCommand Command;
        private readonly HanaTransaction Transaction;
        private readonly string ConnectionString;

        public Hana()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                var server = ConfigurationManager.AppSettings["Server"];
                var dbUser = ConfigurationManager.AppSettings["DBUser"];
                var dbPassword = Encoding.UTF8.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings["DBPassword"]));

                ConnectionString = $"Server={server};UserID={dbUser};Password={dbPassword}";
            }
        }

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

        public async Task<object> ExecuteScalarAsync(string query)
        {
            try
            {
                Connect();

                Command = new HanaCommand(query, Connection)
                {
                    CommandTimeout = 120,
                    CommandType = CommandType.Text,
                    Transaction = Transaction
                };

                return await Command.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar HanaDataReaderAsync: " + ex.Message);
            }
        }

        public async Task<HanaDataReader> ExecuteReaderAsync(string query)
        {
            try
            {
                Connect();

                Command = new HanaCommand(query, Connection)
                {
                    CommandTimeout = 120,
                    CommandType = CommandType.Text,
                    Transaction = Transaction
                };
                DataReader = await Command.ExecuteReaderAsync();

                return DataReader;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar HanaDataReaderAsync: " + ex.Message);
            }
        }

        public async Task ExecuteNonQueryAsync(string query)
        {
            try
            {
                Connect();

                Command = new HanaCommand(query, Connection)
                {
                    CommandTimeout = 120,
                    CommandType = CommandType.Text,
                    Transaction = Transaction
                };

                await Command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao executar HanaDataReaderAsync: " + ex.Message);
            }
        }
    }
}
