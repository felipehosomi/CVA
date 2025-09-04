using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CVA_Rep_DAL;
using System.Collections;

namespace CVA_RepConfig.Forms.Emails
{
    public partial class Emails_Form : UserControl
    {
        CVA_EML_Repository bll = new CVA_EML_Repository();
        bool isLineSelected = false;

        public Emails_Form()
        {
            InitializeComponent();
            Emails_UserControl_Load();
        }

        private void Emails_UserControl_Load()
        {
            var lst = bll.GetAll().ToList();
            
            foreach (var c in lst)
            {
                dgEmails.Rows.Add(c.ID, c.NOME, c.EMAIL);
            }
        }

        private void bSalvar_Click(object sender, EventArgs e)
        {
            if (isLineSelected)
            {
                var lineIndex = dgEmails.CurrentRow.Index;
                int Id = Convert.ToInt32(dgEmails.Rows[lineIndex].Cells["ID"].FormattedValue.ToString());
                CVA_EML obj = bll.GetSingle(Id);
                obj.NOME = dgEmails.Rows[lineIndex].Cells["NOME"].FormattedValue.ToString();
                obj.EMAIL = dgEmails.Rows[lineIndex].Cells["EMAIL"].FormattedValue.ToString();
                bll.Update(obj);
                bll.SaveChanges();
                dgEmails.ClearSelection();
            }
            else
            {
                if (tNome.Text.Equals("") || tEmail.Text.Equals(""))
                {
                    MessageBox.Show("Verifique os campos 'Nome' e 'Email'");
                }
                else
                {
                    CVA_EML obj = new CVA_EML();
                    obj.NOME = tNome.Text;
                    obj.EMAIL = tEmail.Text;

                    bll.Add(obj);
                    bll.SaveChanges();
                    dgEmails.Rows.Clear();
                    tNome.Text = null;
                    tEmail.Text = null;

                    Emails_UserControl_Load();
                }
            }
            isLineSelected = false;
        }

        private void bRemover_Click(object sender, EventArgs e)
        {

            var indexLinha = dgEmails.CurrentRow.Index;

            if (isLineSelected)
            {
                var linhaClicada = dgEmails.CurrentRow;

                int Id = Convert.ToInt32(dgEmails.Rows[indexLinha].Cells["ID"].FormattedValue.ToString());
                CVA_EML obj = bll.GetSingle(Id);
                bll.Remove(obj);
                bll.SaveChanges();

                dgEmails.Rows.Remove(linhaClicada);
                tNome.Text = null;
                tEmail.Text = null;
            }
            else
                MessageBox.Show("Selecione uma linha válida");

            isLineSelected = false;
        }


        private void bCancelar_Click(object sender, EventArgs e)
        {
            if (
    MessageBox.Show("Tem certeza que deseja sair do cadastro?", "Fechar", MessageBoxButtons.YesNo,
        MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Hide();
            }
        }

        private void dgEmails_Click(object sender, DataGridViewCellEventArgs e)
        {
            isLineSelected = true;
        }

        private void tNome_TextChanged(object sender, EventArgs e)
        {
            isLineSelected = false;
        }

        private void tEmail_TextChanged(object sender, EventArgs e)
        {
            isLineSelected = false;
        }
    }
}
