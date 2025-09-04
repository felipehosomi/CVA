using AUXILIAR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class ImpressorDAO
    {
        #region Atributos
        public string connectionString { get; set; }
        public SqlConnection conn { get; set; }
        public XMLReader _reader { get; set; }
        #endregion

        #region Construtor
        public ImpressorDAO()
        {
            this._reader = new XMLReader();
            this.connectionString = _reader.ReadConnectionString();
            this.conn = new SqlConnection();
            this.conn.ConnectionString = connectionString;
        }
        #endregion

       
        public DataTable Get_By_Order(string order)
        {
            try
            {
                using (conn)
                {
                    PrepareConnection();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = $@"SELECT 
                                            FLOOR(T1.MENGE_VERBRAUCH) AS 'Quantidade',
                                            T1.UDF1 AS 'Status',
                                            T0.ITEMNAME AS 'NomeProduto'

                                            FROM OITM T0
                                            INNER JOIN BEAS_FTPOS T1 ON T0.ITEMCODE = T1.ITEMCODE
                                          
                                                  
                                            WHERE T1.BELNR_ID = '{order}'";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var result = new DataTable();
                        result.Load(dr);
                        conn.Close();
                        return result;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public void UpdateStatus(string order)
        {
            try
            {
                using (conn)
                {
                    PrepareConnection();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"UPDATE BEAS_FTPOS SET UDF1 = 'ORDEM IMPRESSA' WHERE BELNR_ID = {order}";
                    cmd.ExecuteReader();
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public void PrepareConnection()
        {
            conn.Close();
            conn.ConnectionString = connectionString;
            conn.Open();
        }
    }
}