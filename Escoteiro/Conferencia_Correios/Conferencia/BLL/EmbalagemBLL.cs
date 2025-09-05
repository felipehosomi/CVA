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
    public class EmbalagemBLL
    {
        public List<EmbalagemModel> GetEmbalagem()
        {
            var lista = new List<EmbalagemModel>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.GetTipoEmbalagem), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    var model = new EmbalagemModel();

                    model.Codigo = Convert.ToInt32(row["Code"].ToString());
                    model.Name = row["Name"].ToString();

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

        public string GetOrderWeight(int docNum)
        {
            var weight = "0,0";
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(String.Format(Query.OrderWeight_Get, docNum), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    weight = row["GrsWeight"].ToString();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return weight;
        }

        public void Insert_Volume(int Docentry, string dtInicial1)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                HanaCommand _HanaCommand = new HanaCommand(string.Format(Query.Insert_Volumes, Docentry, dtInicial1), conn);
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

        public void Insert_VolumeLinha(int Docentry, int LinNum, int Id_vol, int Quantidade, string peso)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                string sql = string.Format(Query.Insert_VolumesLinha, Docentry, LinNum, Id_vol, Quantidade,peso);
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

        public void Update_Volume(int Docentry, string dtInicial1)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                string sql = string.Format(Query.Upadate_Volumes, dtInicial1, Docentry);
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

        public void Update_CodigoRastreio(int Docentry, string codidoRastreio)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                string sql = string.Format(Query.UpdateCodigoRastreio, codidoRastreio, Docentry);
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

        public bool VerificaExisteCodRastreio(string codidoRastreio)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);
            conn.Open();

            HanaDataAdapter DataAdapter = new HanaDataAdapter(string.Format(Query.VerificaExisteCodRastreio, codidoRastreio), conn);

            DataTable result = new DataTable();
            DataAdapter.Fill(result);

            if (result.Rows.Count > 0)
            {
                conn.Close();
                return true;
            }

            else
            {
                conn.Close();
                return false;
            }
        }

        public bool Verifica_Docentry(int DocEntry)
        {

            //aLog.WriteLog("Verificando se o SMS já foi Respondido ao cliente  Pedido: " + DocEntry);
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);
            conn.Open();

            HanaDataAdapter DataAdapter = new HanaDataAdapter(string.Format(Query.Verifica_Existes_DocEntry, DocEntry), conn);

            DataTable result = new DataTable();
            DataAdapter.Fill(result);

            if (result.Rows.Count > 0)
            {
                conn.Close();
                return true;
            }

            else
            {
                conn.Close();
                return false;
            }
        }

        public int Max(int docNum)
        {
            int Qtde = 0;
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.Max_Linhas, docNum), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    Qtde = 0;
                    if (!string.IsNullOrEmpty(row["Qtd"].ToString()))
                    {
                        Qtde = Convert.ToInt32(row["Qtd"].ToString());
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Qtde;
        }

        public int Qdte_ItensPedido(int docentry)
        {
            int Qtde = 0;
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.Qtde_ItensPedido, docentry), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {

                    Qtde = Convert.ToInt32(row["Qtd"].ToString());

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Qtde;
        }

        public int Qtde_Embalagem(string docentry)
        {
            int Qtde = 0;
            string _qtde = string.Empty;
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                string sql = string.Format(Query.QuantidadeEmbalagem, docentry);

                HanaDataAdapter dataAdapter = new HanaDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {

                    //_qtde = .Replace(",",".");

                    Qtde = int.Parse(row[0].ToString());

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Qtde;
        }

        public List<Serie> GetSerie(int DocEntry)
        {
            var lista = new List<Serie>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.GetSerie, DocEntry), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new Serie();
                    int serieAtual = 0;
                    int serieFinal = 0;
                    int.TryParse(row["U_CVA_SerieAtu"].ToString(), out serieAtual);
                    int.TryParse(row["U_CVA_SerieFin"].ToString(), out serieFinal);
                    model.Serie_Atual = serieAtual;
                    model.Serie_Final = serieFinal;
                    model.TipoDoc = !(row["TipoDoc"].ToString() == "") ? Convert.ToInt32(row["TipoDoc"].ToString()) : 1;
                    model.DigTrackNumber = Convert.ToInt32(row["U_CVA_DigTrackNumber"].ToString());
                    model.SufixoCodRastreio = row["U_CVA_SufixoCodRastreio"].ToString();
                    model.TipoServico = row["U_CVA_TipoServico"].ToString();

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

        public string GetTransportationName(int docNum)
        {
            var trnspName = String.Empty;

            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.TrnspName_Get, docNum), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    trnspName = row["TrnspName"].ToString();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return trnspName;
        }

        public string VerificaUtilização(int DocEntry)
        {
            string message = "";
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);
            conn.Open();

            HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.VerificaUtilizacao, DocEntry), conn);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            foreach (DataRow row in dt.Rows){
                if(row["Usage"].ToString() == "")
                {
                    message = "Verifique se foi informado a utilização do item no pedido.";
                    return message;

                }
            }

            return message;
        }

        public List<NfModel> GetNF(int DocEntry)
        {
            var lista = new List<NfModel>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.GetNF, DocEntry), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new NfModel();

                    model.DocType       = Convert.ToInt32(row["DocType"].ToString());
                    model.DocEntry      = Convert.ToInt32(row["DocEntry"].ToString());
                    model.DocLine       = Convert.ToInt32(row["DocLine"].ToString());
                    model.SysNumber     = Convert.ToInt32(row["SysNumber"].ToString());
                    model.LocCode       = Convert.ToInt32(row["LocCode"].ToString());
                    model.Usage         = Convert.ToInt32(row["Usage"].ToString());
                    model.BPLId         = Convert.ToInt32(row["BPLId"].ToString());
                    model.DocSubLine    = Convert.ToInt32(row["DocSubLine"].ToString());

                    model.QtySelected  = Convert.ToDouble(row["QtySelected"].ToString());
                    model.QtdCommit    = Convert.ToDouble(row["QtdCommit"].ToString());
                    model.QtyOnHand    = Convert.ToDouble(row["QtyOnHand"].ToString());

                    model.ManagedBy    = row["ManagedBy"].ToString();
                    model.ItemCode     = row["ItemCode"].ToString();
                    model.WhsCode      = row["WhsCode"].ToString();
                    model.DistNumber   = row["DistNumber"].ToString();
                    model.CardCode     = row["CardCode"].ToString();
                    model.MnfSerial    = row["MnfSerial"].ToString();

                    model.NfeTipoEnv = row["U_nfe_tipoEnv"].ToString();

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

        public List<int> GetDownPaymentDocEntry(int orderEntry)
        {
            List<int> list = new List<int>();

            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.DownPayment_Get, orderEntry), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    list.Add(Convert.ToInt32(row["DocEntry"].ToString()));
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public void Update_Serie(int Docentry, int serie)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                string sql = string.Format(Query.UpdateSerie, serie, Docentry);
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

        public void DeleteLinhaEmbalagem(int Docentry, int lineNum)
        {
            HanaConnection conn;
            conn = new HanaConnection(Properties.Settings.Default.ConectionString);

            try
            {
                conn.Open();

                string sql = string.Format(Query.Delete_LinhaEmbalagem, Docentry, lineNum);
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

        public List<NfModel> GetNFSemLote(int DocEntry)
        {
            var lista = new List<NfModel>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.getNFSemLote, DocEntry), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new NfModel();

                   
                    model.DocEntry = Convert.ToInt32(row["DocEntry"].ToString());
                    model.Usage = Convert.ToInt32(row["Usage"].ToString());
                    model.BPLId = Convert.ToInt32(row["BPLId"].ToString());       
                    model.ItemCode = row["ItemCode"].ToString();
                    model.CardCode = row["CardCode"].ToString();
                    

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

        public List<ListaEmbalagemModel> CarregaListaEmbalagem(int DocNum)
        {
            var lista = new List<ListaEmbalagemModel>();
            try
            {
                HanaConnection conn;
                conn = new HanaConnection(Properties.Settings.Default.ConectionString);
                conn.Open();

                HanaDataAdapter dataAdapter = new HanaDataAdapter(string.Format(Query.CarregaListaEmbagem, DocNum), conn);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);


                foreach (DataRow row in dt.Rows)
                {
                    var model = new ListaEmbalagemModel();

                    model.Pedido = Convert.ToInt32(row["Pedido"].ToString());
                    model.Linha = Convert.ToInt32(row["Nº Linha"].ToString());
                    model.Tipo_Embalagem = row["Tipo Embalagem"].ToString();
                    model.Quantidade = row["Quantidade"].ToString();
                    model.Peso = decimal.Parse(row["Peso"].ToString());
                   

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
