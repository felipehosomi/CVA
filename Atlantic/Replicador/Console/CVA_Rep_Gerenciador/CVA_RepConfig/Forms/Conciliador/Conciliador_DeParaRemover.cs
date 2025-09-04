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
    public partial class Conciliador_DeParaRemover : UserControl
    {
        private readonly ConciliadorDAL bll;
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly ILogger logger;
        public event EventHandler StatusUpdated;

        public Conciliador_DeParaRemover()
        {
            InitializeComponent();
            bll = new ConciliadorDAL();
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Conciliador_DeParaForm>();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (
                    MessageBox.Show("Confirmar a exclusão do registro?", "Confirmação", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var bas = bll.DePara_GetById(Convert.ToInt32(tbID.Text));

                    bll.DePara_Delete(bas);
                    MessageBox.Show("De-Para removido com sucesso.", "Concluído", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    logger.Info("De-Para removido com sucesso.");
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

        private void Conciliador_DeParaRemover_Load(object sender, EventArgs e)
        {
            try
            {
                var dict = bll.DePara_GetFiliaisConciliadora();
                cbFILIAL_PARA.DataSource = new BindingSource(dict, null);
                cbFILIAL_PARA.DisplayMember = "Value";
                cbFILIAL_PARA.ValueMember = "Key";

                var dict2 = bll.Bases_GetAll().ToDictionary(d => d.ID, d => d.BASE);
                cbBASE_DE.DataSource = new BindingSource(dict2, null);
                cbBASE_DE.DisplayMember = "Value";
                cbBASE_DE.ValueMember = "Key";

                var ls = bll.DePara_GetAll();
                var ds = ToDataSet(ls.ToList());

                bindingSource1.DataSource = ds.Tables[0];
                tbID.DataBindings.Add(new Binding("Text", bindingSource1, "ID", true));
                cbBASE_DE.DataBindings.Add(new Binding("selectedValue", bindingSource1, "BASE_DE", true));
                cbFILIAL_PARA.DataBindings.Add(new Binding("selectedValue", bindingSource1, "FILIAL_PARA", true));
                tbNOME.DataBindings.Add(new Binding("Text", bindingSource1, "NOME", true));
                tbCNPJ_DE.DataBindings.Add(new Binding("Text", bindingSource1, "CNPJ_FILIAL_DE", true));
                tbCNPJ_PARA.DataBindings.Add(new Binding("Text", bindingSource1, "CNPJ_FILIAL_PARA", true));

                var dict3 = bll.DePara_GetFiliaisOrigem(Convert.ToInt32(cbBASE_DE.SelectedValue));
                cbFILIAL_DE.DataSource = new BindingSource(dict2, null);
                cbFILIAL_DE.DataBindings.Add(new Binding("selectedItem", bindingSource1, "FILIAL_DE", true));

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

        private void RaiseBaseFormReload()
        {
            StatusUpdated?.Invoke(new object(), new EventArgs());
        }
    }
}
