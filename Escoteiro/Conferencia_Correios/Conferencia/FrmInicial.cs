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
    public partial class FrmInicial : Form
    {
        public string Login = string.Empty;
        public string Autorizacao = string.Empty;

        public FrmInicial( string Logon, string Auto)
        {
            InitializeComponent();

            Login = Logon;
            Autorizacao = Auto;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var frm = new FrmPedidos(Login);

            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var frm = new Form1(Login);
            var frm = new FrmListaConf1(Login);

            frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            var frm = new FrmListaConf2(Login);
            frm.ShowDialog();
        }

        private void FrmInicial_Load(object sender, EventArgs e)
        {

            
            if (Autorizacao == "N")
            {
                btn_listaPedidos.Visible = false;
                btn_listaPedidos.Enabled = false;

                btn_GestaoPicking.Visible = false;
                btn_GestaoPicking.Enabled = false;

                btn_1Conf.Left = 32;
                btn_1Conf.Top = 89;

                btn_2Conf.Left = 176;
                btn_2Conf.Top = 89;
            }
        }

        private void btn_GestaoPicking_MouseClick(object sender, MouseEventArgs e)
        {
            var frm = new FrmGesaoPicking(Login);
            frm.ShowDialog(); 
        }

        private void button1_Click_1(object sender, EventArgs e)
        {   
            Application.Exit();
        }

        private void FrmInicial_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }            
        }
    }
}
