using System;
using System.IO;
using System.Windows.Forms;
using CVA_RepConfig.HelperForms;
using CVA_Rep_Logging;

namespace CVA_RepConfig.Forms.Base
{
    public partial class Base_Form : UserControl
    {
        public Base_Form()
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
            var consultar = new CadastrarBase_UserControl();
            pn_Opcoes.Controls.Add(consultar);
            consultar.StatusUpdated += Consultar_StatusUpdated;
            consultar.Show();
        }

        private void Consultar_StatusUpdated(object sender, EventArgs e)
        {
            FormReload();
        }

        private void Base_Form_Load(object sender, EventArgs e)
        {
            pn_Opcoes.ClearControl();
            var consultar = new ConsultarBase_UserControl();
            pn_Opcoes.Controls.Add(consultar);
            consultar.Show();
        }

        private void FormReload()
        {
            bt_Editar.Visible = true;
            bt_Adicionar.Visible = true;
            bt_Remover.Visible = true;
            bt_Refresh.Visible = true;

            pn_Opcoes.ClearControl();
            var consultar = new ConsultarBase_UserControl();
            pn_Opcoes.Controls.Add(consultar);
            consultar.Show();
        }

        private void bt_Editar_Click(object sender, EventArgs e)
        {
            bt_Editar.Visible = false;
            bt_Adicionar.Visible = true;
            bt_Remover.Visible = true;
            bt_Refresh.Visible = true;

            pn_Opcoes.ClearControl();
            var consultar = new EditarBase_UserControl();
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
            var consultar = new RemoverBase_UserControl();
            pn_Opcoes.Controls.Add(consultar);
            consultar.StatusUpdated += Consultar_StatusUpdated;
            consultar.Show();
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            FormReload();
        }
    }
}