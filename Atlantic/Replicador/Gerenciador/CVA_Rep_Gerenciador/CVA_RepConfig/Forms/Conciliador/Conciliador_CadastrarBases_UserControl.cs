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
    public partial class Conciliador_CadastrarBases_UserControl : UserControl
    {
        private readonly ConciliadorDAL bll;
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly ILogger logger;
        public event EventHandler StatusUpdated;

        public Conciliador_CadastrarBases_UserControl()
        {
            InitializeComponent();
            bll = new ConciliadorDAL();
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Conciliador_BasesForm>();
        }

        private void Conciliador_CadastrarBases_UserControl_Load(object sender, EventArgs e)
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
                if (
                    MessageBox.Show("Confirmar o cadastro do registro?", "Confirmação", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
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
                    if (cbUSE_TRU.SelectedIndex.Equals(-1))
                        throw new GerenciadorException("O campo <Habilitar SSL> deve ser selecionado.");
                    if (tbDB_PAS.Text.Length > 200)
                        throw new GerenciadorException("Tamanho do campo <Senha do banco> maior do que o permitido.");
                    if (tbDB_UNAME.Text.Length > 200)
                        throw new GerenciadorException("Tamanho do campo <Usuário do banco> maior do que o permitido.");
                    if (tbDB_SRVR.Text.Length > 200)
                        throw new GerenciadorException(
                            "Tamanho do campo <End. Servidor> maior do que o permitido.");
                    if (cbDB_TYP.SelectedIndex.Equals(-1))
                        throw new GerenciadorException("O campo <Tipo de conexão> deve ser selecionado.");
                    if (cbTIPO.SelectedIndex.Equals(-1))
                        throw new GerenciadorException("O campo <Tipo de empresa> deve ser selecionado.");

                    var bas = new CVA_BASES
                    {
                        BASE = tbCOMP.Text,
                        PASSWD = tbPAS.Text,
                        LICENSE_SERVER = tbSRVR.Text,
                        USERNAME = tbUNAME.Text,
                        USE_TRUSTED = int.Parse(cbUSE_TRU.SelectedValue.ToString()),
                        DB_USERNAME = tbDB_UNAME.Text,
                        DB_PASSWD = tbDB_PAS.Text,
                        DB_SERVER = tbDB_SRVR.Text,
                        DB_TYPE = int.Parse(cbDB_TYP.SelectedValue.ToString()),
                        TIPO = int.Parse(cbTIPO.SelectedValue.ToString())
                    };

                    bll.Bases_Insert(bas);
                    MessageBox.Show("Base inserida com sucesso.", "Concluído", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    logger.Info("Base inserida com sucesso.");
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

        private void RaiseBaseFormReload()
        {
            StatusUpdated?.Invoke(new object(), new EventArgs());
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
    }
}
