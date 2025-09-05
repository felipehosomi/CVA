using Conferencia.BLL;
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
    public partial class FrmListaConf2 : Form
    {
        PedidoBLL _PedidoBLL = new PedidoBLL();
        ListaConferenciaBLL _ListaConferenciaBLL = new ListaConferenciaBLL();
        public string Login = string.Empty;

        public FrmListaConf2(string logon)
        {
            InitializeComponent();
            Login = logon;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FrmLista1Conferencia_Load(object sender, EventArgs e)
        {
            tx_User.Text = "Usuário: " + Login;

            var listaConf =  _ListaConferenciaBLL.GetListaConf2();
            listaConf = listaConf.OrderBy(o => o.N_Pedido).ToList();

            GridLista1Conf.DataSource = null;
            GridLista1Conf.DataSource = listaConf;

            GridLista1Conf.Columns[0].Width = 80;
            GridLista1Conf.Columns[1].Width = 140;
            GridLista1Conf.Columns[2].Width = 120;
            GridLista1Conf.Columns[3].Width = 120;
            GridLista1Conf.Columns[4].Width = 280;
            GridLista1Conf.Columns[5].Width = 180;
            GridLista1Conf.Columns[6].Width = 370;

            tx_Pedido.Focus();

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (GridLista1Conf.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[0].Value.ToString().Equals(tx_Pedido.Text)).ToList().Count == 0)
                {
                    MessageBox.Show("Nº de pedido de venda não encontrado para a 2ª conferência.");
                    return;
                }

                this.Hide();

                _PedidoBLL.SetAberturaPicking2Conf(Convert.ToInt32(tx_Pedido.Text), Login.ToUpper(), DateTime.Now.ToString());
                _PedidoBLL.InsereStatusEmSeparação(Convert.ToInt32(tx_Pedido.Text));

                var frm = new FormConferencia2(Login, tx_Pedido.Text);
                frm.ShowDialog();               
            }
        }
    }
}
