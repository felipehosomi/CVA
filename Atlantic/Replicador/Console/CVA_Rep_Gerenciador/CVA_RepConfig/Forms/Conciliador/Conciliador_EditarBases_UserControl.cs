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
using CVA_Rep_Logging;
using System.IO;
using CVA_Rep_Exception;

namespace CVA_RepConfig.Forms.Conciliador
{
    public partial class Conciliador_EditarBases_UserControl : UserControl
    {
        private readonly ConciliadorDAL bll;
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly ILogger logger;

        public Conciliador_EditarBases_UserControl()
        {
            InitializeComponent();
            bll = new ConciliadorDAL();
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Conciliador_BasesForm>();
        }

        private void Conciliador_EditarBases_UserControl_Load(object sender, EventArgs e)
        {
            try
            {
                var dict = new Dictionary<string, int> { { "Não", 0 }, { "Sim", 1 } };
                cbUSE_TRU.DataSource = new BindingSource(dict, null);
                cbUSE_TRU.DisplayMember = "Key";
                cbUSE_TRU.ValueMember = "Value";

                var dict2 = new Dictionary<string, int>
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
                cbDB_TYPE.DataSource = new BindingSource(dict2, null);
                cbDB_TYPE.DisplayMember = "Key";
                cbDB_TYPE.ValueMember = "Value";

                var dict3 = new Dictionary<string, int> { { "Base consolidadora", 1 }, { "Base origem", 2 } };
                cbTIPO.DataSource = new BindingSource(dict3, null);
                cbTIPO.DisplayMember = "Key";
                cbTIPO.ValueMember = "Value";

                var ls = bll.Bases_GetAll();
                var ds = ToDataSet(ls.ToList());

                bindingSource1.DataSource = ds.Tables[0];
                tbID.DataBindings.Add(new Binding("Text", bindingSource1, "ID", true));
                tbCOMP.DataBindings.Add(new Binding("Text", bindingSource1, "BASE", true));
                tbPAS.DataBindings.Add(new Binding("Text", bindingSource1, "PASSWD", true));
                tbUNAME.DataBindings.Add(new Binding("Text", bindingSource1, "USERNAME", true));
                tbSRVR.DataBindings.Add(new Binding("Text", bindingSource1, "LICENSE_SERVER", true));
                cbTIPO.DataBindings.Add(new Binding("selectedValue", bindingSource1, "TIPO", true));
                cbUSE_TRU.DataBindings.Add(new Binding("selectedValue", bindingSource1, "USE_TRUSTED", true));
                cbDB_TYPE.DataBindings.Add(new Binding("selectedValue", bindingSource1, "DB_TYPE", true));
                tbDB_SRVR.DataBindings.Add(new Binding("Text", bindingSource1, "DB_SERVER", true));
                tbDB_UNAME.DataBindings.Add(new Binding("Text", bindingSource1, "DB_USERNAME", true));
                tbDB_PAS.DataBindings.Add(new Binding("Text", bindingSource1, "DB_PASSWD", true));
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message, ex);
                MessageBox.Show(
                    "Ocorreu um erro interno." + Environment.NewLine + "Por favor, verifique o arquivo de log.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Confirmar atualização do registro?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (tbCOMP.Text.Length > 200)
                        throw new GerenciadorException("Tamanho do campo <Nome do banco> maior do que o permitido.");
                    if (tbPAS.Text.Length > 200)
                        throw new GerenciadorException("Tamanho do campo <Senha> maior do que o permitido.");
                    if (tbUNAME.Text.Length > 200)
                        throw new GerenciadorException("Tamanho do campo <Usuário> maior do que o permitido.");
                    if (tbSRVR.Text.Length > 200)
                        throw new GerenciadorException(
                            "Tamanho do campo <Servidor de licença> maior do que o permitido.");
                    if (!tbSRVR.Text.Contains(":30000"))
                        throw new GerenciadorException(
                            "O campo <Servidor de licença> deve conter a porta do SLD. Exemplo: VM-002:30000");
                    if (cbTIPO.SelectedIndex.Equals(-1))
                        throw new GerenciadorException("O campo <Tipo de empresa> deve ser selecionado.");
                    if (cbUSE_TRU.SelectedIndex.Equals(-1))
                        throw new GerenciadorException("O campo <Habilitar SSL> deve ser selecionado.");
                    if (tbDB_PAS.Text.Length > 200)
                        throw new GerenciadorException("Tamanho do campo <Senha do banco> maior do que o permitido.");
                    if (tbDB_UNAME.Text.Length > 200)
                        throw new GerenciadorException("Tamanho do campo <Usuário do banco> maior do que o permitido.");
                    if (tbDB_SRVR.Text.Length > 200)
                        throw new GerenciadorException(
                            "Tamanho do campo <End. Servidor> maior do que o permitido.");
                    if (cbDB_TYPE.SelectedIndex.Equals(-1))
                        throw new GerenciadorException("O campo <Tipo de conexão> deve ser selecionado.");

                    var bas = bll.Bases_GetById(int.Parse(tbID.Text));
                    bas.BASE = tbCOMP.Text;
                    bas.PASSWD = tbPAS.Text;
                    bas.LICENSE_SERVER = tbSRVR.Text;
                    bas.USERNAME = tbUNAME.Text;
                    bas.USE_TRUSTED = int.Parse(cbUSE_TRU.SelectedValue.ToString());
                    bas.DB_SERVER = tbDB_SRVR.Text;
                    bas.DB_USERNAME = tbDB_UNAME.Text;
                    bas.DB_PASSWD = tbDB_PAS.Text;
                    bas.DB_TYPE = int.Parse(cbDB_TYPE.SelectedValue.ToString());
                    bas.TIPO = int.Parse(cbTIPO.SelectedValue.ToString());

                    bll.Bases_Update(bas);
                    MessageBox.Show("Base atualizada com sucesso.", "Concluído", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    logger.Info($"Base {tbID.Text} atualizada com sucesso.");
                }
                Hide();
                RaiseBaseFormReload();
            }
            catch (GerenciadorException ex)
            {
                logger.Error(ex.Message, ex);
                MessageBox.Show(
                    "Ocorreu um erro interno." + Environment.NewLine + "Por favor, verifique o arquivo de log.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void tbPAS_KeyDown(object sender, KeyEventArgs e)
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

        private void tbSRVR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
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

        private void tbCOMP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        private DataSet ToDataSet<T>(IList<T> list)
        {
            var elementType = typeof(T);
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

        public event EventHandler StatusUpdated;

        private void RaiseBaseFormReload()
        {
            StatusUpdated?.Invoke(new object(), new EventArgs());
        }
    }
}
