using Conferencia.DAO;
using Conferencia.Model;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conferencia.BLL
{
    public class ListaConferenciaBLL
    {

        public List<ListaConferecia> GetListaConf1()
        {
            var lista = new List<ListaConferecia>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.GetList1conf), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new ListaConferecia();

                    model.Cliente = row["CardName"].ToString();
                    model.Data_Entrega = Convert.ToDateTime(row["DocDueDate"].ToString());
                    model.Data_Pedido = Convert.ToDateTime(row["DocDate"].ToString());
                    model.Filial = row["BPLName"].ToString();
                    model.Origem = row["Origem"].ToString();
                    model.Transportadora = row["Transportadora"].ToString();
                    model.N_Pedido = Convert.ToInt32(row["DocNum"]);
                    

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

        public List<ListaConferecia> GetListaConf2()
        {
            var lista = new List<ListaConferecia>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.GetList2Conf), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new ListaConferecia();

                    model.Cliente = row["CardName"].ToString();
                    model.Data_Entrega = Convert.ToDateTime(row["DocDueDate"].ToString());
                    model.Data_Pedido = Convert.ToDateTime(row["DocDate"].ToString());
                    model.Filial = row["BPLName"].ToString();
                    model.Origem = row["Origem"].ToString();
                    model.Transportadora = row["Transportadora"].ToString();
                    model.N_Pedido = Convert.ToInt32(row["DocNum"]);


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
