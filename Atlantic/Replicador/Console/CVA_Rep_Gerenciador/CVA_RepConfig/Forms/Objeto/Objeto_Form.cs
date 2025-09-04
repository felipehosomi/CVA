using System;
using System.Windows.Forms;
using CVA_RepConfig.HelperForms;

namespace CVA_RepConfig.Forms.Objeto
{
    public partial class Objeto_Form : UserControl
    {
        public Objeto_Form()
        {
            InitializeComponent();
        }

        private void bt_Adicionar_Click(object sender, EventArgs e)
        {
            bt_Editar.Visible = true;
            bt_Adicionar.Visible = false;
            bt_Remover.Visible = true;
            bt_Refresh.Visible = true;

            pn_Opcoes.ClearControl();
            var consultar = new CadastrarObjeto_UserControl();
            pn_Opcoes.Controls.Add(consultar);
            consultar.StatusUpdated += Consultar_StatusUpdated;
            consultar.Show();
        }

        private void bt_Editar_Click(object sender, EventArgs e)
        {
            bt_Editar.Visible = false;
            bt_Adicionar.Visible = true;
            bt_Remover.Visible = true;
            bt_Refresh.Visible = true;

            pn_Opcoes.ClearControl();
            var consultar = new EditarObjeto_UserControl();
            pn_Opcoes.Controls.Add(consultar);
            consultar.StatusUpdated += Consultar_StatusUpdated;
            consultar.Show();
        }

        private void bt_Remover_Click(object sender, EventArgs e)
        {
            bt_Editar.Visible = true;
            bt_Adicionar.Visible = true;
            bt_Remover.Visible = false;
            bt_Refresh.Visible = true;

            pn_Opcoes.ClearControl();
            var consultar = new RemoverObjeto_UserControl();
            pn_Opcoes.Controls.Add(consultar);
            consultar.StatusUpdated += Consultar_StatusUpdated;
            consultar.Show();
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            FormReload();
        }

        private void Consultar_StatusUpdated(object sender, EventArgs e)
        {
            FormReload();
        }

        private void FormReload()
        {
            bt_Editar.Visible = true;
            bt_Adicionar.Visible = true;
            bt_Remover.Visible = true;
            bt_Refresh.Visible = true;

            pn_Opcoes.ClearControl();
            var consultar = new ConsultarObjeto_UserControl();
            pn_Opcoes.Controls.Add(consultar);
            consultar.Show();
        }

        private void Objeto_Form_Load(object sender, EventArgs e)
        {
            pn_Opcoes.ClearControl();
            var consultar = new ConsultarObjeto_UserControl();
            pn_Opcoes.Controls.Add(consultar);
            consultar.Show();
        }
    }
}