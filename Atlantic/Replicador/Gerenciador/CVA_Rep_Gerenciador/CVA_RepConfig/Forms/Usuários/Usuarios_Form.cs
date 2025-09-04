using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CVA_Rep_BLL;
using CVA_Rep_DAL;

namespace CVA_RepConfig.Forms.Usuários
{
    public partial class Usuarios_Form : UserControl
    {
        public UsuarioBLL _usuarioBLL = new UsuarioBLL();
        public List<USER> users;

        public Usuarios_Form()
        {
            InitializeComponent();
            this.users = GetUsers();
            FillCombo();
        }

        public void FillCombo()
        {
            foreach (var item in this.users)
                cbUser.Items.Add(item.USER_CODE + " - " + item.U_NAME);
        }

        private void UserSelected(object sender, EventArgs e)
        {
            tPassword.Text = _usuarioBLL.GeneratePassword();
        }

        private List<USER> GetUsers()
        {
            return _usuarioBLL.GetUsers();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            if (cbUser.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione um usuário!", "Atenção",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            var detectPipe = tPassword.Text.IndexOf("|", 0, tPassword.Text.Length);
            if (detectPipe >= 0)
            {
                MessageBox.Show("Sua senha contém caracteres inválidos. Por favor, informe uma nova senha", "Sucesso");
                return;
            }

            if (MessageBox.Show("Tem certeza que deseja alterar a senha deste usuário em todas as bases?", "Atenção",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_usuarioBLL.UpdatePassword(Convert.ToInt32(users[cbUser.SelectedIndex].USERID), tPassword.Text))
                {
                    MessageBox.Show("A nova senha será alterada no próximo ciclo de replicação!", "Sucesso");
                }
                else
                    MessageBox.Show("Ocorreu um erro ao gravar a senha. Tente novamente", "Atenção");
            }
        }
    }
}
