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
    public partial class Conciliador_DeParaCadastrar : UserControl
    {
        private readonly ConciliadorDAL bll;
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly ILogger logger;
        public event EventHandler StatusUpdated;

        public Conciliador_DeParaCadastrar()
        {
            InitializeComponent();
            bll = new ConciliadorDAL();
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Conciliador_DeParaForm>();
        }

        private void Conciliador_DeParaCadastrar_Load(object sender, EventArgs e)
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
                    if (cbBASE_DE.SelectedIndex.Equals(-1))
                        throw new GerenciadorException("O campo <Base origem> deve ser selecionado.");
                    if (cbFILIAL_DE.SelectedIndex.Equals(-1))
                        throw new GerenciadorException("O campo <Filial origem> deve ser selecionado.");
                    if (cbFILIAL_PARA.SelectedIndex.Equals(-1))
                        throw new GerenciadorException("O campo <Filial consolidadora> deve ser selecionado.");

                    var bas = new CVA_BASES_DE_PARA
                    {
                        BASE_DE = int.Parse(cbBASE_DE.SelectedValue.ToString()),
                        FILIAL_DE = int.Parse(cbFILIAL_DE.SelectedValue.ToString()),
                        NOME = tbNOME.Text,
                        CNPJ_FILIAL_DE = tbCNPJ_DE.Text,
                        CNPJ_FILIAL_PARA = tbCNPJ_PARA.Text,
                        FILIAL_PARA = int.Parse(cbFILIAL_PARA.SelectedValue.ToString())
                    };

                    bll.DePara_Insert(bas);
                    MessageBox.Show("De-Para inserido com sucesso.", "Concluído", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    logger.Info("De-Para inserido com sucesso.");
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

        private void cbBASE_DE_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                cbFILIAL_DE.Enabled = true;
                cbFILIAL_DE.DataSource = null;

                var cbSender = (ComboBox)sender;

                var dict = bll.DePara_GetFiliaisOrigem(Convert.ToInt32(cbSender.SelectedValue));
                cbFILIAL_DE.DataSource = new BindingSource(dict, null);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message, ex);
                MessageBox.Show(
                    "Ocorreu um erro interno." + Environment.NewLine + "Por favor, verifique o arquivo de log.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbFILIAL_DE_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                cbFILIAL_PARA.Enabled = true;
                tbNOME.Enabled = true;
                tbCNPJ_DE.Enabled = true;

                var cbSender = (ComboBox)sender;

                var sNome = bll.DePara_GetNomeFilialOrigem(Convert.ToInt32(cbBASE_DE.SelectedValue), Convert.ToInt32(cbSender.SelectedItem));
                tbNOME.Text = sNome;

                var sCnpj = bll.DePara_GetCNPJFilialOrigem(Convert.ToInt32(cbBASE_DE.SelectedValue), Convert.ToInt32(cbSender.SelectedItem));
                tbCNPJ_DE.Text = sCnpj;

                tbNOME.Enabled = false;
                tbCNPJ_DE.Enabled = false;

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

        private void cbFILIAL_PARA_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                tbCNPJ_PARA.Enabled = true;

                var cbSender = (ComboBox)sender;
                var key = ((KeyValuePair<int, string>)cbSender.SelectedItem).Key.ToString();
                var sCnpj = bll.DePara_GetCNPJFilialConciliadora(Convert.ToInt32(key));
                tbCNPJ_PARA.Text = sCnpj;
                tbCNPJ_PARA.Enabled = false;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message, ex);
                MessageBox.Show(
                    "Ocorreu um erro interno." + Environment.NewLine + "Por favor, verifique o arquivo de log.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
