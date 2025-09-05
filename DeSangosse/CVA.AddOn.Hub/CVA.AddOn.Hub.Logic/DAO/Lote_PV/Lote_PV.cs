using CVA.AddOn.Common;
using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Hub.Logic.DAO.Resource;
using CVA.AddOn.Hub.Logic.MODEL;
using CVA.Core.DSP.Controle.Auxiliar;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CVA.AddOn.Hub.Logic.DAO.Lote
{
    public class Lote_PV
    {
        
        SqlConnection conn { get; set; }
        XMLReader xmlReader { get; set; }

        public Lote_PV()
        {
            xmlReader = new XMLReader();
            conn = new SqlConnection();
            OpenConnection();
        }
        


        public void OpenConnection()
        {
            conn.Close();
            conn.ConnectionString = xmlReader.readConnectionString();
            conn.Open();

        }



        public void InsertLote(int i, LotesModel model)
        {
            Recordset rst = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            string sql = String.Format(Query.InsereLote, model.Item + model.Lote, model.Item + model.Lote, model.Item, model.Lote);
            rst.DoQuery(sql);

        }


        public void ApagaDadosTabelaLote()
        {
            Recordset rst = (Recordset)SBOApp.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
            string sql = String.Format(Query.ApagaDadosTabelaLote);
            rst.DoQuery(sql);
        }


        public DataTable pegaLote()
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = @"select U_ItemCode,U_Lote from [@LOTES_PV]";

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
    }
}
