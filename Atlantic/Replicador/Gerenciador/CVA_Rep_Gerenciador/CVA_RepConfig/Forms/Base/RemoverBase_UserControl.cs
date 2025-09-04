using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CVA_Rep_BLL;
using CVA_Rep_Logging;

namespace CVA_RepConfig.Forms.Base
{
    public partial class RemoverBase_UserControl : UserControl
    {
        private readonly CVA_BAS_BLL bll;
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly ILogger logger;

        public RemoverBase_UserControl()
        {
            InitializeComponent();
            bll = new CVA_BAS_BLL();
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Base_Form>();
        }

        public event EventHandler StatusUpdated;

        private void RaiseBaseFormReload()
        {
            StatusUpdated?.Invoke(new object(), new EventArgs());
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Confirmar a exclusão do registro?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var bas = bll.GetById(int.Parse(tbID.Text));

                    bll.Delete(bas);
                    MessageBox.Show("Base removida com sucesso.", "Concluído", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    logger.Info($"Base {tbID.Text} removida com sucesso.");
                }
                Hide();
                RaiseBaseFormReload();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message, ex);
                MessageBox.Show(
                    "Ocorreu um erro interno." + Environment.NewLine + "Por favor, verifique o arquivo de log.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void tbPAS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void tbCOMP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void RemoverBase_UserControl_Load(object sender, EventArgs e)
        {
            try
            {
                var bllStu = new CVA_STU_BLL();
                cbSTU.DataSource = bllStu.GetAll().ToList();
                cbSTU.DisplayMember = "STU";
                cbSTU.ValueMember = "ID";
                var dict = new Dictionary<string, int> { { "Não", 0 }, { "Sim", 1 } };
                cbUSE_TRU.DataSource = new BindingSource(dict, null);
                cbUSE_TRU.DisplayMember = "Key";
                cbUSE_TRU.ValueMember = "Value";
                dict = new Dictionary<string, int>
            {
                {"MSSQL", 1},
                {"DB2", 2},
                {"SYBASE", 3},
                {"MSSQL 2005", 4},
                {"MAXDB", 5},
                {"MSSQL 2008", 6},
                {"MSSQL 2012", 7},
                {"MSSQL 2014", 8},
                {"HANA DB", 9}
            };
                cbDB_TYP.DataSource = new BindingSource(dict, null);
                cbDB_TYP.DisplayMember = "Key";
                cbDB_TYP.ValueMember = "Value";

                var ls = bll.GetAll();
                var ds = ToDataSet(ls.ToList());

                bindingSource1.DataSource = ds.Tables[0];
                tbID.DataBindings.Add(new Binding("Text", bindingSource1, "ID", true));
                tbINS.DataBindings.Add(new Binding("Text", bindingSource1, "INS", true));
                tbCOMP.DataBindings.Add(new Binding("Text", bindingSource1, "COMP", true));
                tbPAS.DataBindings.Add(new Binding("Text", bindingSource1, "PAS", true));
                tbUNAME.DataBindings.Add(new Binding("Text", bindingSource1, "UNAME", true));
                tbSRVR.DataBindings.Add(new Binding("Text", bindingSource1, "SRVR", true));
                cbSTU.DataBindings.Add(new Binding("selectedValue", bindingSource1, "STU", true));
                cbUSE_TRU.DataBindings.Add(new Binding("selectedValue", bindingSource1, "USE_TRU", true));
                tbDB_SRVR.DataBindings.Add(new Binding("Text", bindingSource1, "DB_SRVR", true));
                tbDB_UNAME.DataBindings.Add(new Binding("Text", bindingSource1, "DB_UNAME", true));
                tbDB_PAS.DataBindings.Add(new Binding("Text", bindingSource1, "DB_PAS", true));
                cbDB_TYP.DataBindings.Add(new Binding("selectedValue", bindingSource1, "DB_TYP", true));

                tbUPD.Text = DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message, ex);
                MessageBox.Show(
                    "Ocorreu um erro interno." + Environment.NewLine + "Por favor, verifique o arquivo de log.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void tbDB_SRVR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void tbDB_UNAME_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void tbDB_PAS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}