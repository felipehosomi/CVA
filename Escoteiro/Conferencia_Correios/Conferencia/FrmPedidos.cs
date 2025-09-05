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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conferencia
{


    public partial class FrmPedidos : Form
    {
        public string Login = string.Empty;

        PedidoBLL _PedidoBLL = new PedidoBLL();

        public FrmPedidos(string Logon)
        {
            InitializeComponent();

            Login = Logon;
        }

        private void FrmPedidos_Load(object sender, EventArgs e)
        {
            Lbl_Usuario.Text = "Usuário: " + Login;

            CarregaComboBoxStaus();
            CarregaComboBoxFilial();
            CarregaComboBoxTipoEnvio();
            CarregaComboBoxUfCliente();


        }

        private void CarregaComboBoxStaus()
        {
            cb_Status.DataSource = _PedidoBLL.CarregaComboStatus();
            cb_Status.DisplayMember = "Descricao";
            cb_Status.ValueMember = "Code";
        }

        private void CarregaComboBoxFilial()
        {
            cb_filial.DataSource = _PedidoBLL.CarregaComboFilial();
            cb_filial.DisplayMember = "Descricao";
            cb_filial.ValueMember = "Code";
        }

        private void CarregaComboBoxTipoEnvio()
        {
            cb_TipoEnvio.DataSource = _PedidoBLL.CarregaComboTipoEnvio();
            cb_TipoEnvio.DisplayMember = "Descricao";
            cb_TipoEnvio.ValueMember = "Code";
        }

        private void CarregaComboBoxUfCliente()
        {
            cb_UfCliente.DataSource = _PedidoBLL.CarregaComboUfCliente();
            cb_UfCliente.DisplayMember = "Descricao";
            cb_UfCliente.ValueMember = "Code";
        }

        private void btn_Pesquisa_MouseClick(object sender, MouseEventArgs e)
        {
            AtualizaPesquisa();

        }

        private void AtualizaPesquisa()
        {
            DataGridViewCheckBoxColumn ck = new DataGridViewCheckBoxColumn();

            if (GridLiberados.Columns.Count > 0)
            {
                GridLiberados.DataSource = null;
                GridLiberados.Columns.Remove(GridLiberados.Columns[0]);
                GridLiberados.Refresh();
            }

            if (GridPendentes.Columns.Count > 0)
            {
                GridPendentes.DataSource = null;
                GridPendentes.Columns.Remove(GridPendentes.Columns[0]);
                GridPendentes.Refresh();

            }

            int Liberado_Status = cb_Status.SelectedIndex;
            int Liberado_Filial = Convert.ToInt32(cb_filial.SelectedValue);
            int Liberado_TipoEnvio = Convert.ToInt32(cb_TipoEnvio.SelectedValue);
            string Liberado_UfCliente = cb_UfCliente.SelectedValue.ToString();
            string Liberado_DataDe = DataDe.Value.ToString("yyyyMMdd");
            string Liberado_DataAte = DataAte.Value.ToString("yyyyMMdd");

            var listaLiberados = _PedidoBLL.ListaPedidosLiberado(Liberado_Status, Liberado_Filial, Liberado_TipoEnvio, Liberado_UfCliente, Liberado_DataDe, Liberado_DataAte);


            ck.HeaderText = "#";
            GridLiberados.Columns.Add(ck);

            GridLiberados.DataSource = null;
            GridLiberados.DataSource = listaLiberados;

            GridLiberados.Columns[0].Width = 20;
            GridLiberados.Columns[1].Width = 50;
            GridLiberados.Columns[2].Width = 70;
            GridLiberados.Columns[3].Width = 100;
            GridLiberados.Columns[4].Width = 70;
            GridLiberados.Columns[5].Width = 200;
            GridLiberados.Columns[6].Width = 190;
            GridLiberados.Columns[7].Width = 120;
            GridLiberados.Columns[8].Width = 100;

            GridLiberados.Columns[3].DefaultCellStyle.Format = "C2"; //"R$ 0.00##";



            for (int i = 0; i < GridLiberados.ColumnCount; i++)
            {
                if (i != 0)
                {                  
                    GridLiberados.Columns[i].ReadOnly = true;
                }
            }




            int Pendente_Status = cb_Status.SelectedIndex;
            int Pendente_Filial = Convert.ToInt32(cb_filial.SelectedValue);
            int Pendente_TipoEnvio = Convert.ToInt32(cb_TipoEnvio.SelectedValue);
            string Pendente_UfCliente = cb_UfCliente.SelectedValue.ToString();
            string Pendente_DataDe = DataDe.Value.ToString("yyyyMMdd");
            string Pendente_DataAte = DataAte.Value.ToString("yyyyMMdd");


            var listaPendentes = _PedidoBLL.ListaPedidosPendentes(Pendente_Status, Pendente_Filial, Pendente_TipoEnvio, Pendente_UfCliente, Pendente_DataDe, Pendente_DataAte);

            ck = new DataGridViewCheckBoxColumn();
            ck.HeaderText = "#";
            GridPendentes.Columns.Add(ck);

            GridPendentes.DataSource = null;
            GridPendentes.DataSource = listaPendentes;

            GridPendentes.Columns[0].Width = 20;
            GridPendentes.Columns[1].Width = 50;
            GridPendentes.Columns[2].Width = 70;
            GridPendentes.Columns[3].Width = 100;
            GridPendentes.Columns[4].Width = 70;
            GridPendentes.Columns[5].Width = 100;
            GridPendentes.Columns[6].Width = 200;
            GridPendentes.Columns[7].Width = 190;
            GridLiberados.Columns[8].Width = 120;

            GridPendentes.Columns[3].DefaultCellStyle.Format = "C2";

            for (int i = 0; i < GridPendentes.ColumnCount; i++)
            {
                if (i != 0)
                {                
                    GridPendentes.Columns[i].ReadOnly = true;                    
                }
            }


      
        }

        private void btn_confirmar_Liberados_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                // Grid Liberados
                List<string> listaPicking = new List<string>();
                if (GridLiberados.Rows.Count > 0)
                {
                    for (int i = 0; i < GridLiberados.Rows.Count; i++)
                    {
                        var Insertar = GridLiberados.Rows[i].Cells[0].Value == null ? "False" : GridLiberados.Rows[i].Cells[0].Value.ToString();

                        if (Insertar == "True")
                        {
                            listaPicking.Add(GridLiberados.Rows[i].Cells[1].Value.ToString());
                        }
                    }
                    if (listaPicking.Count > 0)
                    {
                        foreach (var item in listaPicking)
                        {
                            _PedidoBLL.AtualizaPickingPedido(item.ToString());
                        }

                        var res = MessageBox.Show("Pedidos Confirmados! Ir para Gestão de Picking?", "Alerta", MessageBoxButtons.YesNo);

                        if (res == DialogResult.Yes)
                        {
                            var frm = new FrmGesaoPicking(Login);
                            frm.ShowDialog();
                            this.Hide();
                        }
                        else
                        {
                            AtualizaPesquisa();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Para Confirmar é necessário Selecionar o Pedido antes! ", "Alerta");
                    }
                   
                }
                else
                {
                    MessageBox.Show("Para Confirmar é necessário efetuar uma pesquisa antes! ", "Alerta");

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Erro!");
                return;
            }

        }

        private void btn_Check_Liberados_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < GridLiberados.Rows.Count; i++)
            {
                GridLiberados.Rows[i].Cells[0].Value = true;
            }
        }

        private void btn_UnCheck_Liberados_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < GridLiberados.Rows.Count; i++)
            {
                GridLiberados.Rows[i].Cells[0].Value = false;
            }
        }

        private void btn_Confirmar_Pendentes_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                // Grid Liberados
                List<string> listaPicking = new List<string>();
                if (GridPendentes.Rows.Count > 0)
                {

                    for (int i = 0; i < GridPendentes.Rows.Count; i++)
                    {
                        
                        var Insertar = GridPendentes.Rows[i].Cells[0].Value == null ? "False" : GridPendentes.Rows[i].Cells[0].Value.ToString();

                        if (Insertar == "True")
                        {
                            listaPicking.Add(GridPendentes.Rows[i].Cells[1].Value.ToString());
                        }
                    }

                    if (listaPicking.Count > 0)
                    {
                        foreach (var item in listaPicking)
                        {
                            _PedidoBLL.AtualizaPickingPedido(item.ToString());
                        }

                        var res = MessageBox.Show("Pedidos Confirmados! Ir para Gestão de Picking?", "Alerta", MessageBoxButtons.YesNo);

                        if (res == DialogResult.Yes)
                        {
                            var frm = new FrmGesaoPicking(Login);
                            frm.ShowDialog();
                            this.Hide();
                        }
                        else
                        {
                            AtualizaPesquisa();
                            //this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Para Confirmar é necessário Selecionar o Pedido antes! ", "Alerta");
                    }
                
                }
                else
                {
                    MessageBox.Show("Para Confirmar é necessário efetuar uma pesquisa antes! ", "Alerta");

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Erro!");
                return;
            }
        }

        private void btn_check_Pendentes_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < GridPendentes.Rows.Count; i++)
            {
                GridPendentes.Rows[i].Cells[0].Value = true;
            }
        }

        private void btn_UnCheck_Pendentes_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < GridPendentes.Rows.Count; i++)
            {
                GridPendentes.Rows[i].Cells[0].Value = false;
            }
        }
    }
}
