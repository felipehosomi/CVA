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
    public class GestaoPickingBLL
    {
        #region Carrega ComboBox        

        public List<Data> CarregaComboData()
        {
            var lista = new List<Data>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarregaComboData), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new Data();

                    model.Code = row["Code"].ToString();
                    model.Descricao = row["Descricao"].ToString();

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

        public List<Transportadora> CarregaComboTransportadora()
        {
            var lista = new List<Transportadora>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarregaComboBoxTransportadora), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new Transportadora();

                    model.Code = row["Code"].ToString();
                    model.Descricao = row["Descricao"].ToString();

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

        public List<Tipo_Envio> CarregaComboTipoEnvio()
        {
            var lista = new List<Tipo_Envio>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarrehaComboTipoEnvio), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new Tipo_Envio();

                    model.Code = row["Code"].ToString();
                    model.Descricao = row["Descricao"].ToString();

                    lista.Add(model);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return lista;
        }

        public List<_Status> CarregaCombo_Status()
        {
            var lista = new List<_Status>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarregaCB_Status), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new _Status();

                    model.Code = row["Code"].ToString();
                    model.Descricao = row["Descricao"].ToString();

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

        #endregion

        public List<GestaoPickingModel> ListaPicking(string DataInicial, string DataFinal, int Filial, int Status, int TipoEnvio, int N_Pedido)
        {
            var lista = new List<GestaoPickingModel>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                string sql = string.Format(Query.ListaPicking,DataInicial,DataFinal,Filial,Status,TipoEnvio,N_Pedido);
                HanaDataAdapter dataAdapter = new HanaDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new GestaoPickingModel();

                    model.N_Pedido = Convert.ToInt32(row["DocNum"]);
                    model.Origem = row["Origem"].ToString();
                    model.Cliente = row["Cliente"].ToString();
                    model.DocDate = Convert.ToDateTime(row["DocDate"].ToString());
                    model.Transportadora = row["Transportadora"].ToString();
                    model.NomeFantasia = row["NomeFantasia"].ToString();
                    model.E_Mail = row["E_Mail"].ToString();
                    model.AberturaPicking =row["AberturaPicking"].ToString();
                    model.UsuarioPicking = row["UsuarioPicking"].ToString();
                    model.StatusPicking = row["StatusPicking"].ToString();
                    model.Impresso = row["Impresso"].ToString();
                    model.CodigoRastreio = row["CodigoRastreio"].ToString();
                    model.N_PedidoMagento = Convert.ToInt32(row["PedidoMagento"].ToString());

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

        public void AtualizaPickingPedido(string DocEntry)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                string sql = string.Format(Query.AtualizaPickingPedidoVenda, DocEntry);
                HanaCommand _HanaCommand = new HanaCommand(sql, conn);
                _HanaCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public void AtualizaPedidoImpresso(string DocEntry)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                string sql = string.Format(Query.AtualizaImpressaoPedido, DocEntry);
                HanaCommand _HanaCommand = new HanaCommand(sql, conn);
                _HanaCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
