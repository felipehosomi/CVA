using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CVA_RepConfig.HelperForms;
using CVA_Rep_BLL;
using CVA_Rep_DAL;
using CVA_Rep_Exception;
using CVA_Rep_Logging;

namespace CVA_RepConfig.Forms.Timer
{
    public partial class Timer_Form : UserControl
    {
        private readonly Intervalo_UserControl intervaloUserControl;
        private readonly ILogService logService = Log4NetService.Instance;
        private readonly ILogger logger;

        public Timer_Form()
        {
            InitializeComponent();
            intervaloUserControl = new Intervalo_UserControl();
            logService.Configure(
                new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\log4net.config"));
            logger = logService.GetLogger<Timer_Form>();
        }

        #region Events

        private void rb_Diario_CheckedChanged(object sender, EventArgs e)
        {
            pn_Opcoes.ClearControl();
            var diario = new Diario_UserControl();
            pn_Opcoes.Controls.Add(diario);
            diario.Show();
        }

        #endregion

        private void rb_Intervalo_CheckedChanged(object sender, EventArgs e)
        {
            pn_Opcoes.ClearControl();
            pn_Opcoes.Controls.Add(intervaloUserControl);
            intervaloUserControl.Show();
        }

        private void rb_Customizado_CheckedChanged(object sender, EventArgs e)
        {
            pn_Opcoes.ClearControl();
            var customizado = new Customizado_UserControl();
            pn_Opcoes.Controls.Add(customizado);
            customizado.Show();
        }

        private void bt_Salvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (rb_Intervalo.Checked)
                {
                    if (!string.IsNullOrEmpty(intervaloUserControl.tbId.Text))
                    {
                        var bll = new CVA_TIM_BLL();

                        var timer = bll.GetById(int.Parse(intervaloUserControl.tbId.Text));

                        if (timer == null)
                        {
                            throw new GerenciadorException("Timer não encontrado.");
                        }

                        timer.TIM = int.Parse(intervaloUserControl.tbIntervalo.Text);
                        timer.UPD = DateTime.Now;
                        timer.NUM_OBJ = int.Parse(intervaloUserControl.tbQuantidade.Text);

                        bll.Update(timer);

                        MessageBox.Show("Timer atualizado", "Timer atualizado", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        logger.Info($"Timer atualizado: TIM {timer.TIM} - NUM_OBJ {timer.NUM_OBJ}");
                    }
                    else
                    {
                        var timer = new CVA_TIM
                        {
                            TIM = int.Parse(intervaloUserControl.tbIntervalo.Text),
                            INS = DateTime.Now,
                            STU = 2,
                            NUM_OBJ = int.Parse(intervaloUserControl.tbQuantidade.Text)
                        };

                        var bll = new CVA_TIM_BLL();
                        bll.Add(timer);

                        timer = bll.GetAll().FirstOrDefault();

                        if (timer == null)
                        {
                            throw new GerenciadorException("Timer não encontrado.");
                        }
                        rb_Intervalo.Select();
                        intervaloUserControl.tbIntervalo.Text = timer.TIM.ToString();
                        intervaloUserControl.tbId.Text = timer.ID.ToString();
                        intervaloUserControl.tbQuantidade.Text = timer.NUM_OBJ.ToString();

                        MessageBox.Show("Timer atualizado", "Timer atualizado", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        logger.Info($"Timer atualizado: TIM {timer.TIM} - NUM_OBJ {timer.NUM_OBJ}");
                    }
                }
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
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void Timer_Form_Load(object sender, EventArgs e)
        {
            try
            {
                var bll = new CVA_TIM_BLL();

                var timer = bll.GetAll().FirstOrDefault(p => ((int) p.STU).Equals(2));

                if (timer == null)
                {
                    throw new GerenciadorException("Timer não encontrado.");
                }
                rb_Intervalo.Select();
                intervaloUserControl.tbIntervalo.Text = timer.TIM.ToString();
                intervaloUserControl.tbId.Text = timer.ID.ToString();
                intervaloUserControl.tbQuantidade.Text = timer.NUM_OBJ.ToString();
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
    }
}