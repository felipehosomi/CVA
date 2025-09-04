using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CVA_Rep_BLL;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;

namespace CVA_RepConfig.Forms.Base
{
    public partial class CadastrarBase_UserControl : UserControl
    {
        private readonly CVA_BAS_BLL bll;
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly ILogger logger;

        public CadastrarBase_UserControl()
        {
            InitializeComponent();
            bll = new CVA_BAS_BLL();
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Base_Form>();
        }

        public event EventHandler StatusUpdated;

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (
                    MessageBox.Show("Confirmar o cadastro do registro?", "Confirmação", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (tbCOMP.Text.Length > 200)
                        throw new GerenciadorException("Tamanho do campo <Empresa> maior do que o permitido.");
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
                    if (cbSTU.SelectedIndex.Equals(-1))
                        throw new GerenciadorException("O campo <Status do cadastro> deve ser selecionado.");
                    if (cbUSE_TRU.SelectedIndex.Equals(-1))
                        throw new GerenciadorException("O campo <Habilitar SSL> deve ser selecionado.");
                    if (tbDB_PAS.Text.Length > 200)
                        throw new GerenciadorException("Tamanho do campo <Senha do banco> maior do que o permitido.");
                    if (tbDB_UNAME.Text.Length > 200)
                        throw new GerenciadorException("Tamanho do campo <Usuário do banco> maior do que o permitido.");
                    if (tbDB_SRVR.Text.Length > 200)
                        throw new GerenciadorException(
                            "Tamanho do campo <Banco de dados> maior do que o permitido.");
                    if (cbDB_TYP.SelectedIndex.Equals(-1))
                        throw new GerenciadorException("O campo <Tipo de conexão> deve ser selecionado.");

                    var bas = new CVA_BAS
                    {
                        COMP = tbCOMP.Text,
                        INS = DateTime.Parse(tbINS.Text),
                        PAS = tbPAS.Text,
                        STU = int.Parse(cbSTU.SelectedValue.ToString()),
                        SRVR = tbSRVR.Text,
                        UNAME = tbUNAME.Text,
                        USE_TRU = int.Parse(cbUSE_TRU.SelectedValue.ToString()),
                        DB_UNAME = tbDB_UNAME.Text,
                        DB_PAS = tbDB_PAS.Text,
                        DB_SRVR = tbDB_SRVR.Text,
                        DB_TYP = int.Parse(cbDB_TYP.SelectedValue.ToString())
                    };

                    bll.Add(bas);
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

        private void CadastrarBase_UserControl_Load(object sender, EventArgs e)
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

                // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                tbINS.Text = DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message, ex);
                MessageBox.Show(
                    "Ocorreu um erro interno." + Environment.NewLine + "Por favor, verifique o arquivo de log.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RaiseBaseFormReload()
        {
            StatusUpdated?.Invoke(new object(), new EventArgs());
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

        private void tbDB_SRVR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}