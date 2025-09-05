using Conferencia.DAO;
using Conferencia.Model;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;

namespace Conferencia.BLL
{
    public class PedidoBLL
    {

        #region 1° Conferência

        public List<PedidoModel> GetCardCode(string DocEntry)
        {
            var lista = new List<PedidoModel>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.GetCardCode, DocEntry), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new PedidoModel();

                    model.Cliente = row["Cliente"].ToString();
                    model.DocStatus = row["DocStatus"].ToString();
                    model.TipoPag = row["U_CVA_TipoPag"].ToString();
                    model.ItemCode = row["ItemCode"].ToString();
                    model.Dscription = row["Dscription"].ToString();
                    model.ParcFaturado = row["ParcFaturado"].ToString();
                    model.ParcAntecipado = row["ParcAntecipado"].ToString();
                    model.CardCode = row["CardCode"].ToString();

                    model.DocNum = Convert.ToInt32(row["DocNum"]);
                    model.Qtd_pedido = Convert.ToInt32(row["OpenCreQty"]);

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

        public List<PedidoModel> VerificaItemPedido(string DocEntry, string ItemCode)
        {
            var lista = new List<PedidoModel>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.VerificaItemPedido, DocEntry, ItemCode), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new PedidoModel();

                    model.Cliente = row["Cliente"].ToString();
                    model.DocStatus = row["DocStatus"].ToString();
                    model.TipoPag = row["U_CVA_TipoPag"].ToString();
                    model.ItemCode = row["ItemCode"].ToString();
                    model.Dscription = row["Dscription"].ToString();
                    model.ParcFaturado = row["ParcFaturado"].ToString();
                    model.CardCode = row["CardCode"].ToString();
                    model.CodBarras = row["CodBarras"].ToString();
                    model.ParcAntecipado = row["ParcAntecipado"].ToString();

                    model.DocNum = Convert.ToInt32(row["DocNum"]);
                    model.Qtd_pedido = Convert.ToInt32(row["OpenCreQty"]);

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

        public List<CabecalhoPedidoModel> CarregCabecalhoPedido(string DocNum)
        {
            var lista = new List<CabecalhoPedidoModel>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CabecalhoPedido, DocNum), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new CabecalhoPedidoModel();

                    model.Cliente = row["Cliente"].ToString();
                    model.Transportadora = row["Transportadora"].ToString();
                    model.Origem = row["Origem"].ToString();

                    model.N_Pedido = Convert.ToInt32(row["DocNum"]);

                    model.Data_Entrega = Convert.ToDateTime(row["DocDueDate"]);
                    model.Data_Pedido = Convert.ToDateTime(row["DocDate"]);

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

        public List<GriConf> CarregaGridConf1(string DocNum)
        {
            var lista = new List<GriConf>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.Itens1Conf, DocNum), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new GriConf();

                    model.Codbarras = row["CodBarras"].ToString();
                    model.ItemCode = row["ItemCode"].ToString();
                    model.ItemName = row["Dscription"].ToString();

                    model.Quantidade = Convert.ToInt32(row["Quantidade"]);
                    //model.Saldo = Convert.ToInt32(row["Saldo"]);
                    model.Escaneado = Convert.ToInt32(row["Escaneado"]);

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

