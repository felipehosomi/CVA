using System;
using System.Windows.Forms;
using CVA_RepConfig.Forms.Base;
using CVA_RepConfig.Forms.Log;
using CVA_RepConfig.Forms.Objeto;
using CVA_RepConfig.Forms.Timer;
using CVA_RepConfig.HelperForms;
using CVA_RepConfig.Forms.Conciliador;
using CVA_RepConfig.Forms.Emails;
using CVA_RepConfig.Forms.AcessoCVA;

namespace CVA_RepConfig
{
    public partial class Init : Form
    {
        public Init()
        {
            InitializeComponent();
            Resize += SetMinimizeState;
            notifyIcon1.Click += ToggleMinimizeState;
        }

        private void ToggleMinimizeState(object sender, EventArgs e)
        {
            var isMinimized = WindowState == FormWindowState.Minimized;
            WindowState = isMinimized ? FormWindowState.Normal : FormWindowState.Minimized;
        }

        private void SetMinimizeState(object sender, EventArgs e)
        {
            var isMinimized = WindowState == FormWindowState.Minimized;
            ShowInTaskbar = !isMinimized;
            notifyIcon1.Visible = isMinimized;
            if (isMinimized)
                notifyIcon1.ShowBalloonTip(500, "Application", "Application minimized to tray.", ToolTipIcon.Info);
        }


        private void ConfigurarTimerClick(object sender, EventArgs e)
        {
            panelView.ClearControl();
            var timer_Form = new Timer_Form();
            panelView.Controls.Add(timer_Form);
            timer_Form.Show();
        }

        private void Base_Click(object sender, EventArgs e)
        {
            panelView.ClearControl();
            var bases = new Base_Form();
            panelView.Controls.Add(bases);
            bases.Show();
        }

        private void objetosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelView.ClearControl();
            var objeto = new Objeto_Form();
            panelView.Controls.Add(objeto);
            objeto.Show();
        }

        private void registroDeLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelView.ClearControl();
            var objeto = new Log_Form();
            panelView.Controls.Add(objeto);
            objeto.Show();
        }

        private void basesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelView.ClearControl();
            var obj = new Conciliador_BasesForm();
            panelView.Controls.Add(obj);
            obj.Show();
        }

        private void deParaDeFiliaisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelView.ClearControl();
            var obj = new Conciliador_DeParaForm();
            panelView.Controls.Add(obj);
            obj.Show();
        }

        private void emailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelView.ClearControl();
            var obj = new Emails_Form();
            panelView.Controls.Add(obj);
            obj.Show();
        }

        private void acessoRestritoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelView.ClearControl();
            var obj = new AcessoCVA_Form();
            panelView.Controls.Add(obj);
            obj.Show();
        }
    }
}