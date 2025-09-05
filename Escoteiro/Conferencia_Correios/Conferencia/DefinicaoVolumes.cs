using Conferencia.BLL;
using Conferencia.ConexaoSAP;
using Conferencia.DAO;
using Conferencia.Model;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conferencia
{
    public partial class DefinicaoVolumes : Form
    {
        EmbalagemBLL _EmbalagemBLL = new EmbalagemBLL();
        PedidoBLL _PedidoBLL = new PedidoBLL();
        public string p = string.Empty;
        public string strSql = string.Empty;
        public string msgError = string.Empty;
        public int error = 0;
        public string Docnum = string.Empty;

        public DefinicaoVolumes(string pedido)
        {
            InitializeComponent();
            p = pedido;
            CarregarGrid(Convert.ToInt32(p));
        }

        private void CarregaComboBox()
        {
            cb_Embalagem.DataSource = _EmbalagemBLL.GetEmbalagem();
            cb_Embalagem.DisplayMember = "Name";
            cb_Embalagem.ValueMember = "Codigo";
        }

        private void DefinicaoVolumes_Load(object sender, EventArgs e)
        {
            tx_pedido.Text = p;
            CarregaComboBox();
            cb_Embalagem.SelectedIndex = 1;
            tx_peso.Text = _EmbalagemBLL.GetOrderWeight(Convert.ToInt32(p));
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (cb_Embalagem.SelectedIndex == 0 || string.IsNullOrEmpty(tx_peso.Text) || string.IsNullOrEmpty(tx_quantidade.Text))
            {
                if (cb_Embalagem.SelectedIndex == 0)
                {
                    MessageBox.Show("Tipo de Embalagem Obrigatório!", "Alerta");
                }
                else
                {
                    if (string.IsNullOrEmpty(tx_peso.Text))
                    {
                        MessageBox.Show("Peso Obrigatório!", "Alerta");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(tx_quantidade.Text))
                        {
                            MessageBox.Show("Quantidade Obrigatório!", "Alerta");
                        }
                    }
                }
            }
            else
            {

                var t = cb_Embalagem.SelectedIndex;

                string dataConf = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                int docNum = Convert.ToInt32(tx_pedido.Text);

                int lineNum = _EmbalagemBLL.Max(docNum) + 1;

                var exists = _EmbalagemBLL.Verifica_Docentry(docNum);

                var peso = tx_peso.Text.Replace(',', '.');

                if (!exists)
                {
                    // Insert Cabeçalho
                    _EmbalagemBLL.Insert_Volume(docNum, dataConf);
                    _EmbalagemBLL.Insert_VolumeLinha(docNum, lineNum, t, Convert.ToInt32(tx_quantidade.Text), peso);

                    LimpaCampos();
                }
                else
                {
                    _EmbalagemBLL.Update_Volume(docNum, dataConf);
                    _EmbalagemBLL.Insert_VolumeLinha(docNum, lineNum, t, Convert.ToInt32(tx_quantidade.Text), peso);

                    LimpaCampos();
                }

                CarregarGrid(docNum);

                btn_Doc.Enabled = true;
            }
        }

        private void CarregarGrid(int docNum)
        {
            var listaEmbalagem = _EmbalagemBLL.CarregaListaEmbalagem(docNum);

            dataGridEbalagem.DataSource = null;
            dataGridEbalagem.DataSource = listaEmbalagem;

            dataGridEbalagem.Columns[0].Width = 90;
            dataGridEbalagem.Columns[1].Width = 90;
            dataGridEbalagem.Columns[2].Width = 120;
            dataGridEbalagem.Columns[3].Width = 90;
            dataGridEbalagem.Columns[4].Width = 100;
            //dataGridEbalagem.Columns[4].DefaultCellStyle.Format = "N2";

            dataGridEbalagem.Refresh();

            if (listaEmbalagem.Count > 0)
                btn_Doc.Enabled = true;
        }

        private void LimpaCampos()
        {
            cb_Embalagem.SelectedIndex = 0;

            tx_peso.Text = "";
            tx_quantidade.Text = "";
        }

        private void btn_Doc_Click(object sender, EventArgs e)
        {
            var transpName = _EmbalagemBLL.GetTransportationName(Convert.ToInt32(tx_pedido.Text));
            var list = new List<Serie>();
            string message = _EmbalagemBLL.VerificaUtilização(Convert.ToInt32(tx_pedido.Text));
            if (message != "")
            {
                MessageBox.Show(message);
                return;
            }

            if (transpName.StartsWith("TRANSPORTADORA") || transpName == "RETIRA LOJA")
            {
                list.Add(new Serie { TipoDoc = 1, TipoServico = "", SufixoCodRastreio = "", Serie_Atual = 1, Serie_Final = 1000 });
            }
            else
            {
                list = _EmbalagemBLL.GetSerie(Convert.ToInt32(tx_pedido.Text));
            }

            if (list.Count <= 0 || list[0].Serie_Atual == 0 || list[0].Serie_Final == 0)
            {
                MessageBox.Show("Verifique se todas as informações abaixo estão preenchidas:\n\nTransportadora definida no pedido\nSérie no cadastro de despacho.", "Não é possível continuar");
            }
            else
            {
                Recordset rstUpdateNF = (Recordset)Conexao.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                foreach (var item in list)
                {
                    var serie = item.Serie_Atual + 1;
                    //string NewCodRastreio = string.Empty;
                    //if (!transpName.StartsWith("TRANSPORTADORA") && transpName != "RETIRA LOJA")
                    //{
                    //    //completa os zeros caso esteja faltando
                    //    string serieAtual = serie.ToString();
                    //    if (serieAtual.Length < 8){
                    //        String zeros = "";
                    //        int diferenca = 8 - serieAtual.Length;
                    //        for (int i = 0; i < diferenca; i++)
                    //        {
                    //            zeros += "0";
                    //        }
                    //        serieAtual = zeros + serieAtual;

                    //    }
                    //    string CodRastreio = item.TipoServico + serieAtual + item.SufixoCodRastreio;

                    //     NewCodRastreio = geraEtiquetaComDigitoVerificador(CodRastreio);
                    //}


                    if (serie > item.Serie_Final)
                    {
                        MessageBox.Show("Código de Rastreio esgotado.", "ERRO!");
                        break;
                    }
                    else
                    {

                        int count = 0;

                        Documents _Invoices = null;

                        _Invoices = (Documents)Conexao.oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                        var order = (Documents)Conexao.oCompany.GetBusinessObject(BoObjectTypes.oOrders);

                        _Invoices.DocDate = DateTime.Now;
                        _Invoices.DocDueDate = DateTime.Now;

                        var listaNf = _EmbalagemBLL.GetNF(Convert.ToInt32(tx_pedido.Text));
                        var listaNfSemLote = _EmbalagemBLL.GetNFSemLote(Convert.ToInt32(tx_pedido.Text));

                        if (listaNf.Count == 0)
                        {
                            MessageBox.Show("Existe Itens no Pedido sem Lote Atribuido ! ", "Alerta!");
                            return;
                        }

                        order.GetByKey(listaNf.First().DocEntry);

                        foreach (var model in listaNf)
                        {
                            //if (count > 0)
                            //{
                            //    _Invoices.Lines.Add();
                            //    _Invoices.Lines.SetCurrentLine(count);
                            //}

                            _Invoices.BPL_IDAssignedToInvoice = model.BPLId;
                            //if (!transpName.StartsWith("TRANSPORTADORA") && transpName != "RETIRA LOJA")
                            //{
                            //    _Invoices.UserFields.Fields.Item("U_CVA_CodRastreio").Value = NewCodRastreio;
                            //}

                            _Invoices.UserFields.Fields.Item("U_CVA_OrderNum").Value = "PEDIDO" + tx_pedido.Text;

                            _Invoices.UserFields.Fields.Item("U_nfe_tipoEnv").Value = model.NfeTipoEnv;


                            //_Invoices.Lines.ItemCode = model.ItemCode;
                            //_Invoices.Lines.Usage = model.Usage.ToString();

                            //Vinculo com o Pedido
                            _Invoices.Lines.BaseEntry = model.DocEntry;
                            _Invoices.Lines.BaseType = 17;
                            _Invoices.Lines.BaseLine = model.DocLine;

                            // Lotes do Pedido para Nota

                            _Invoices.Lines.BatchNumbers.Quantity = Convert.ToDouble(model.QtySelected);
                            _Invoices.Lines.BatchNumbers.BatchNumber = model.DistNumber;
                            _Invoices.Lines.BatchNumbers.BaseLineNumber = count++;

                            _Invoices.Lines.Add();
                        }

                        for (var i = 0; i < order.Expenses.Count; i++)
                        {
                            order.Expenses.SetCurrentLine(i);

                            if (order.Expenses.ExpenseCode == 0) continue;

                            _Invoices.Expenses.BaseDocEntry = order.DocEntry;
                            _Invoices.Expenses.BaseDocLine = order.Expenses.LineNum;
                            _Invoices.Expenses.BaseDocType = 17;
                            _Invoices.Expenses.Add();
                        }

                        var dpEntryList = _EmbalagemBLL.GetDownPaymentDocEntry(order.DocEntry);

                        if (dpEntryList.Count > 0)
                        {
                            foreach (var dpEntry in dpEntryList)
                            {
                                _Invoices.DownPaymentsToDraw.DocEntry = dpEntry;
                                _Invoices.DownPaymentsToDraw.Add();
                            }
                        }

                        _Invoices.AddressExtension.ShipToStreet = order.AddressExtension.ShipToStreet;
                        _Invoices.AddressExtension.ShipToStreetNo = order.AddressExtension.ShipToStreetNo;
                        _Invoices.AddressExtension.ShipToBuilding = order.AddressExtension.ShipToBuilding;
                        _Invoices.AddressExtension.ShipToBlock = order.AddressExtension.ShipToBlock;
                        _Invoices.AddressExtension.ShipToCountry = order.AddressExtension.ShipToCountry;
                        _Invoices.AddressExtension.ShipToZipCode = order.AddressExtension.ShipToZipCode;
                        _Invoices.AddressExtension.ShipToCounty = order.AddressExtension.ShipToCounty;
                        _Invoices.AddressExtension.ShipToCity = order.AddressExtension.ShipToCity;
                        _Invoices.AddressExtension.ShipToState = order.AddressExtension.ShipToState;

                        _Invoices.AddressExtension.BillToStreet = order.AddressExtension.BillToStreet;
                        _Invoices.AddressExtension.BillToStreetNo = order.AddressExtension.BillToStreetNo;
                        _Invoices.AddressExtension.BillToBuilding = order.AddressExtension.BillToBuilding;
                        _Invoices.AddressExtension.BillToBlock = order.AddressExtension.BillToBlock;
                        _Invoices.AddressExtension.BillToCountry = order.AddressExtension.BillToCountry;
                        _Invoices.AddressExtension.BillToZipCode = order.AddressExtension.BillToZipCode;
                        _Invoices.AddressExtension.BillToCounty = order.AddressExtension.BillToCounty;
                        _Invoices.AddressExtension.BillToCity = order.AddressExtension.BillToCity;
                        _Invoices.AddressExtension.BillToState = order.AddressExtension.BillToState;

                        // Quantidade Embalagem Volume
                        int Qtde_embalagem = _EmbalagemBLL.Qtde_Embalagem(tx_pedido.Text);

                        _Invoices.TaxExtension.PackQuantity = Qtde_embalagem;

                        int Qtde = _EmbalagemBLL.Qdte_ItensPedido(Convert.ToInt32(tx_pedido.Text));

                        if (listaNf.Count != Qtde)
                        {
                            MessageBox.Show("Existe Itens no Pedido sem Lote Atribuido ! ", "Alerta!");
                        }
                        else
                        {

                            error = _Invoices.Add();
                            if (error != 0)
                            {
                                Conexao.oCompany.GetLastError(out error, out msgError);
                                MessageBox.Show(error + " - " + msgError, "Erro.");
                            }
                            else
                            {
                                rstUpdateNF.DoQuery(String.Format(Query.AtualizaSubFinalidadeNF, Conexao.oCompany.GetNewObjectKey()));

                                //_EmbalagemBLL.Update_Serie(Convert.ToInt32(tx_pedido.Text), serie);
                                _PedidoBLL.SetFechamentoPicking2Conf(Convert.ToInt32(tx_pedido.Text), DateTime.Now.ToString());
                                if (!transpName.StartsWith("TRANSPORTADORA") && transpName != "RETIRA LOJA")
                                {
                                    if (AdicionaNumeroRastreio())
                                    {
                                        MessageBox.Show("Nota Fiscal de Saída Criada com Sucesso ! ", "Alerta!");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Nota Fiscal de Saída Criada com Sucesso ! ", "Alerta!");
                                }

                                this.Close();
                            }
                        }
                        //else
                        //{
                        //    int count = 0;
                        //    Documents _Delivery = null;

                        //    _Delivery = (Documents)Conexao.oCompany.GetBusinessObject(BoObjectTypes.oDeliveryNotes);

                        //    var listaNf = _EmbalagemBLL.GetNF(Convert.ToInt32(tx_pedido.Text));
                        //    var listaNfItem = _EmbalagemBLL.GetNF(Convert.ToInt32(tx_pedido.Text));


                        //    foreach (var model in listaNf)
                        //    {
                        //        if (count > 0)
                        //        {
                        //            _Delivery.Lines.Add();
                        //            _Delivery.Lines.SetCurrentLine(count);

                        //        }

                        //        _Delivery.CardCode = model.CardCode;
                        //        _Delivery.DocDate = DateTime.Now;
                        //        _Delivery.DocDueDate = DateTime.Now;
                        //        _Delivery.UserFields.Fields.Item("U_CVA_CodRastreio").Value = ;
                        //        _Delivery.BPL_IDAssignedToInvoice = model.BPLId;
                        //        _Delivery.UserFields.Fields.Item("U_CVA_OrderNum").Value = "PEDIDO-" + tx_pedido.Text;



                        //        _Delivery.Lines.ItemCode = model.ItemCode;
                        //        _Delivery.Lines.Usage = model.Usage.ToString();

                        //        //Vinculo com o Pedido
                        //        _Delivery.Lines.BaseEntry = model.DocEntry;
                        //        _Delivery.Lines.BaseType = 17;
                        //        _Delivery.Lines.BaseLine = model.DocLine;

                        //        // Lotes do Pedido para Nota
                        //        _Delivery.Lines.BatchNumbers.Quantity = Convert.ToDouble(model.QtySelected);
                        //        _Delivery.Lines.BatchNumbers.BatchNumber = model.DistNumber;
                        //        _Delivery.Lines.BatchNumbers.BaseLineNumber = count;

                        //        count++;
                        //    }

                        //    int Qtde = _EmbalagemBLL.Qdte_ItensPedido(Convert.ToInt32(tx_pedido.Text));
                        //    if (listaNf.Count != Qtde)
                        //    {
                        //        MessageBox.Show("Existe Itens no Pedido sem Lote Atribuido ! ", "Alerta!");
                        //    }
                        //    else
                        //    {
                        //        error = _Delivery.Add();
                        //        if (error != 0)
                        //        {
                        //            Conexao.oCompany.GetLastError(out error, out msgError);
                        //            MessageBox.Show(error + " - " + msgError, "Erro.");
                        //        }
                        //        else
                        //        {
                        //            MessageBox.Show("Entrega Criada com Sucesso! ", "Alerta!");
                        //            _EmbalagemBLL.Update_Serie(Convert.ToInt32(tx_pedido.Text), serie);
                        //            this.Close();
                        //        }
                        //    }
                        //}
                    }

                }
            }
        }

        private bool AdicionaNumeroRastreio()
        {
            var list = _EmbalagemBLL.GetSerie(Convert.ToInt32(tx_pedido.Text));
            if (list.Count <= 0 || list[0].Serie_Atual == 0 || list[0].Serie_Final == 0)
            {
                MessageBox.Show("Nota Fiscal criado com sucesso!\nFalha ao gerar Código de Rastreio\nVerifique se todas as informações abaixo estão preenchidas:\n\nTransportadora definida no pedido\nSérie no cadastro de despacho.", "Alerta!");
                return false;
            }
            else
            {
                foreach (var item in list)
                {
                    var serie = item.Serie_Atual + 1;
                    string NewCodRastreio = string.Empty;

                    //completa os zeros caso esteja faltando
                    string serieAtual = serie.ToString();
                    if (serieAtual.Length < 8)
                    {
                        String zeros = "";
                        int diferenca = 8 - serieAtual.Length;
                        for (int i = 0; i < diferenca; i++)
                        {
                            zeros += "0";
                        }
                        serieAtual = zeros + serieAtual;

                    }
                    string CodRastreio = item.TipoServico + serieAtual + item.SufixoCodRastreio;

                    NewCodRastreio = geraEtiquetaComDigitoVerificador(CodRastreio);


                    if (serie > item.Serie_Final)
                    {
                        MessageBox.Show("Nota Fiscal criado com sucesso!\nFalha ao gerar Código de Rastreio\nCódigo de Rastreio esgotado.", "Alerta!");
                        return false;
                    }
                    else
                    {
                        if (!_EmbalagemBLL.VerificaExisteCodRastreio(NewCodRastreio))
                        {
                            _EmbalagemBLL.Update_CodigoRastreio(Convert.ToInt32(tx_pedido.Text), NewCodRastreio);
                            _EmbalagemBLL.Update_Serie(Convert.ToInt32(tx_pedido.Text), serie);
                        }else
                        {
                            _EmbalagemBLL.Update_Serie(Convert.ToInt32(tx_pedido.Text), serie);
                            AdicionaNumeroRastreio();
                        }
                        
                    }

                }
            }
            return true;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            if (tx_peso.Text == "" && tx_quantidade.Text == "")
            {
                this.Close();
            }
            else
            {
                var res = MessageBox.Show("Deseja cancelar todos os dados seram perdidos.", "Alerta", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }

        private void btn_ExcluirLinha_Click(object sender, EventArgs e)
        {
            var embalagemBLL = new EmbalagemBLL();

            if (MessageBox.Show("Tem certeza que deseja excluir as linhas selecionadas?", "Atenção", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (DataGridViewRow selectedRow in dataGridEbalagem.SelectedRows)
                {
                    int docNum = Convert.ToInt32(selectedRow.Cells[0].Value);
                    int lineNum = Convert.ToInt32(selectedRow.Cells[1].Value);

                    embalagemBLL.DeleteLinhaEmbalagem(docNum, lineNum);
                }

                int pedido = Convert.ToInt32(tx_pedido.Text);
                CarregarGrid(pedido);
            }
        }

        public static String geraEtiquetaComDigitoVerificador(string numeroEtiqueta)
        {
            string prefixo = numeroEtiqueta.Substring(0, 2);
            string numero = numeroEtiqueta.Substring(2, 10);
            string sufixo = numeroEtiqueta.Substring(10).Trim();
            string retorno = numero;
            string dv;
            int[] multiplicadores = { 8, 6, 4, 2, 3, 5, 9, 7 };
            int soma = 0;
            // Preenche número com 0 à esquerda
            if (numeroEtiqueta.Length < 12)
            {
                retorno = "Error…";
            }
            else if (numero.Length < 8 && numeroEtiqueta.Length == 12)
            {
                String zeros = "";
                int diferenca = 8 - numero.Length;
                for (int i = 0; i < diferenca; i++)
                {
                    zeros += "0";
                }
                retorno = zeros + numero;
            }
            else
            {
                retorno = numero.Substring(0, 8);
            }
            for (int i = 0; i < 8; i++)
            {
                int dg_1 = int.Parse(retorno.Substring(i, 1));
                //int dg_2 = int.Parse(retorno.Substring(2, 1));
                //int dg_3 = int.Parse(retorno.Substring(2, 1));
                //int dg_4 = int.Parse(retorno.Substring(2, 1));
                //int dg_5 = int.Parse(retorno.Substring(2, 1));
                //int dg_6 = int.Parse(retorno.Substring(2, 1));
                //int dg_7 = int.Parse(retorno.Substring(2, 1));
                //int dg_8 = int.Parse(retorno.Substring(2, 1));


                //int vlr = int.Parse(retorno.Substring(i, (i+1)));
                int mult = multiplicadores[i];
                int vlrMult = dg_1 * mult;

                soma += vlrMult;
            }
            int resto = soma % 11;
            if (resto == 0)
            {
                dv = "5";
            }
            else if (resto == 1)
            {
                dv = "0";
            }
            else
            {
                dv = (11 - resto).ToString();
            }
            retorno += dv;
            retorno = prefixo + retorno + sufixo;
            return retorno;
        }

        private void tx_quantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
