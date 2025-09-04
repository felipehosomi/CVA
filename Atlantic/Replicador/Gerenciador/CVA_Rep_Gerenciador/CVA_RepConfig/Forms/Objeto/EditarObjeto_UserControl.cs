using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CVA_Rep_BLL;

namespace CVA_RepConfig.Forms.Objeto
{
    public partial class EditarObjeto_UserControl : UserControl
    {
        private readonly CVA_OBJ_BLL bll;

        public EditarObjeto_UserControl()
        {
            InitializeComponent();
            bll = new CVA_OBJ_BLL();
        }

        public event EventHandler StatusUpdated;

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

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Confirmar a alteração do registro?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var bas = bll.GetById(int.Parse(tbID.Text));
                    bas.OBJ = tbObjeto.Text;
                    bas.UPD = DateTime.Parse(tbUPD.Text);
                    bas.DSCR = tbDescricao.Text;
                    bas.STU = int.Parse(cbSTU.SelectedValue.ToString());

                    bll.Update(bas);
                    MessageBox.Show("Objeto atualizado com sucesso.", "Concluído", MessageBoxButtons.OK,
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

        private void tbObjeto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void tbDescricao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void RaiseBaseFormReload()
        {
            StatusUpdated?.Invoke(new object(), new EventArgs());
        }

        private void EditarObjeto_UserControl_Load(object sender, EventArgs e)
        {
            var bllStu = new CVA_STU_BLL();
            cbSTU.DataSource = bllStu.GetAll().ToList();
            cbSTU.DisplayMember = "STU";
            cbSTU.ValueMember = "ID";

            var ls = bll.GetAll();
            var ds = ToDataSet(ls.ToList());

            bindingSource1.DataSource = ds.Tables[0];
            tbID.DataBindings.Add(new Binding("Text", bindingSource1, "ID", true));
            tbINS.DataBindings.Add(new Binding("Text", bindingSource1, "INS", true));
            tbObjeto.DataBindings.Add(new Binding("Text", bindingSource1, "OBJ", true));
            tbDescricao.DataBindings.Add(new Binding("Text", bindingSource1, "DSCR", true));
            cbSTU.DataBindings.Add(new Binding("selectedValue", bindingSource1, "STU", true));

            tbUPD.Text = DateTime.Now.ToString();
        }

        private DataSet ToDataSet<T>(IList<T> list)
        {
            var elementType = typeof (T);
            var ds = new DataSet();
            var t = new DataTable();
            ds.Tables.Add(t);

            foreach (var propInfo in elementType.GetProperties())
            {
                var ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
                t.Columns.Add(propInfo.Name, ColType);
            }

            foreach (var item in list)
            {
                var row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            return ds;
        }
    }
}