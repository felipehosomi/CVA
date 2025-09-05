using CVA.Core.DSP.Controle.Auxiliar;
using Dover.Framework.DAO;
using SAPbobsCOM;
using System.Data;
using System.Data.SqlClient;

namespace CVA.Core.DSP.Controle.DAO
{
    public class CalculaLoteDAO
    {
        private BusinessOneDAO _businessOneDAO { get; set; }
        public SAPbouiCOM.Application _application { get; set; }
        public Company _company { get; set; }
        SqlConnection conn { get; set; }
        public XMLReader xmlRead { get; set; }

        public CalculaLoteDAO()
        {
            xmlRead = new XMLReader();
            conn = new SqlConnection();
            OpenConnection();

        }


        public void OpenConnection()
        {
            conn.Close();
            conn.ConnectionString = xmlRead.readConnectionString();
            conn.Open();

        }

        #region GetNextLote

        public DataTable GetNextLote()
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"select U_DSP_NUM_LOTE as 'code' from owor where DocEntry=(select max(DocEntry) from OWOR)";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetNewLote

        public DataTable GetNewLote(string novolote)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"select U_DSP_NUM_LOTE +  U_CVA_SubLote as 'novolote' from OWOR where DocNum = '{novolote}'";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