        public void Insert_1Conferencia(int Docentry, string dtInicial1, string dtFinal1, string dtInicial2, string dtFinal2, string status1, string status2, string docstaus)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.Insert_1Conferencia, Docentry, dtInicial1, dtFinal1, dtInicial2, dtFinal2, status1, status2, docstaus), conn);
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

        public void Insert_1ConferenciaLinha(int Docentry, string CodBarras, int LinNum, string Itemcode, string ItemName, int Quantidade1, int Quantidade2, string user1, string user2, string data1, string hora1, string data2, string hora2)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.Insert_1ConferenciaLinha, Docentry, CodBarras, LinNum, Itemcode, ItemName.Replace("'","''"), Quantidade1, Quantidade2, user1, user2, data1, hora1, data2, hora2), conn);
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

        public void SetAberturaPicking1Conf(int DocNum, string Usuario, string Abertura)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.SetAberturaPicking1Conf, Usuario, Abertura, DocNum), conn);
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

        public void InsereStatusEmSeparação(int DocNum)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.InsereStatusEmSeparação, DocNum), conn);
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

        public void SetFechamentoPicking1Conf(int DocNum, string Abertura)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.SetFechamentoPicking1Conf, Abertura, DocNum), conn);
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

        public void SetAberturaPicking2Conf(int DocNum, string Usuario, string Abertura)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.SetAberturaPicking2Conf, Usuario, Abertura, DocNum), conn);
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

        public void SetFechamentoPicking2Conf(int DocNum, string Abertura)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.SetFechamentoPicking2Conf, Abertura, DocNum), conn);
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

        #region Finaliza Parcial

        public void Insert_1ConferenciaParcial(int Docentry, string dtInicial1, string dtFinal1, string dtInicial2, string dtFinal2, string status1, string status2, string docstaus)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.Insert_1ConferenciaParcial, Docentry, dtInicial1, dtFinal1, dtInicial2, dtFinal2, status1, status2, docstaus), conn);
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

        public void Insert_1ConferenciaLinhaParcial(int Docentry, string CodBarras, int LinNum, string Itemcode, string ItemName, int QuantidadePedido, int Quantidade1, int Quantidade2, string user1, string user2, string data1, string hora1, string data2, string hora2)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.Insert_1ConferenciaLinhaParcial, Docentry, CodBarras, LinNum, Itemcode, ItemName.Replace("'","''"), QuantidadePedido, Quantidade1, Quantidade2, user1, user2, data1, hora1, data2, hora2), conn);

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


        public void Insert_2ConferenciaParcial(int Docentry, string dtInicial1, string dtFinal1, string dtInicial2, string dtFinal2, string status1, string status2, string docstaus)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.Insert_1ConferenciaParcial, Docentry, dtInicial1, dtFinal1, dtInicial2, dtFinal2, status1, status2, docstaus), conn);
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

        public void Insert_2ConferenciaLinhaParcial(int Docentry, string CodBarras, int LinNum, string Itemcode, string ItemName, int QuantidadePedido, int Quantidade1, int Quantidade2, string user1, string user2, string data1, string hora1, string data2, string hora2)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.Insert_1ConferenciaLinhaParcial, Docentry, CodBarras, LinNum, Itemcode, ItemName.Replace("'", "''"), QuantidadePedido, Quantidade1, Quantidade2, user1, user2, data1, hora1, data2, hora2), conn);
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

        public int VerificaParcial(string DocNum)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);
            int qtd = 0;

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.VerificaParcial, DocNum), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    qtd = Convert.ToInt32(row["qtde"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return qtd;
        }

        public List<GriConf> CarregaParcial(string DocNum)
        {
            var lista = new List<GriConf>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarregaParcial, DocNum), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new GriConf();

                    model.Codbarras = row["CVA_CODBARRAS"].ToString();
                    model.ItemCode = row["CVA_ITEMCODE"].ToString();
                    model.ItemName = row["CVA_DESCRICAO"].ToString();

                    model.Quantidade = Convert.ToInt32(row["CVA_QUANTIDADEPEDIDO"]);
                    //model.Saldo = Convert.ToInt32(row["Saldo"]);
                    model.Escaneado = Convert.ToInt32(row["CVA_QUANT_1"]);

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

        public List<GriConf> CarregaParcial2(string DocNum)
        {
            var lista = new List<GriConf>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarregaParcial_2, DocNum), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new GriConf();

                    model.Codbarras = row["CVA_CODBARRAS"].ToString();
                    model.ItemCode = row["CVA_ITEMCODE"].ToString();
                    model.ItemName = row["CVA_DESCRICAO"].ToString();

                    model.Quantidade = Convert.ToInt32(row["CVA_QUANTIDADEPEDIDO"]);
                    //model.Saldo = Convert.ToInt32(row["Saldo"]);
                    model.Escaneado = Convert.ToInt32(row["CVA_QUANT_2"]);

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

        public void DeleteParcial(int DocNum)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.DeleteParcial, DocNum), conn);
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

        public void DeleteParcialItens(int DocNum)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.DeleteParcialItens, DocNum), conn);
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

        public void Update_Parcial(int Docentry, string dtInicial2, string dtFinal2, string status2)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.Update_Parcial, dtInicial2, dtFinal2, status2, Docentry), conn);
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

        public void Update_PAarcialLinha(int Docentry, int LinNum, string Itemcode, int Quantidade2, string user2, string data2, string hora2)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.Update_Parcial_Linha, Quantidade2, user2, data2, hora2, Docentry, Itemcode), conn);
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

        public void Update_Parcial2(int Docentry, string dtInicial2, string dtFinal2, string status2)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.Update_Parcial2, dtInicial2, dtFinal2, status2, Docentry), conn);
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

        public void Update_2PAarcialLinha(int Docentry, int LinNum, string Itemcode, int Quantidade2, string user2, string data2, string hora2)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.Update_Parcial_Linha2, Quantidade2, user2, data2, hora2, Docentry, Itemcode), conn);
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


        #endregion

        #endregion

        #region 2º Conferência

        public string GetStatusConf(string DocEntry)
        {
            string Status = string.Empty;
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.StatusConf, DocEntry), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    Status = row["CVA_STATUS_1"].ToString();
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

            return Status;
        }

        public List<ListaConferidaModel> LitsConf(string DocEntry, string Codbarras)
        {
            var lista = new List<ListaConferidaModel>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.ListConf1, DocEntry, Codbarras), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new ListaConferidaModel();

                    model.ItemCode = row["CVA_ITEMCODE"].ToString();
                    model.ItemName = row["CVA_DESCRICAO"].ToString();
                    model.Quantidade = Convert.ToInt32(row["CVA_QUANT_1"]);
                    model.CodBarras = row["CVA_CODBARRAS"].ToString();

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

        public void Update_2Conferencia(int Docentry, string dtInicial2, string dtFinal2, string status2)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.Update_2Conferencia, dtInicial2, dtFinal2, status2, Docentry), conn);
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

        public void Update_2ConferenciaLinha(int Docentry, int LinNum, string Itemcode, int Quantidade2, string user2, string data2, string hora2)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                string sql = string.Format(Query.Update_2ConferenciaLinha, Quantidade2, user2, data2, hora2, Docentry, Itemcode);
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
        #endregion



        #region Lista de Pedidos

        #region Combos Box

        public List<Status> CarregaComboStatus()
        {
            var lista = new List<Status>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarregaComboStatus), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new Status();

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

        public List<Filial> CarregaComboFilial()
        {
            var lista = new List<Filial>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarregaComboFilial), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new Filial();

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

        public List<UF_Cliente> CarregaComboUfCliente()
        {
            var lista = new List<UF_Cliente>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarregaComboxUfCliente), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new UF_Cliente();

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

        #endregion

        public List<ListaPedidoModel> ListaPedidosLiberado(int Status, int Filial, int TipoEnvio, string UfCliente, string DataDe, string DataAte)
        {
            var lista = new List<ListaPedidoModel>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                string sql = string.Format(Query.ListaPedidosAbertos, Status, Filial, TipoEnvio, UfCliente, DataDe, DataAte);
                HanaDataAdapter dataAdapter = new HanaDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new ListaPedidoModel();

                    model.N_Pedido = Convert.ToInt32(row["N_Pedido"]);
                    model.Origem = row["Origem"].ToString();
                    model.Cliente = row["Cliente"].ToString();
                    model.NomeFantasia = row["NomeFantasia"].ToString();
                    model.Data_Pedido = Convert.ToDateTime(row["Data_Pedido"].ToString());
                    model.Total_Pedido = Convert.ToDouble(row["Total_Pedido"]);
                    model.Forma_Pagamento = row["Forma_Pagamento"].ToString();
                    model.Transportadora = row["Transportadora"].ToString();
                    model.Status = row["Status"].ToString();

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

        public List<ListaPedidoModelPendente> ListaPedidosPendentes(int Status, int Filial, int TipoEnvio, string UfCliente, string DataDe, string DataAte)
        {
            var lista = new List<ListaPedidoModelPendente>();
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                string sql = string.Format(Query.ListaPedidosPendente, Status, Filial, TipoEnvio, UfCliente, DataDe, DataAte);
                HanaDataAdapter dataAdapter = new HanaDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new ListaPedidoModelPendente();

                    model.N_Pedido = Convert.ToInt32(row["N_Pedido"]);
                    model.Origem = row["Origem"].ToString();
                    model.Cliente = row["Cliente"].ToString();
                    model.NomeFantasia = row["NomeFantasia"].ToString();
                    model.Data_Pedido = Convert.ToDateTime(row["Data_Pedido"].ToString());
                    model.Total_Pedido = Convert.ToDouble(row["Total_Pedido"]);
                    model.Transportadora = row["Transportadora"].ToString();
                    model.Status = row["Status"].ToString();
                    model.Forma_Pagamento = row["Forma_Pagamento"].ToString();

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


        public void AtualizaPickingPedido(string DocEntry)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                string sql = string.Format(Query.AtualizaPickingPedido, DocEntry);
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

        public void AtualizaGestaoPickingPedido(string DocEntry)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                string sql = string.Format(Query.AtualizaGestaoPickingPedido, DocEntry);
                HanaCommand _HanaCommand = new HanaCommand(sql, conn);
                _HanaCommand.ExecuteNonQuery();

                sql = string.Format(Query.DeleteParcial, DocEntry);
                _HanaCommand = new HanaCommand(sql, conn);
                _HanaCommand.ExecuteNonQuery();

                sql = string.Format(Query.DeleteParcialItens, DocEntry);
                _HanaCommand = new HanaCommand(sql, conn);
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

        #endregion


    }
}
