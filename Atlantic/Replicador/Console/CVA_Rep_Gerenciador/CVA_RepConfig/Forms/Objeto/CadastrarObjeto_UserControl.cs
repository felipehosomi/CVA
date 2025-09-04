using System;
using System.Linq;
using System.Windows.Forms;
using CVA_Rep_BLL;
using CVA_Rep_DAL;

namespace CVA_RepConfig.Forms.Objeto
{
    public partial class CadastrarObjeto_UserControl : UserControl
    {
        private readonly CVA_OBJ_BLL bll;

        public CadastrarObjeto_UserControl()
        {
            InitializeComponent();
            bll = new CVA_OBJ_BLL();
        }

        public event EventHandler StatusUpdated;

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Confirmar a inclusão do registro?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var obj = new CVA_OBJ
                    {
                        INS = DateTime.Parse(tbINS.Text),
                        STU = int.Parse(cbSTU.SelectedValue.ToString()),
                        OBJ = tbObjeto.Text,
                        DSCR = tbDescricao.Text
                    };

                    bll.Add(obj);
                    MessageBox.Show("Objeto inserido com sucesso.", "Concluído", MessageBoxButtons.OK,
                        MessageBoxIcon.Information); 
                }
                Hide();
                RaiseBaseFormReload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Tem certeza que deseja sair do cadastro?", "Fechar", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Hide();
                RaiseBaseFormReload();
            }
        }

        private void tbSRVR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void tbUNAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void CadastrarObjeto_UserControl_Load(object sender, EventArgs e)
        {
            var bllStu = new CVA_STU_BLL();
            cbSTU.DataSource = bllStu.GetAll().ToList();
            cbSTU.DisplayMember = "STU";
            cbSTU.ValueMember = "ID";

            tbINS.Text = DateTime.Now.ToString();
        }

        private void RaiseBaseFormReload()
        {
            StatusUpdated?.Invoke(new object(), new EventArgs());
        }
    }
}