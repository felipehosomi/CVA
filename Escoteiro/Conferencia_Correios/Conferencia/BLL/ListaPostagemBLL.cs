

using Conferencia.DAO;
using Conferencia.Model;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;

namespace Conferencia.BLL
{
    public class ListaPostagemBLL
    {

        public List<ListaPostagemModel> GetInfo(string TipoDespacho, string Transportadora, string DataInicial, string DataFinal)
        {
            var lista = new List<ListaPostagemModel>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarregaListaPostagem,TipoDespacho,Transportadora,DataInicial,DataFinal), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new ListaPostagemModel();

                    model.Cliente = row["Cliente"].ToString();
                    model.Faturaemnto = Convert.ToDateTime(row["Data Faturamento"].ToString());                    
                    model.Serial = Convert.ToInt32(row[" N°NF"].ToString());

                    lista.Add(model);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lista;
        }

        public List<ComboTransp> GetCombo()
        {
            var lista = new List<ComboTransp>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarregaComboTrnsp), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new ComboTransp();

                    model.CardCode = row["CardCode"].ToString();
                    model.CardName = row["CardName"].ToString();

                    lista.Add(model);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lista;
        }

        public List<ComboTransp> GetComboDespacho()
        {
            var lista = new List<ComboTransp>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarregaComboDesp), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new ComboTransp();

                    model.CardName = row["CardName"].ToString();

                    lista.Add(model);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lista;
        }
    }

}
