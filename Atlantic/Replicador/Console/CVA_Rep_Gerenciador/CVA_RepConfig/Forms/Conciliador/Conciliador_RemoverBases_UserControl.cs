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

namespace CVA_RepConfig.Forms.Conciliador
{
    public partial class Conciliador_RemoverBases_UserControl : UserControl
    {
        private readonly ConciliadorDAL bll;
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly ILogger logger;

        public Conciliador_RemoverBases_UserControl()
        {
            InitializeComponent();
            bll = new ConciliadorDAL();
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Conciliador_BasesForm>();
        }

        private void Conciliador_RemoverBases_UserControl_Load(object sender, EventArgs e)
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
                cbDB_TYP.DataSource = new BindingSource(dict2, null);
                cbDB_TYP.DisplayMember = "Key";
                cbDB_TYP.ValueMember = "Value";

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
                tbDB_SRVR.DataBindings.Add(new Binding("Text", bindingSource1, "DB_SERVER", true));
                tbDB_UNAME.DataBindings.Add(new Binding("Text", bindingSource1, "DB_USERNAME", true));
                tbDB_PAS.DataBindings.Add(new Binding("Text", bindingSource1, "DB_PASSWD", true));
                cbDB_TYP.DataBindings.Add(new Binding("selectedValue", bindingSource1, "DB_TYPE", true));
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
                if (MessageBox.Show("Confirmar a exclusão do registro?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var bas = bll.Bases_GetById(int.Parse(tbID.Text));

                    bll.Bases_Delete(bas);
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

        public event EventHandler StatusUpdated;

        private void RaiseBaseFormReload()
        {
            StatusUpdated?.Invoke(new object(), new EventArgs());
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
    }
}
