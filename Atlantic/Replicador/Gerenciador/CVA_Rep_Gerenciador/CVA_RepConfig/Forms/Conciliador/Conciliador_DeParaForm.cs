using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVA_RepConfig.HelperForms;

namespace CVA_RepConfig.Forms.Conciliador
{
    public partial class Conciliador_DeParaForm : UserControl
    {
        public Conciliador_DeParaForm()
        {
            InitializeComponent();
        }

        private void Conciliador_DeParaForm_Load(object sender, EventArgs e)
        {
            pn_Opcoes.ClearControl();
            var consultar = new Conciliador_DeParaConsultar();
            pn_Opcoes.Controls.Add(consultar);
            consultar.Show();
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            FormReload();
        }

        private void bt_Remover_Click(object sender, EventArgs e)
        {
            bt_Editar.Visible = true;
            bt_Adicionar.Visible = true;
            bt_Remover.Visible = false;
            bt_Refresh.Visible = true;

            pn_Opcoes.ClearControl();
            var consultar = new Conciliador_DeParaRemover();
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
            var consultar = new Conciliador_DeParaEditar();
            pn_Opcoes.Controls.Add(consultar);
            consultar.StatusUpdated += Consultar_StatusUpdated;
            consultar.Show();
        }

        private void bt_Adicionar_Click(object sender, EventArgs e)
        {
            bt_Editar.Visible = true;
            bt_Adicionar.Visible = false;
            bt_Remover.Visible = true;
            bt_Refresh.Visible = true;

            pn_Opcoes.ClearControl();
            var consultar = new Conciliador_DeParaCadastrar();
            pn_Opcoes.Controls.Add(consultar);
            consultar.StatusUpdated += Consultar_StatusUpdated;
            consultar.Show();
        }

        private void FormReload()
        {
            bt_Editar.Visible = true;
            bt_Adicionar.Visible = true;
            bt_Remover.Visible = true;
            bt_Refresh.Visible = true;

            pn_Opcoes.ClearControl();
            var consultar = new Conciliador_DeParaConsultar();
            pn_Opcoes.Controls.Add(consultar);
            consultar.Show();
        }

        private void Consultar_StatusUpdated(object sender, EventArgs e)
        {
            FormReload();
        }
    }
}
