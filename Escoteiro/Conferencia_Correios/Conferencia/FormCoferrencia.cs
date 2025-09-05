using Conferencia.BLL;
using Conferencia.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conferencia
{
    public partial class Form1 : Form
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

        public Form1(string Logon, string Docnum)
        {
            InitializeComponent();
            Login = Logon;
            DocNum = Docnum;

        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {

            var res = MessageBox.Show("Deseja cancelar todos os dados seram perdidos.", "Alerta", MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)
            {
                this.Hide();

                var frm = new FrmListaConf1(Login);
                frm.ShowDialog();
            }
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
                    if (lista.Count > 0)
                    {
                        foreach (var item in lista)
                        {
                            if (item.DocStatus != "O")
                            {
                                MessageBox.Show("Pedido não se encontra Aberto para Conferencia !", "Alerta");
                            }
                            else
                            {
                                tx_Cliente.Text = item.Cliente;
                                tx_produto.Focus();
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

        private void tx_produto_KeyDown(object sender, KeyEventArgs e)
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
                    var cod = mult[1].Split('-');

                    codigo = cod[0];
                    if (codigo.IndexOf('-') > 1)
                    {
                        var codi = tx_produto.Text.Split('-');

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
                    if (tx_produto.Text.IndexOf('-') > 1)
                    {
                        var cod = tx_produto.Text.Split('-');

                        _cod = cod[0];
                        qtde = cod[1];
                        tx_quantidade.Text = qtde;
                    }
                    else
                    {
                        _cod = tx_produto.Text;
                        qtde = "1";
                        tx_quantidade.Text = qtde;
                    }

                    quantidade = Convert.ToInt32(qtde);
                    tx_quantidade.Text = quantidade.ToString();
                }



                list = _PedidoBLL.VerificaItemPedido(DocNum, _cod);


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

                                if (tx_produto.Text.IndexOf('*') > 1)
                                {
                                    var mult = tx_produto.Text.Split('*');
                                    var _mult = mult[0];
                                    _cod = mult[1];
                                    var cod = mult[1].Split('-');

                                    codigo = cod[0];
                                    if (codigo.IndexOf('-') > 1)
                                    {
                                        var codi = tx_produto.Text.Split('-');

                                        model.Codbarras = codi[0];
                                    }
                                    else
                                    {
                                        model.Codbarras = _cod;
                                    }

                                }
                                else
                                {
                                    if (tx_produto.Text.IndexOf('-') > 1)
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

                                //lbl_Escaneado.Text = "Qtde Escaneado: " + model.Escaneado;
                                ////lbl_QtdTotal.Text = "Qtde Total: " + item.Qtd_pedido;

                                //Escaneado_Anterior = model.Escaneado;
                                //lbl_diferenca.Text = "Diferença: " + (model.Escaneado - Total_Pedido);


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
                            }
                            else
                            {
                                prod.Escaneado = prod.Escaneado + Convert.ToInt32(quantidade);
                                lbl_CodBarras.Text = _cod;
                                lbl_CodItem.Text = item.ItemCode;
                                lbl_desc.Text = item.Dscription;

                                //lbl_Escaneado.Text = "Qtde Escaneado: " + prod.Escaneado;
                                ////lbl_QtdTotal.Text = "Qtde Total: " + item.Qtd_pedido;
                                //Escaneado_Anterior = model.Escaneado;
                                //lbl_diferenca.Text = "Diferença: " + (prod.Escaneado - Escaneado_Anterior);
                            }
                        }

                        dataGridConf1.DataSource = null;
                        dataGridConf1.DataSource = _GriConf;
                        dataGridConf1.Columns[0].Width = 200;
                        dataGridConf1.Columns[1].Width = 740;
                        dataGridConf1.Columns[2].Width = 150;

                        int linha = 0;
                        foreach (var _item in _GriConf)
                        {
                            if (dataGridConf1.Rows[linha].Cells[4].Value.ToString() == _item.Quantidade.ToString())
                            {
                                // Verde -> Quantidade OK
                                Color MyRgbColor = new Color();
                                MyRgbColor = Color.FromArgb(7, 158, 53);
                                dataGridConf1.Rows[linha].DefaultCellStyle.BackColor = MyRgbColor;
                            }
                            else
                            {
                                Color MyRgbColor = new Color();
                                MyRgbColor = Color.Red;
                                dataGridConf1.Rows[linha].DefaultCellStyle.BackColor = MyRgbColor;
                            }

                            linha++;
                        }

                        dataGridConf1.Refresh();

                        Total_Pedido = 0;
                        Tota_Escaneado = 0;

                        foreach (var row in _GriConf)
                        {
                            Total_Pedido += row.Quantidade;
                        }

                        foreach (var row in _GriConf)
                        {
                            Tota_Escaneado += row.Escaneado;
                            //lbl_Escaneado.Text = "Qtde Escaneado: " + row.Escaneado;
                            ////lbl_QtdTotal.Text = "Qtde Total: " + Total_Pedido;
                            //lbl_diferenca.Text = "Diferença: " + (row.Escaneado - Total_Pedido);
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
                if ((Tota_Escaneado - Total_Pedido) == 0)
                {
                    btn_add.Enabled = true;
                }

            }
        }

        private void btn_Excluir_Click(object sender, EventArgs e)
        {
            Int32 rowToDelete = this.dataGridConf1.Rows.GetFirstRow(DataGridViewElementStates.Selected);

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
                    dataGridConf1.DataSource = null;
                    dataGridConf1.DataSource = listaConf1;
                    dataGridConf1.Columns[1].Width = 200;

                }
            }
            else
            {
                MessageBox.Show("Para excluir um item é preciso selecionar uma linha! ", "Alerta");
            }


        }

        private void LimpaTela()
        {
            dataGridConf1.DataSource = null;

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

        private void tx_quantidade_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                list = _PedidoBLL.VerificaItemPedido(tx_ID.Text, tx_produto.Text);

                if (string.IsNullOrEmpty(tx_quantidade.Text))
                {
                    MessageBox.Show("Necessário Informa a Quantidade!", "Alerta");
                }
                else
                {
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {

                            if (item.ItemCode == tx_produto.Text)
                            {
                                var model = new GriConf();

                                GriConf prod = listaConf1.Where(i => i.ItemCode == tx_produto.Text).FirstOrDefault();

                                if (prod == null)
                                {
                                    model = new GriConf();

                                    model.ItemCode = item.ItemCode;
                                    model.ItemName = item.Dscription;
                                    if (Convert.ToInt32(tx_quantidade.Text) > item.Qtd_pedido)
                                    {
                                        MessageBox.Show("Quantidade Superior a do Pedido!", "Alerta");
                                        break;
                                    }
                                    else
                                    {
                                        model.Escaneado = Convert.ToInt32(tx_quantidade.Text);
                                    }

                                    model.Quantidade = item.Qtd_pedido;

                                    listaConf1.Add(model);

                                    tx_quantidade.Text = "";
                                    tx_produto.Text = "";
                                }
                                else
                                {

                                    if (prod.Quantidade < item.Qtd_pedido)
                                    {
                                        prod.Quantidade = prod.Quantidade + 1;

                                        tx_quantidade.Text = "";
                                        tx_produto.Text = "";
                                    }
                                    else
                                    {
                                        MessageBox.Show("Quantidade Superior a do Pedido!", "Alerta");
                                        break;
                                    }
                                }

                            }
                            dataGridConf1.DataSource = null;
                            dataGridConf1.DataSource = listaConf1;
                            dataGridConf1.Columns[1].Width = 200;

                            dataGridConf1.Refresh();
                        }
                    }

                    else
                    {
                        CustomMessageBox.Show("Item não encontrado no Pedido!");
                        //MessageBox.Show("Item não encontrado no Pedido!", "Alerta");
                    }
                }
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                int j = 0;

                lista = _PedidoBLL.GetCardCode(DocNum);

                if (_GriConf.Count != lista.Count)
                {
                    MessageBox.Show("A quantidade de Itens do Pedido não é mesma Conferida. Verifique!");
                }
                else
                {
                    string dataConf = DateTime.Now.ToString("yyyy/MM/dd");

                    string hora = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;

                    int Docentry = Convert.ToInt32(DocNum);
                    var usuario = tx_user.Text.Split(':');
                    string user = usuario[1];


                    // Cabeçalho
                    _PedidoBLL.Insert_1Conferencia(Docentry, dataConf, dataConf, DateTime.MinValue.ToString("yyyy/MM/dd"), DateTime.MinValue.ToString("yyyy/MM/dd"), "1", null, "1");

                    //Linhas
                    int i = 0;
                    foreach (var item in _GriConf)
                    {
                        _PedidoBLL.Insert_1ConferenciaLinha(Docentry, item.Codbarras, i, item.ItemCode, item.ItemName, item.Escaneado, 0, user, null, dataConf, hora, null, null);
                        i++;
                    }

                    foreach (var item in lista)
                    {
                        MessageBox.Show("Conferencia Efetuada com Sucesso!");
                        _PedidoBLL.SetFechamentoPicking1Conf(Convert.ToInt32(DocNum), DateTime.Now.ToString());
                        break;

                        //if (item.ParcFaturado == "Y")
                        //{
                        //    MessageBox.Show("Conferencia Efetuada com Sucesso!");
                        //    _PedidoBLL.SetFechamentoPicking1Conf(Convert.ToInt32(DocNum), DateTime.Now.ToString());
                        //    break;
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Conferencia Salva com Restrições no faturamento!");
                        //    _PedidoBLL.SetFechamentoPicking1Conf(Convert.ToInt32(DocNum), DateTime.Now.ToString());
                        //    break;
                        //}
                    }

                    DeleteParcial(Convert.ToInt32(DocNum));

                    LimpaTela();
                    tx_user.Focus();


                    //var res = MessageBox.Show("Ir para 2º Confrência?", "Alerta", MessageBoxButtons.YesNo);

                    //if (res == DialogResult.Yes)
                    //{
                    //    this.Hide();

                    //   var frm = new FrmListaConf2(Login);
                    //    frm.ShowDialog();
                    //}
                    //else

                    {

                        this.Hide();
                        var frm = new FrmListaConf1(Login);
                        frm.ShowDialog();
                    }

                     
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
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
                _GriConf = _PedidoBLL.CarregaParcial(DocNum);

                dataGridConf1.DataSource = null;
                dataGridConf1.DataSource = _GriConf;
                dataGridConf1.Columns[0].Width = 200;
                dataGridConf1.Columns[1].Width = 740;
                dataGridConf1.Columns[2].Width = 150;
                dataGridConf1.Refresh();

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

                if ((Total_Parcial - Total_Pedido) == 0)
                {
                    btn_add.Enabled = true;
                }
                else
                {
                    btn_add.Enabled = false;
                }

                foreach (var _item in _GriConf)
                {
                    if (dataGridConf1.Rows[linha].Cells[4].Value.ToString() == _item.Quantidade.ToString())
                    {
                        // Verde -> Quantidade OK
                        Color MyRgbColor = new Color();
                        MyRgbColor = Color.FromArgb(7, 158, 53);
                        dataGridConf1.Rows[linha].DefaultCellStyle.BackColor = MyRgbColor;
                    }
                    else
                    {
                        Color MyRgbColor = new Color();
                        MyRgbColor = Color.Red;
                        dataGridConf1.Rows[linha].DefaultCellStyle.BackColor = MyRgbColor;
                    }

                    linha++;
                }

                dataGridConf1.Refresh();

            }
            else
            {
                _GriConf = _PedidoBLL.CarregaGridConf1(DocNum);

                dataGridConf1.DataSource = null;
                dataGridConf1.DataSource = _GriConf;
                dataGridConf1.Columns[0].Width = 200;
                dataGridConf1.Columns[1].Width = 740;
                dataGridConf1.Columns[2].Width = 150;
                dataGridConf1.Refresh();

                Total_Pedido = 0;
                int i = 0;
                foreach (var item in _GriConf)
                {
                    Total_Pedido += item.Quantidade;
                    i++;
                }

                foreach (var item in _GriConf)
                {
                    lbl_Escaneado.Text = "Qtde Escaneado: " + item.Escaneado;
                    lbl_QtdTotal.Text = "Qtde Total: " + Total_Pedido;
                    lbl_diferenca.Text = "Diferença: " + (item.Escaneado - Total_Pedido);
                }

            }


            if ((Total_Parcial - Total_Pedido) == 0)
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
                if (dataGridConf1.Rows[linha].Cells[4].Value.ToString() == _item.Quantidade.ToString())
                {
                    // Verde -> Quantidade OK
                    Color MyRgbColor = new Color();
                    MyRgbColor = Color.FromArgb(7, 158, 53);
                    dataGridConf1.Rows[linha].DefaultCellStyle.BackColor = MyRgbColor;
                }
                else
                {
                    Color MyRgbColor = new Color();
                    MyRgbColor = Color.Red;
                    dataGridConf1.Rows[linha].DefaultCellStyle.BackColor = MyRgbColor;
                }

                linha++;
            }

            dataGridConf1.Refresh();

        }

        private void btn_FinalizaParcial_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {

                var res = MessageBox.Show("Tem certeza que Deseja Finalizar Parcial ?", "Alerta", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {

                    string hora = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                    var usuario = tx_user.Text.Split(':');
                    var user = usuario[1];

                    var parcial = VerificaParcial(DocNum);
                    if (!parcial)
                    {
                        // Cabeçalho
                        _PedidoBLL.Insert_1ConferenciaParcial(Convert.ToInt32(DocNum), DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"), DateTime.MinValue.ToString("yyyy/MM/dd"), DateTime.MinValue.ToString("yyyy/MM/dd"), "1", null, "1");


                        List<GriConf> _listaPacial = new List<GriConf>();


                        for (int i = 0; i < dataGridConf1.Rows.Count; i++)
                        {
                            var model = new GriConf();

                            model.ItemCode = dataGridConf1.Rows[i].Cells[0].Value.ToString();
                            model.ItemName = dataGridConf1.Rows[i].Cells[1].Value.ToString();
                            model.Codbarras = dataGridConf1.Rows[i].Cells[2].Value.ToString();
                            model.Quantidade = Convert.ToInt32(dataGridConf1.Rows[i].Cells[3].Value.ToString());
                            model.Escaneado = Convert.ToInt32(dataGridConf1.Rows[i].Cells[4].Value.ToString());

                            _listaPacial.Add(model);

                        }

                        int o = 0;
                        foreach (var item in _listaPacial)
                        {
                            _PedidoBLL.Insert_1ConferenciaLinhaParcial(Convert.ToInt32(DocNum), item.Codbarras, o, item.ItemCode, item.ItemName, item.Quantidade, item.Escaneado, 0, user, null, DateTime.Now.ToString("yyyy/MM/dd"), hora, null, null);
                            o++;
                        }

                    }
                    else
                    {
                        _PedidoBLL.Update_Parcial(Convert.ToInt32(DocNum), DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"), "1");


                        List<GriConf> _listaPacial = new List<GriConf>();


                        for (int i = 0; i < dataGridConf1.Rows.Count; i++)
                        {
                            var model = new GriConf();

                            model.ItemCode = dataGridConf1.Rows[i].Cells[0].Value.ToString();
                            model.ItemName = dataGridConf1.Rows[i].Cells[1].Value.ToString();
                            model.Codbarras = dataGridConf1.Rows[i].Cells[2].Value.ToString();
                            model.Quantidade = Convert.ToInt32(dataGridConf1.Rows[i].Cells[3].Value.ToString());
                            model.Escaneado = Convert.ToInt32(dataGridConf1.Rows[i].Cells[4].Value.ToString());

                            _listaPacial.Add(model);

                        }

                        int o = 0;
                        foreach (var item in _listaPacial)
                        {
                            _PedidoBLL.Update_PAarcialLinha(Convert.ToInt32(DocNum), o, item.ItemCode, item.Escaneado, user, DateTime.Now.ToString("yyyy/MM/dd"), hora);
                            o++;
                        }
                    }
                    // Cabeçalho
                    

                    MessageBox.Show("Conferência Parcial Salva com Sucesso!!!", "Alerta");                 

                    LimpaTela();
                    tx_produto.Focus();

                    this.Hide();
                    var frm = new FrmListaConf1(Login);
                    frm.ShowDialog();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Erro ao Salvar Conferência");
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

    }

}