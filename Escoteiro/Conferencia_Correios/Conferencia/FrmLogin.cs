using Conferencia.BLL;
using Conferencia.ConexaoSAP;
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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();

            var m = new Conexao();

            if (!Conexao.oCompany.Connected)
            {
                string message = $"Falha na conexão SAP: {Conexao.RetCode} - {Conexao.ErrMsg}";
                MessageBox.Show(message);
                throw new Exception(message);
            }
        }

        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            LoginBLL _LoginBll = new LoginBLL();

            string User = tx_Usuario.Text;
            string Senha = tx_senha.Text;

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                var ListLogin = _LoginBll.GeLogin(User.ToUpper(), Senha);

                if (ListLogin.Count > 0)
                {
                    foreach (var model in ListLogin)
                    {
                        var frm = new FrmInicial(User,model.Autorizacao);
                        frm.Show();

                        this.Hide();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Usuário ou Senha Inválidos !", "Falha de Login");
                    tx_senha.Text = "";
                    tx_senha.Focus();
                }
            }
        }
    }
}
