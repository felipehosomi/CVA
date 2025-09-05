using Conferencia.BLL;
using Conferencia.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conferencia
{
    public partial class FormConferencia2 : Form
    {
        public string Login = string.Empty;
        public string DocNum = string.Empty;
        public int Escaneado_Anterior = 0;
        public int Total_Pedido = 0;
        public int Tota_Escaneado = 0;
        public int Total_Parcial = 0;


        PedidoBLL _PedidoBLL = new PedidoBLL();
        List<GriConf> listaConf1 = new List<GriConf>();
        List<PedidoModel> list = new List<PedidoModel>();
        List<PedidoModel> lista = new List<PedidoModel>();
        List<GriConf> _GriConf = new List<GriConf>();


        public string Pedido = string.Empty;


        public FormConferencia2(string Logon, string Docnum)
        {
            InitializeComponent();

            Login = Logon;
            DocNum = Docnum;
        }

        private void tx_ID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)

            {
                if (tx_ID.Text == "")
                {
                    MessageBox.Show("Necessário Informar um Número de Pedido!", "Alerta");
                }
                else
                {
                    lista = _PedidoBLL.GetCardCode(tx_ID.Text);
                    Pedido = tx_ID.Text;
                    string status = _PedidoBLL.GetStatusConf(tx_ID.Text);
                    if (lista.Count > 0)
                    {
                        foreach (var item in lista)
                        {
                            if (item.DocStatus == "O" && status == "1")
                            {
                                //tx_Cliente.Text = item.Cliente;
                                tx_produto.Focus();
                            }
                            else
                            {
                                if (item.DocStatus != "O")
                                {
                                    MessageBox.Show("Pedido não se encontra Aberto para Conferencia !", "Alerta");
                                    break;
                                }
                                else if (status != "1")
                                {
                                    MessageBox.Show("1º Conferência não realizada para este Pedido !", "Alerta");
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Numero do Pedido Inválido!", "Alerta");
                    }

                }
            }
        }

        //private void tx_produto_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
        //    {
        //        var qtde = "";
        //        var codigo = "";
        //        var _cod = "";
        //        int quantidade = 0;
        //        if (tx_produto.Text.IndexOf('*') > 1)
        //        {
        //            var mult = tx_produto.Text.Split('*');
        //            var _mult = mult[0];
        //            _cod = mult[1];
        //            var cod = mult[1].Split('-');

        //            codigo = cod[0];
        //            if (codigo.IndexOf('-') > 1)
        //            {
        //                var codi = tx_produto.Text.Split('-');

        //                codigo = codi[0];
        //                qtde = codi[1];
        //                tx_quantidade.Text = qtde;
        //            }
        //            else
        //            {
        //                codigo = _cod;
        //                qtde = "1";
        //                tx_quantidade.Text = qtde;
        //            }

        //            quantidade = Convert.ToInt32(qtde) * Convert.ToInt32(_mult);
        //            tx_quantidade.Text = quantidade.ToString();

        //        }
        //        else
        //        {
        //            if (tx_produto.Text.IndexOf('-') > 1)
        //            {
        //                var cod = tx_produto.Text.Split('-');

        //                _cod = cod[0];
        //                qtde = cod[1];
        //                tx_quantidade.Text = qtde;
        //            }
        //            else
        //            {
        //                _cod = tx_produto.Text;
        //                qtde = "1";
        //                tx_quantidade.Text = qtde;
        //            }

        //            quantidade = Convert.ToInt32(qtde);
        //            tx_quantidade.Text = quantidade.ToString();
        //        }

        //        list = _PedidoBLL.LitsConf(tx_ID.Text, _cod);

        //        //var qtde = "";
        //        //var codigo = "";

        //        if (list.Count > 0)
        //        {
        //            //    if (tx_produto.Text.IndexOf('-') > 1)
        //            //    {
        //            //        var cod = tx_produto.Text.Split('-');

        //            //        codigo = cod[0];
        //            //        qtde = cod[1];
        //            //        tx_quantidade.Text = qtde;
        //            //    }
        //            //    else
        //            //    {
        //            //        codigo = tx_produto.Text;
        //            //        qtde = "1";
        //            //        tx_quantidade.Text = qtde;
        //            //    }

        //            foreach (var item in list)
        //            {
        //                if (item.CodBarras == _cod)
        //                {
        //                    var model = new ListaProdutosConf1();

        //                    ListaProdutosConf1 prod = listaConf1.Where(i => i.CodBarras == item.CodBarras).FirstOrDefault();

        //                    if (prod == null)
        //                    {
        //                        if (Convert.ToInt32(tx_quantidade.Text) <= item.Quantidade)
        //                        {
        //                            model = new ListaProdutosConf1();

        //                            model.ItemCode = item.ItemCode;
        //                            model.ItemName = item.ItemName;
        //                            model.Quantidade = Convert.ToInt32(tx_quantidade.Text);
        //                            model.Qtde_Um = Convert.ToInt32(tx_quantidade.Text); ;
        //                            //model.CodBarras = tx_produto.Text;

        //                            if (tx_produto.Text.IndexOf('*') > 1)
        //                            {
        //                                var mult = tx_produto.Text.Split('*');
        //                                var _mult = mult[0];
        //                                _cod = mult[1];
        //                                var cod = mult[1].Split('-');

        //                                codigo = cod[0];
        //                                if (codigo.IndexOf('-') > 1)
        //                                {
        //                                    var codi = tx_produto.Text.Split('-');

        //                                    model.CodBarras = codi[0];
        //                                }
        //                                else
        //                                {
        //                                    model.CodBarras = _cod;
        //                                }

        //                            }
        //                            else
        //                            {
        //                                if (tx_produto.Text.IndexOf('-') > 1)
        //                                {
        //                                    var cod = tx_produto.Text.Split('-');

        //                                    model.CodBarras = cod[0];
        //                                }
        //                                else
        //                                {
        //                                    model.CodBarras = tx_produto.Text;

        //                                }
        //                            }

        //                            listaConf1.Add(model);
        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("Quantidade Superior a do Pedido!", "Alerta");
        //                        }

        //                    }

        //                    else
        //                    {
        //                        if (prod.Quantidade < item.Quantidade)
        //                        {
        //                            prod.Quantidade = prod.Quantidade + Convert.ToInt32(quantidade);

        //                        }
        //                        else
        //                        {
        //                            MessageBox.Show("Quantidade Superior a do Pedido!", "Alerta");
        //                        }
        //                    }

        //                }



        //                dataGridView.DataSource = null;
        //                dataGridView.DataSource = listaConf1;
        //                dataGridView.Columns[1].Width = 200;

        //                dataGridView.Refresh();
        //            }

        //            tx_quantidade.Text = "";
        //            tx_produto.Text = "";
        //        }

        //        else
        //        {
        //            MessageBox.Show("Item não encontrado no Pedido!", "Alerta");
        //        }
        //    }
        ////}

        private void tx_produto_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    var qtde = "";
                    var codigo = "";
                    var _cod = "";
                    int quantidade = 0;
                    if (tx_produto.Text.IndexOf('*') > 0)
                    {
                        var mult = tx_produto.Text.Split('*');
                        var _mult = mult[0];
                        _cod = mult[1];
                        var cod = mult[1];

                        codigo = cod;

                        if (codigo.IndexOf('-') > 0)
                        {
                            var codi = codigo.Split('-');

                            codigo = codi[0];
                            qtde = codi[1];
                            tx_quantidade.Text = qtde;
                        }
                        else
                        {
                            codigo = _cod;
                            qtde = "1";
                            tx_quantidade.Text = qtde;
                        }

                        quantidade = Convert.ToInt32(qtde) * Convert.ToInt32(_mult);
                        tx_quantidade.Text = quantidade.ToString();

                    }
                    else
                    {
                        if (tx_produto.Text.IndexOf('-') > 0)
                        {
                            var cod = tx_produto.Text.Split('-');

                            codigo = cod[0];
                            qtde = cod[1];
                            tx_quantidade.Text = qtde;
                        }
                        else
                        {
                            codigo = tx_produto.Text;
                            qtde = "1";
                            tx_quantidade.Text = qtde;
                        }

                        quantidade = Convert.ToInt32(qtde);
                        tx_quantidade.Text = quantidade.ToString();
                    }

                    list = _PedidoBLL.VerificaItemPedido(DocNum, codigo);


                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            var model = new GriConf();

                            GriConf prod = _GriConf.Where(i => i.Codbarras == item.CodBarras).FirstOrDefault();

                            if (prod == null)
                            {
                                if (Convert.ToInt32(tx_quantidade.Text) <= item.Qtd_pedido)
                                {
                                    model = new GriConf();

                                    model.ItemCode = item.ItemCode;
                                    model.ItemName = item.Dscription;
                                    model.Quantidade = item.Qtd_pedido;
                                    model.Escaneado = Convert.ToInt32(tx_quantidade.Text);

                                    if (tx_produto.Text.IndexOf('*') > 0)
                                    {
                                        var mult = tx_produto.Text.Split('*');
                                        var _mult = mult[0];
                                        _cod = mult[1];
                                        var cod = mult[1];

                                        codigo = cod;
                                        if (codigo.IndexOf('-') > 0)
                                        {
                                            var codi = codigo.Split('-');

                                            model.Codbarras = codi[0];
                                        }
                                        else
                                        {
                                            model.Codbarras = _cod;
                                        }
                                    }
                                    else
                                    {
                                        if (tx_produto.Text.IndexOf('-') > 0)
                                        {
                                            var cod = tx_produto.Text.Split('-');

                                            model.Codbarras = cod[0];
                                        }
                                        else
                                        {
                                            model.Codbarras = tx_produto.Text;

                                        }
                                    }


                                    listaConf1.Add(model);
                                    _GriConf.Add(model);

                                    lbl_CodBarras.Text = model.Codbarras;
                                    lbl_CodItem.Text = item.ItemCode;
                                    lbl_desc.Text = item.Dscription;
                                }
                                else
                                {
                                    CustomMessageBox.Show("Quantidade Superior a do Pedido!");
                                    //MessageBox.Show("Quantidade Superior a do Pedido!", "Alerta");
                                }

                            }
                            else
                            {
                                if (prod.Escaneado + quantidade > item.Qtd_pedido)
                                {
                                    CustomMessageBox.Show("Quantidade Superior a do Pedido!");
                                    //MessageBox.Show("Quantidade Superior a do Pedido!", "Alerta");
                                    tx_produto.Text = "";
                                }
                                else
                                {
                                    prod.Escaneado = prod.Escaneado + quantidade;
                                    lbl_CodBarras.Text = _cod;
                                    lbl_CodItem.Text = item.ItemCode;
                                    lbl_desc.Text = item.Dscription;
                                }
                            }

                            dataGridConf2.DataSource = null;
                            dataGridConf2.DataSource = _GriConf;
                            dataGridConf2.Columns[0].Width = 200;
                            dataGridConf2.Columns[1].Width = 740;
                            dataGridConf2.Columns[2].Width = 150;

                            int linha = 0;
                            foreach (var _item in _GriConf)
                            {
                                if (dataGridConf2.Rows[linha].Cells[4].Value.ToString() == _item.Quantidade.ToString())
                                {
                                    // Verde -> Quantidade OK
                                    Color MyRgbColor = new Color();
                                    MyRgbColor = Color.FromArgb(7, 158, 53);
                                    dataGridConf2.Rows[linha].DefaultCellStyle.BackColor = MyRgbColor;
                                }
                                else
                                {
                                    Color MyRgbColor = new Color();
                                    MyRgbColor = Color.Red;
                                    dataGridConf2.Rows[linha].DefaultCellStyle.BackColor = MyRgbColor;
                                }

                                linha ++;
                            }

                            dataGridConf2.Refresh();


                            //for (int i = 0; i < dataGridConf2.RowCount; i++)
                            //{
                            //    if (dataGridConf2.Rows[i].Cells[4].Value.ToString() == )
                            //    {
                            //        // Verde -> Quantidade OK
                            //        Color MyRgbColor = new Color();
                            //        MyRgbColor = Color.FromArgb(7, 158, 53);
                            //        dataGridConf2.Rows[i].DefaultCellStyle.BackColor = MyRgbColor;
                            //        dataGridConf2.Refresh();
                            //    }
                            //    else
                            //    {
                            //        Color MyRgbColor = new Color();
                            //        MyRgbColor = Color.Red;
                            //        dataGridConf2.Rows[i].DefaultCellStyle.BackColor = MyRgbColor;
                            //        dataGridConf2.Refresh();
                            //    }
                            //}

                            dataGridConf2.Refresh();

                            Total_Pedido = 0;
                            Tota_Escaneado = 0;

                            foreach (var row in _GriConf)
                            {
                                Total_Pedido += row.Quantidade;
                            }

                            foreach (var row in _GriConf)
                            {
                                Tota_Escaneado += row.Escaneado;
                            }

                            foreach (var row in _GriConf)
                            {
                                lbl_Escaneado.Text = "Qtde Escaneado: " + Tota_Escaneado;
                                lbl_diferenca.Text = "Diferença: " + (Tota_Escaneado - Total_Pedido);
                                break;
                            }
                        }

                        tx_quantidade.Text = "";
                        tx_produto.Text = "";
                        tx_produto.Focus();
                    }


                    else
                    {
                        CustomMessageBox.Show("Item não encontrado no Pedido!");
                        //MessageBox.Show("Item não encontrado no Pedido!", "Alerta");
                        tx_produto.Text = "";
                    }
                }

                if ((Tota_Escaneado - Total_Pedido) == 0)
                {
                    btn_add.Enabled = true;
                }
            
                
            }
            catch (Exception ex)
            {

                throw;
            }



        }

        private void tx_quantidade_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            //{
            //    list = _PedidoBLL.LitsConf(tx_ID.Text, tx_produto.Text);

            //    if (string.IsNullOrEmpty(tx_quantidade.Text))
            //    {
            //        MessageBox.Show("Necessário Informa a Quantidade!", "Alerta");
            //    }
            //    else
            //    {

            //        if (list.Count > 0)
            //        {
            //            foreach (var item in list)
            //            {
            //                if (item.ItemCode == tx_produto.Text)
            //                {
            //                    var model = new GriConf();

            //                    GriConf prod = listaConf1.Where(i => i.ItemCode == tx_produto.Text).FirstOrDefault();

            //                    if (prod == null)
            //                    {
            //                        model = new GriConf();

            //                        model.ItemCode = item.ItemCode;
            //                        model.ItemName = item.ItemName;

            //                        if (Convert.ToInt32(tx_quantidade.Text) > item.Quantidade)
            //                        {
            //                            MessageBox.Show("Quantidade Superior a do Pedido!", "Alerta");
            //                            break;
            //                        }
            //                        else
            //                        {
            //                            model.Quantidade = Convert.ToInt32(tx_quantidade.Text);
            //                        }

            //                        model.Qtde_Um = 1;

            //                        listaConf1.Add(model);

            //                        tx_quantidade.Text = "";
            //                        tx_produto.Text = "";
            //                        tx_produto.Focus();
            //                    }
            //                    else

            //                    {
            //                        if (Convert.ToInt32(tx_quantidade.Text) > item.Quantidade)
            //                        {
            //                            MessageBox.Show("Quantidade Superior a do Pedido!", "Alerta");
            //                            break;
            //                        }
            //                        else
            //                        {
            //                            var t = prod.Quantidade;

            //                            prod.Quantidade += Convert.ToInt32(tx_quantidade.Text);
            //                            if (prod.Quantidade > item.Quantidade)
            //                            {
            //                                MessageBox.Show("Quantidade Superior a do Pedido!", "Alerta");

            //                                //prod.Quantidade = 0;
            //                                prod.Quantidade = t;
            //                                break;
            //                            }
            //                            tx_quantidade.Text = "";
            //                            tx_produto.Text = "";
            //                        }
            //                    }
            //                }

            //                dataGridConf2.DataSource = null;
            //                dataGridConf2.DataSource = listaConf1;
            //                dataGridConf2.Columns[1].Width = 200;

            //                dataGridConf2.Refresh();
            //            }
            //        }

            //        else
            //        {
            //            MessageBox.Show("Item não encontrado no Pedido!", "Alerta");
            //            tx_produto.Focus();
            //        }
            //    }
            //}
        }

        private void btn_Excluir_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = this.dataGridConf2.Rows.GetFirstRow(DataGridViewElementStates.Selected);

            if (rowToDelete > -1)
            {
                var res = MessageBox.Show("Deseja mesmo excluir o item ?", "Alerta", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    for (int i = 0; i < listaConf1.Count; i++)
                    {
                        if (i == rowToDelete)
                        {
                            listaConf1.RemoveAt(i);
                        }
                    }
                    dataGridConf2.DataSource = null;
                    dataGridConf2.DataSource = listaConf1;
                    dataGridConf2.Columns[1].Width = 200;

                }
            }
            else
            {
                MessageBox.Show("Para excluir um item é preciso selecionar uma linha! ", "Alerta");
            }

        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            int j = 0;

            lista = _PedidoBLL.GetCardCode(DocNum);

            //foreach (var item in lista)
            //{
            //    ListaProdutosConf1 prod = listaConf1.Where(i => i.ItemCode == item.ItemCode).FirstOrDefault();


            //    if (prod != null)
            //    {
            //        //if ((item.TipoPag == "A" || item.TipoPag == "F") && item.ParcAntecipado == "N")
            //        //{
            //        if (item.Qtd_pedido != prod.Quantidade)
            //        {
            //            MessageBox.Show("A quantidade do Item" + " " + prod.ItemCode + " " + " não é a mesma do Pedido!", "Alerta");
            //            dataGridConf2[4, j].Style.BackColor = Color.Red;
            //            break;
            //        }
            //        else
            //        {
            //            if (prod.Quantidade < item.Qtd_pedido)
            //            {
            //                dataGridConf2[4, j].Style.BackColor = Color.Red;
            //            }
            //            else
            //            {
            //                dataGridConf2[4, j].Style.BackColor = Color.Green;
            //            }
            //        }
            //        //}
            //        //else
            //        //{
            //        //    if (prod.Quantidade < item.Qtd_pedido)
            //        //    {
            //        //        dataGridView[4, j].Style.BackColor = Color.Red;
            //        //    }
            //        //    else
            //        //    {
            //        //        dataGridView[4, j].Style.BackColor = Color.Green;
            //        //    }
            //        //}
            //    }
            //    j++;

            //}
            //dataGridConf2.Refresh();
            //Thread.Sleep(500);

            // Quntidade de Itens diferente
            //if (listaConf1.Count != lista.Count)
            //{
            //    var res = MessageBox.Show("A quantidade de Itens da 1° Conferência não é a mesma da 2° Conferência./n Deseja Continuar?", "Alerta", MessageBoxButtons.YesNo);

            //    if (res == DialogResult.Yes)
            //    {
            //        string dataConf = DateTime.Now.ToString("yyyy/MM/dd");

            //        string hora = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;

            //        int Docentry = Convert.ToInt32(tx_ID.Text);
            //        string user = tx_user.Text;

            //        // Cabeçalho
            //        _PedidoBLL.Update_2Conferencia(Docentry, dataConf, dataConf, "1");

            //        //Linhas
            //        int i = 0;
            //        foreach (var item in listaConf1)
            //        {
            //            _PedidoBLL.Update_2ConferenciaLinha(Docentry, i, item.ItemCode, item.Quantidade, user, dataConf, hora);
            //            i++;
            //        }

            //        foreach (var item in lista)
            //        {
            //            if (item.TipoPag == "F" && item.ParcFaturado == "Y")
            //            {
            //                MessageBox.Show("Conferencia Efetuada com Sucesso!");
            //                break;
            //            }
            //            else
            //            {
            //                MessageBox.Show("Conferencia Salva com Restrições no faturamento!");
            //                break;
            //            }
            //        }
            //        //LimpaTela();
            //        //tx_user.Focus();

            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            string dataConf = DateTime.Now.ToString("yyyy/MM/dd");

            string hora = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;

            int Docentry = Convert.ToInt32(DocNum);
            var usuario = tx_user.Text.Split(':');
            var user = usuario[1];

            // Cabeçalho
            _PedidoBLL.Update_2Conferencia(Docentry, dataConf, dataConf, "1");

            //Linhas
            int i = 0;
            foreach (var item in _GriConf)
            {
                _PedidoBLL.Update_2ConferenciaLinha(Docentry, i, item.ItemCode, item.Quantidade, user, dataConf, hora);
                i++;
            }

            MessageBox.Show("Conferencia Efetuada com Sucesso!");
            //_PedidoBLL.SetFechamentoPicking2Conf(Convert.ToInt32(DocNum), DateTime.Now.ToString());

            //foreach (var item in lista)
            //{
            //    if (item.TipoPag == "F" && item.ParcFaturado == "Y")
            //    {
            //        MessageBox.Show("Conferencia Efetuada com Sucesso!");
            //        _PedidoBLL.SetFechamentoPicking2Conf(Convert.ToInt32(DocNum), DateTime.Now.ToString());
            //        btn_vol.Enabled = true;
            //        tx_produto.Focus();
            //        break;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Conferencia Salva com Restrições no faturamento!");
            //        _PedidoBLL.SetFechamentoPicking2Conf(Convert.ToInt32(DocNum), DateTime.Now.ToString());
            //        break;
            //    }
            //}

            DeleteParcial(Convert.ToInt32(DocNum));

            //Pedido = tx_ID.Text;
            DefinicaoVolumes form = new DefinicaoVolumes(DocNum);
            form.ShowDialog();
            this.Hide();
            //LimpaTela();
            //tx_user.Focus();
            //}
        }

        private void LimpaTela()
        {
            dataGridConf2.DataSource = null;

            lbl_Escaneado.Text = "Qtde Escaneado: 0";
            lbl_diferenca.Text = "Diferença : 0";
            lbl_QtdTotal.Text = "Qtde Total: 0";

            lbl_Cliente.Text = "";
            lbl_CodBarras.Text = "";
            lbl_CodItem.Text = "";
            lbl_DataEntrega.Text = "";
            lbl_DataPedido.Text = "";
            lbl_desc.Text = "";
            lbl_Origem.Text = "";
            lbl_Pedido.Text = "";
            lbl_TipoEntrega.Text = "";
        }

        private void btn_vol_Click(object sender, EventArgs e)
        {
            Pedido = tx_ID.Text;
            DefinicaoVolumes form = new DefinicaoVolumes(Pedido);
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("Deseja cancelar todos os dados seram perdidos.", "Alerta", MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)
            {
                this.Hide();

                var frm = new FrmListaConf2(Login);
                frm.ShowDialog();
            }
        }

        private void FormConferencia2_Load(object sender, EventArgs e)
        {
            int linha = 0;
            tx_user.Text = "Usuário: " + Login.ToUpper();

            var listaCabecalho = _PedidoBLL.CarregCabecalhoPedido(DocNum);

            foreach (var item in listaCabecalho)
            {
                lbl_Cliente.Text = item.Cliente;
                lbl_DataEntrega.Text = item.Data_Entrega.ToShortDateString();
                lbl_DataPedido.Text = item.Data_Pedido.ToShortDateString();
                lbl_Origem.Text = item.Origem;
                lbl_TipoEntrega.Text = item.Transportadora;
                lbl_Pedido.Text = item.N_Pedido.ToString();
            }

            // Verifica Finalizado Parcial
            if (VerificaParcial(DocNum))
            {
                _GriConf = _PedidoBLL.CarregaParcial2(DocNum);

                dataGridConf2.DataSource = null;
                dataGridConf2.DataSource = _GriConf;
                dataGridConf2.Columns[0].Width = 200;
                dataGridConf2.Columns[1].Width = 740;
                dataGridConf2.Columns[2].Width = 150;
                dataGridConf2.Refresh();

                Total_Pedido = 0;
                Total_Parcial = 0;
                int i = 0;
                foreach (var item in _GriConf)
                {
                    Total_Pedido += item.Quantidade;
                    i++;
                }

                foreach (var item in _GriConf)
                {
                    Total_Parcial += item.Escaneado;
                    i++;
                }

                foreach (var item in _GriConf)
                {
                    lbl_Escaneado.Text = "Qtde Escaneado: " + Total_Parcial;
                    lbl_QtdTotal.Text = "Qtde Total: " + Total_Pedido;
                    lbl_diferenca.Text = "Diferença: " + (Total_Parcial - Total_Pedido);
                    break;
                }

               
                foreach (var _item in _GriConf)
                {
                    if (dataGridConf2.Rows[linha].Cells[4].Value.ToString() == _item.Quantidade.ToString())
                    {
                        // Verde -> Quantidade OK
                        Color MyRgbColor = new Color();
                        MyRgbColor = Color.FromArgb(7, 158, 53);
                        dataGridConf2.Rows[linha].DefaultCellStyle.BackColor = MyRgbColor;
                    }
                    else
                    {
                        Color MyRgbColor = new Color();
                        MyRgbColor = Color.Red;
                        dataGridConf2.Rows[linha].DefaultCellStyle.BackColor = MyRgbColor;
                    }

                    linha++;
                }

                dataGridConf2.Refresh();

                if ((Total_Parcial - Total_Pedido) == 0)
                {
                    btn_add.Enabled = true;
                }
                else
                {
                    btn_add.Enabled = false;
                }

            }
            else
            {
                _GriConf = _PedidoBLL.CarregaGridConf1(DocNum);

                dataGridConf2.DataSource = null;
                dataGridConf2.DataSource = _GriConf;
                dataGridConf2.Columns[0].Width = 200;
                dataGridConf2.Columns[1].Width = 740;
                dataGridConf2.Columns[2].Width = 150;
                dataGridConf2.Refresh();

                Total_Pedido = 0;
                int i = 0;
                foreach (var item in _GriConf)
                {
                    Total_Pedido += item.Quantidade;
                    Tota_Escaneado += item.Escaneado;
                    i++;
                }

                foreach (var item in _GriConf)
                {
                    lbl_Escaneado.Text = "Qtde Escaneado: " + Tota_Escaneado;
                    lbl_QtdTotal.Text = "Qtde Total: " + Total_Pedido;
                    lbl_diferenca.Text = "Diferença: " + (Tota_Escaneado - Total_Pedido);
                }

                if ((Tota_Escaneado - Total_Pedido) == 0)
                {
                    btn_add.Enabled = true;
                }
                else
                {
                    btn_add.Enabled = false;
                }

                linha = 0;
                foreach (var _item in _GriConf)
                {
                    if (dataGridConf2.Rows[linha].Cells[4].Value.ToString() == _item.Quantidade.ToString())
                    {
                        // Verde -> Quantidade OK
                        Color MyRgbColor = new Color();
                        MyRgbColor = Color.FromArgb(7, 158, 53);
                        dataGridConf2.Rows[linha].DefaultCellStyle.BackColor = MyRgbColor;
                    }
                    else
                    {
                        Color MyRgbColor = new Color();
                        MyRgbColor = Color.Red;
                        dataGridConf2.Rows[linha].DefaultCellStyle.BackColor = MyRgbColor;
                    }

                    linha++;
                }

                dataGridConf2.Refresh();
            }

           
        }

        private bool VerificaParcial(string DocNum)
        {
            var i = _PedidoBLL.VerificaParcial(DocNum);

            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DeleteParcial(int Docnum)
        {
            try
            {
                _PedidoBLL.DeleteParcial(Docnum);
                _PedidoBLL.DeleteParcialItens(Docnum);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                var res = MessageBox.Show("Tem certeza que Deseja Finalizar Parcial ?", "Alerta", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {

                    string hora = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                    var usuario = tx_user.Text.Split(':');
                    var user = usuario[1];


                    //// Cabeçalho
                    //_PedidoBLL.Insert_1ConferenciaParcial(Convert.ToInt32(DocNum), DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"), DateTime.MinValue.ToString("yyyy/MM/dd"), DateTime.MinValue.ToString("yyyy/MM/dd"), "1", null, "1");


                    //List<GriConf> _listaPacial = new List<GriConf>();


                    //for (int i = 0; i < dataGridConf2.Rows.Count; i++)
                    //{
                    //    var model = new GriConf();

                    //    model.ItemCode = dataGridConf2.Rows[i].Cells[0].Value.ToString();
                    //    model.ItemName = dataGridConf2.Rows[i].Cells[1].Value.ToString();
                    //    model.Codbarras = dataGridConf2.Rows[i].Cells[2].Value.ToString();
                    //    model.Quantidade = Convert.ToInt32(dataGridConf2.Rows[i].Cells[3].Value.ToString());
                    //    model.Escaneado = Convert.ToInt32(dataGridConf2.Rows[i].Cells[4].Value.ToString());

                    //    _listaPacial.Add(model);

                    //}

                    //int o = 0;
                    //foreach (var item in _listaPacial)
                    //{
                    //    _PedidoBLL.Insert_1ConferenciaLinhaParcial(Convert.ToInt32(DocNum), item.Codbarras, o, item.ItemCode, item.ItemName, item.Quantidade, item.Escaneado, 0, user, null, DateTime.Now.ToString("yyyy/MM/dd"), hora, null, null);
                    //    o++;
                    //}
                    var parcial = VerificaParcial(DocNum);
                    if (!parcial)
                    {
                        // Cabeçalho
                        _PedidoBLL.Insert_2ConferenciaParcial(Convert.ToInt32(DocNum), DateTime.MinValue.ToString("yyyy/MM/dd"), DateTime.MinValue.ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"), null, "1", "1");


                        List<GriConf> _listaPacial = new List<GriConf>();


                        for (int i = 0; i < dataGridConf2.Rows.Count; i++)
                        {
                            var model = new GriConf();

                            model.ItemCode = dataGridConf2.Rows[i].Cells[0].Value.ToString();
                            model.ItemName = dataGridConf2.Rows[i].Cells[1].Value.ToString();
                            model.Codbarras = dataGridConf2.Rows[i].Cells[2].Value.ToString();
                            model.Quantidade = Convert.ToInt32(dataGridConf2.Rows[i].Cells[3].Value.ToString());
                            model.Escaneado = Convert.ToInt32(dataGridConf2.Rows[i].Cells[4].Value.ToString());

                            _listaPacial.Add(model);

                        }

                        int o = 0;
                        foreach (var item in _listaPacial)
                        {
                            _PedidoBLL.Insert_2ConferenciaLinhaParcial(Convert.ToInt32(DocNum), item.Codbarras, o, item.ItemCode, item.ItemName, item.Quantidade, 0, item.Escaneado, null, user, null, null, DateTime.Now.ToString("yyyy/MM/dd"), hora);
                            o++;
                        }

                    }
                    else
                    {
                        _PedidoBLL.Update_Parcial2(Convert.ToInt32(DocNum), DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"), "1");


                        List<GriConf> _listaPacial = new List<GriConf>();


                        for (int i = 0; i < dataGridConf2.Rows.Count; i++)
                        {
                            var model = new GriConf();

                            model.ItemCode = dataGridConf2.Rows[i].Cells[0].Value.ToString();
                            model.ItemName = dataGridConf2.Rows[i].Cells[1].Value.ToString();
                            model.Codbarras = dataGridConf2.Rows[i].Cells[2].Value.ToString();
                            model.Quantidade = Convert.ToInt32(dataGridConf2.Rows[i].Cells[3].Value.ToString());
                            model.Escaneado = Convert.ToInt32(dataGridConf2.Rows[i].Cells[4].Value.ToString());

                            _listaPacial.Add(model);

                        }

                        int o = 0;
                        foreach (var item in _listaPacial)
                        {
                            _PedidoBLL.Update_2PAarcialLinha(Convert.ToInt32(DocNum), o, item.ItemCode, item.Escaneado, user, DateTime.Now.ToString("yyyy/MM/dd"), hora);
                            o++;
                        }
                    }

                    MessageBox.Show("Conferência Parcial Salva com Sucesso!!!", "Alerta");

                    LimpaTela();
                    tx_produto.Focus();

                    this.Hide();
                    var frm = new FrmListaConf2(Login);
                    frm.ShowDialog();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Erro ao Salvar Conferência");
            }
        }
    }
}
