using Conferencia.Arquivo;
using Conferencia.BLL;
using Conferencia.Model;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections;
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
    public partial class FrmGesaoPicking : Form
    {
        List<GestaoPickingModel> listaPicking = new List<GestaoPickingModel>();
        GestaoPickingBLL _GestaoPickingBLL = new GestaoPickingBLL();
        PedidoBLL _PedidoBLL = new PedidoBLL();
        public string Login = string.Empty;
        private FolderBrowserDialog folderDialog;
        private FileDialog openDialog;
        private PrintDialog printDialog;
        private DialogResult resultDialog;
        private ConnectionInfo ReportConnectionInfo;
        public string CrystalDirectory;

        public FrmGesaoPicking(string Logon)
        {
            InitializeComponent();
            Login = Logon;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void FrmGesaoPicking_Load(object sender, EventArgs e)
        {
            tx_User.Text = "Usuário: " + Login;

            CarregaComboBoxFilial();
            CarregaComboBoxTransportadora();
            CarregaComboBox_Status();

            tx_NumeroPedido.Focus();
        }

        private void CarregaComboBoxFilial()
        {
            cb_Filial.DataSource = _PedidoBLL.CarregaComboFilial();
            cb_Filial.DisplayMember = "Descricao";
            cb_Filial.ValueMember = "Code";
        }

        private void CarregaComboBoxTransportadora()
        {
            cb_transportadora.DataSource = _GestaoPickingBLL.CarregaComboTipoEnvio();
            cb_transportadora.DisplayMember = "Descricao";
            cb_transportadora.ValueMember = "Code";
        }

        private void CarregaComboBox_Status()
        {
            cb__Status.DataSource = _GestaoPickingBLL.CarregaCombo_Status();
            cb__Status.DisplayMember = "Descricao";
            cb__Status.ValueMember = "Code";
        }

        private void btn_pesquisar_MouseClick(object sender, MouseEventArgs e)
        {
            AtualizaPesquisa();

        }

        private void AtualizaPesquisa()
        {
            DataGridViewCheckBoxColumn ck = new DataGridViewCheckBoxColumn();

            if (GridPicking.Columns.Count > 0)
            {
                GridPicking.DataSource = null;
                GridPicking.Columns.Remove(GridPicking.Columns[0]);
                GridPicking.Refresh();
            }

            int N_Pedido = 0;
            string DataDe = Convert.ToDateTime(tx_dataDe.Text).ToString("yyyyMMdd");
            string DataAte = Convert.ToDateTime(tx_dataAte.Text).ToString("yyyyMMdd");

            int Status = cb__Status.SelectedIndex;
            int Filial = Convert.ToInt32(cb_Filial.SelectedValue);
            if (!string.IsNullOrEmpty(tx_NumeroPedido.Text))
            {
                N_Pedido = Convert.ToInt32(tx_NumeroPedido.Text);
                DataDe = "";
                DataAte = "";
            }


            int TipoEnvio = Convert.ToInt32(cb_transportadora.SelectedValue);

            listaPicking = _GestaoPickingBLL.ListaPicking(DataDe, DataAte, Filial, Status, TipoEnvio, N_Pedido);
            listaPicking = listaPicking.OrderBy(o => o.N_Pedido).ToList();

            ck.HeaderText = "#";
            GridPicking.Columns.Add(ck);

            GridPicking.DataSource = null;
            GridPicking.DataSource = listaPicking;

            GridPicking.Columns[0].Width = 20;
            GridPicking.Columns[1].Width = 60;
            GridPicking.Columns[2].Width = 90;
            GridPicking.Columns[3].Width = 130;
            GridPicking.Columns[4].Width = 120;
            GridPicking.Columns[5].Width = 280;
            GridPicking.Columns[6].Width = 280;
            GridPicking.Columns[7].Width = 120;
            GridPicking.Columns[8].Width = 120;
            GridPicking.Columns[9].Width = 120;
            GridPicking.Columns[10].Width = 120;
            GridPicking.Columns[11].Width = 120;
            GridPicking.Columns[12].Width = 120;


            GridPicking.Columns[1].ReadOnly = true;
            GridPicking.Columns[2].ReadOnly = true;
            GridPicking.Columns[3].ReadOnly = true;
            GridPicking.Columns[4].ReadOnly = true;
            GridPicking.Columns[5].ReadOnly = true;
            GridPicking.Columns[6].ReadOnly = true;
            GridPicking.Columns[7].ReadOnly = true;
            GridPicking.Columns[8].ReadOnly = true;
            GridPicking.Columns[9].ReadOnly = true;
            GridPicking.Columns[10].ReadOnly = true;
            GridPicking.Columns[11].ReadOnly = true;
            GridPicking.Columns[12].ReadOnly = true;



            foreach (DataGridViewRow row in GridPicking.Rows)
            {
                if (row.Cells[1].Value.ToString() == "Impresso")
                {
                    // Azul -> Impresso
                    Color MyRgbColor = new Color();
                    MyRgbColor = Color.Blue;
                    row.Cells[1].Style.BackColor = MyRgbColor;
                    MyRgbColor = Color.White;
                    row.Cells[1].Style.ForeColor = MyRgbColor;

                }
            }
            GridPicking.Refresh();
        }

        private void btn_Check_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < GridPicking.Rows.Count; i++)
            {
                GridPicking.Rows[i].Cells[0].Value = true;
            }
        }

        private void btn_UnCheck_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < GridPicking.Rows.Count; i++)
            {
                GridPicking.Rows[i].Cells[0].Value = false;
            }
        }

        private void btn_Remove_MouseClick(object sender, MouseEventArgs e)
        {

            List<string> listaPicking = new List<string>();

            if (GridPicking.Rows.Count > 0)
            {
                if (MessageBox.Show("Tem certeza que deseja excluir as linhas selecionadas?", "Atenção", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < GridPicking.Rows.Count; i++)
                    {
                        var Insertar = GridPicking.Rows[i].Cells[0].Value == null ? "False" : GridPicking.Rows[i].Cells[0].Value.ToString();

                        if (Insertar == "True")
                        {
                            listaPicking.Add(GridPicking.Rows[i].Cells[2].Value.ToString());
                        }
                    }

                    if (listaPicking.Count > 0)
                    {
                        foreach (var item in listaPicking)
                        {
                            _PedidoBLL.AtualizaGestaoPickingPedido(item.ToString());
                        }

                        AtualizaPesquisa();

                    }
                    else
                    {
                        MessageBox.Show("Para Remover é necessário Selecionar o Pedido antes! ", "Alerta");
                    }


                }
                else
                {
                    MessageBox.Show("Para excluir um item é preciso selecionar uma linha! ", "Alerta");

                }
            }


            //Int32 rowToDelete = this.GridPicking.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            //if (rowToDelete > -1)
            //{

            //    var res = MessageBox.Show("Deseja mesmo excluir o item ?", "Alerta", MessageBoxButtons.YesNo);

            //    if (res == DialogResult.Yes)
            //    {
            //        for (int i = 0; i < listaPicking.Count; i++)
            //        {
            //            if (i == rowToDelete)
            //            {
            //                listaPicking.RemoveAt(i);
            //            }

            //        }
            //        GridPicking.DataSource = null;
            //        GridPicking.DataSource = listaPicking;

            //        GridPicking.Columns[0].Width = 20;
            //        GridPicking.Columns[1].Width = 90;
            //        GridPicking.Columns[2].Width = 120;
            //        GridPicking.Columns[3].Width = 120;
            //        GridPicking.Columns[4].Width = 120;
            //        GridPicking.Columns[5].Width = 280;
            //        GridPicking.Columns[6].Width = 280;
            //        GridPicking.Columns[7].Width = 120;
            //        GridPicking.Columns[8].Width = 120;
            //        GridPicking.Columns[9].Width = 120;
            //        GridPicking.Columns[10].Width = 120;
            //        GridPicking.Columns[11].Width = 120;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Para excluir um item é preciso selecionar uma linha! ", "Alerta");
            //}
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            string Insertar = string.Empty;
            try
            {
                List<string> listaPicking = new List<string>();
                List<int> listaPickin = new List<int>();
                if (GridPicking.Rows.Count > 0)
                {

                    for (int i = 0; i < GridPicking.Rows.Count; i++)
                    {

                        string Finalizado = GridPicking.Rows[i].Cells[7].Value.ToString();
                        Insertar = GridPicking.Rows[i].Cells[0].Value == null ? "False" : GridPicking.Rows[i].Cells[0].Value.ToString();

                        if (Insertar == "True")
                        {
                            if (Finalizado == "Finalizado")
                            {
                                //MessageBox.Show("Não é possivel Gerar uma lista para um Pedido já Fianlizado ", "Alerta");
                                Color MyRgbColor = new Color();
                                MyRgbColor = Color.Red;
                                GridPicking.Rows[i].DefaultCellStyle.BackColor = MyRgbColor;

                            }
                            else
                            {
                                listaPicking.Add(GridPicking.Rows[i].Cells[2].Value.ToString());
                                listaPickin.Add(int.Parse(GridPicking.Rows[i].Cells[2].Value.ToString()));
                                Color MyRgbColor = new Color();
                                MyRgbColor = Color.FromArgb(7, 158, 53);
                                GridPicking.Rows[i].DefaultCellStyle.BackColor = MyRgbColor;
                            }
                        }
                        else
                        {
                            if (Finalizado == "Finalizado")
                            {
                                //MessageBox.Show("Não é possivel Gerar uma lista para um Pedido já Fianlizado ", "Alerta");
                                Color MyRgbColor = new Color();
                                MyRgbColor = Color.Red;
                                GridPicking.Rows[i].DefaultCellStyle.BackColor = MyRgbColor;
                            }
                        }
                    }
                    GridPicking.Refresh();

                    foreach (var item in listaPicking)
                    {
                        _GestaoPickingBLL.AtualizaPickingPedido(item.ToString());
                    }

                    MessageBox.Show("Lista Gerada com Sucesso, Apenas para os Pedidos em verde! \n Pedidos em Vermelho já estão finalizados! ", "Alerta");
                    GerarRelatorio(listaPickin);

                }
                else
                {
                    MessageBox.Show("Para Confirmar é necessário efetuar uma pesquisa antes! ", "Alerta");

                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(Insertar))
                {
                    MessageBox.Show("Para Gerar a lista é necessário selecionar um pedido ! ", "Alerta");
                }
                else
                {
                    MessageBox.Show(ex.Message, "Erro!");
                }


                return;
            }

        }

        public static void CarregaFormulario()
        {
            //SAPbouiCOM.Form Form = SBOApp.Application.Forms.ActiveForm;
            //Form = SBOApp.Application.Forms.GetForm(ItemEventInfo.FormTypeEx, ItemEventInfo.FormTypeCount);
            //Form.DataSources.UserDataSources.Add("U_De", SAPbouiCOM.BoDataType.dt_SHORT_NUMBER, 10);

            //SAPbouiCOM.EditText oEdit = (SAPbouiCOM.EditText)Form.Items.Item("tx_PedIni").Specific;
            //oEdit.DataBind.SetBound(true, "", "U_De");


            //Form.DataSources.UserDataSources.Add("U_Ate", SAPbouiCOM.BoDataType.dt_SHORT_NUMBER, 10);
            //oEdit = (SAPbouiCOM.EditText)Form.Items.Item("tx_PedFin").Specific;
            //oEdit.DataBind.SetBound(true, "", "U_Ate");


            //Form.DataSources.UserDataSources.Add("U_DtIni", SAPbouiCOM.BoDataType.dt_DATE, 10);
            //oEdit = (SAPbouiCOM.EditText)Form.Items.Item("tx_dtIni").Specific;
            //oEdit.DataBind.SetBound(true, "", "U_DtIni");
            //oEdit.Value = DateTime.Now.ToString("yyyyMMdd");

            //Form.DataSources.UserDataSources.Add("U_DtFin", SAPbouiCOM.BoDataType.dt_DATE, 10);
            //oEdit = (SAPbouiCOM.EditText)Form.Items.Item("tx_dtFin").Specific;
            //oEdit.DataBind.SetBound(true, "", "U_DtFin");
            //oEdit.Value = DateTime.Now.ToString("yyyyMMdd");

            ////Form.DataSources.UserDataSources.Add("ud_PN", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 30);
            //SAPbouiCOM.EditText edit = (SAPbouiCOM.EditText)Form.Items.Item("tx_Cliente").Specific;
            //edit.DataBind.SetBound(true, "", "ud_PN");

            //SAPbouiCOM.CheckBox _edit = (SAPbouiCOM.CheckBox)Form.Items.Item("ck_Impre").Specific;
            //_edit.DataBind.SetBound(true, "", "ud_Ck");





            //ck_Impresso

            //SAPbouiCOM.EditText _edit = (SAPbouiCOM.EditText)Form.Items.Item("tx_Cliente").Specific;
            //edit.DataBind.SetBound(true, "", "ud_PN");
        }

        public void GerarRelatorio(List<int> ListaPickin)
        {
            var xml = new XMLReader();

            string PedidoIni = "0";
            string pedidoFin = "0";
            DateTime DtInicial = Convert.ToDateTime(tx_dataDe.Text);
            DateTime DtFinal = Convert.ToDateTime(tx_dataAte.Text);
            string cliente = "*";
            string chekBox = "N";

            PedidoIni = ListaPickin.Min().ToString();
            pedidoFin = ListaPickin.Max().ToString();

            if (string.IsNullOrEmpty(cliente))
            {
                cliente = "*";

            }

            CrystalReport crRelatorio = new CrystalReport();
            crRelatorio.ExecuteCrystalReport(@"C:\CVA Consultoria\Relatórios\Separação de Mercadoria.rpt", PedidoIni, pedidoFin, DtInicial, DtFinal, chekBox);
        }

        public void GerarRelatorioReImpressão(List<int> ListaPickin)
        {
            var xml = new XMLReader();

            string PedidoIni = "0";
            string pedidoFin = "0";
            DateTime DtInicial = Convert.ToDateTime(tx_dataDe.Text);
            DateTime DtFinal = Convert.ToDateTime(tx_dataAte.Text);
            string cliente = "*";
            string chekBox = "Y";

            PedidoIni = ListaPickin.Min().ToString();
            pedidoFin = ListaPickin.Max().ToString();

            if (string.IsNullOrEmpty(cliente))
            {
                cliente = "*";

            }

            CrystalReport crRelatorio = new CrystalReport();
            crRelatorio.ExecuteCrystalReport(@"C:\CVA Consultoria\Relatórios\Separação de Mercadoria.rpt", PedidoIni, pedidoFin, DtInicial, DtFinal, chekBox);

            //AtualizaPesquisa();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string Insertar = string.Empty;
            try
            {
                List<string> listaPicking = new List<string>();
                List<int> listaPickin = new List<int>();
                if (GridPicking.Rows.Count > 0)
                {

                    for (int i = 0; i < GridPicking.Rows.Count; i++)
                    {

                        string Impresso = GridPicking.Rows[i].Cells[1].Value.ToString();
                        Insertar = GridPicking.Rows[i].Cells[0].Value == null ? "False" : GridPicking.Rows[i].Cells[0].Value.ToString();

                        if (Insertar == "True")
                        {
                            if (Impresso == "Impresso")
                            {
                                listaPicking.Add(GridPicking.Rows[i].Cells[2].Value.ToString());
                                listaPickin.Add(int.Parse(GridPicking.Rows[i].Cells[2].Value.ToString()));
                                //MessageBox.Show("Não é possivel Gerar uma lista para um Pedido já Fianlizado ", "Alerta");
                                Color MyRgbColor = new Color();
                                MyRgbColor = Color.LightGray;
                                GridPicking.Rows[i].DefaultCellStyle.BackColor = MyRgbColor;

                            }
                            //    else
                            //    {
                            //        //listaPicking.Add(GridPicking.Rows[i].Cells[2].Value.ToString());
                            //        //listaPickin.Add(int.Parse(GridPicking.Rows[i].Cells[2].Value.ToString()));
                            //        Color MyRgbColor = new Color();
                            //        MyRgbColor = Color.FromArgb(7, 158, 53);
                            //        GridPicking.Rows[i].DefaultCellStyle.BackColor = MyRgbColor;
                            //    }
                            //}
                            //else
                            //{
                            //    if (Finalizado == "Finalizado")
                            //    {
                            //        //MessageBox.Show("Não é possivel Gerar uma lista para um Pedido já Fianlizado ", "Alerta");
                            //        Color MyRgbColor = new Color();
                            //        MyRgbColor = Color.Red;
                            //        GridPicking.Rows[i].DefaultCellStyle.BackColor = MyRgbColor;
                            //    }
                        }
                    }
                    GridPicking.Refresh();

                    foreach (var item in listaPicking)
                    {
                        _GestaoPickingBLL.AtualizaPickingPedido(item.ToString());
                    }

                    MessageBox.Show("Lista de Reimpressão Gerada com Sucesso, Apenas para os Pedidos em Cinza. ! ", "Alerta");
                    GerarRelatorioReImpressão(listaPickin);

                }
                else
                {
                    MessageBox.Show("Para ReImpressão é necessário efetuar uma pesquisa antes! ", "Alerta");

                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(Insertar))
                {
                    MessageBox.Show("Para Gerar a lista é necessário selecionar um pedido ! ", "Alerta");
                }
                else
                {
                    MessageBox.Show(ex.Message, "Erro!");
                }


                return;
            }
        }
    }
}



