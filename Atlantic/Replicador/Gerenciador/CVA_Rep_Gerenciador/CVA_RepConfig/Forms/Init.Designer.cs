namespace CVA_RepConfig
{
	partial class Init
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Init));
            this.mn_Init = new System.Windows.Forms.MenuStrip();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mni_Timer = new System.Windows.Forms.ToolStripMenuItem();
            this.mni_ConfigurarTimer = new System.Windows.Forms.ToolStripMenuItem();
            this.cadastrosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mni_Base = new System.Windows.Forms.ToolStripMenuItem();
            this.objetosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.emailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.conciliadorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.basesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deParaDeFiliaisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ajudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sobreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registroDeLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelView = new System.Windows.Forms.Panel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.cVAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acessoRestritoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mn_Init.SuspendLayout();
            this.SuspendLayout();
            // 
            // mn_Init
            // 
            this.mn_Init.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem,
            this.mni_Timer,
            this.cadastrosToolStripMenuItem,
            this.conciliadorToolStripMenuItem,
            this.ajudaToolStripMenuItem,
            this.cVAToolStripMenuItem});
            this.mn_Init.Location = new System.Drawing.Point(0, 0);
            this.mn_Init.Name = "mn_Init";
            this.mn_Init.Size = new System.Drawing.Size(709, 24);
            this.mn_Init.TabIndex = 0;
            this.mn_Init.Text = "menuStrip1";
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sairToolStripMenuItem});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.arquivoToolStripMenuItem.Text = "Arquivo";
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.sairToolStripMenuItem.Text = "Sair";
            // 
            // mni_Timer
            // 
            this.mni_Timer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mni_ConfigurarTimer});
            this.mni_Timer.Name = "mni_Timer";
            this.mni_Timer.Size = new System.Drawing.Size(50, 20);
            this.mni_Timer.Text = "Timer";
            // 
            // mni_ConfigurarTimer
            // 
            this.mni_ConfigurarTimer.Name = "mni_ConfigurarTimer";
            this.mni_ConfigurarTimer.Size = new System.Drawing.Size(131, 22);
            this.mni_ConfigurarTimer.Text = "Configurar";
            this.mni_ConfigurarTimer.Click += new System.EventHandler(this.ConfigurarTimerClick);
            // 
            // cadastrosToolStripMenuItem
            // 
            this.cadastrosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mni_Base,
            this.objetosToolStripMenuItem,
            this.emailsToolStripMenuItem});
            this.cadastrosToolStripMenuItem.Name = "cadastrosToolStripMenuItem";
            this.cadastrosToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.cadastrosToolStripMenuItem.Text = "Cadastros";
            // 
            // mni_Base
            // 
            this.mni_Base.Name = "mni_Base";
            this.mni_Base.Size = new System.Drawing.Size(115, 22);
            this.mni_Base.Text = "Base";
            this.mni_Base.Click += new System.EventHandler(this.Base_Click);
            // 
            // objetosToolStripMenuItem
            // 
            this.objetosToolStripMenuItem.Name = "objetosToolStripMenuItem";
            this.objetosToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.objetosToolStripMenuItem.Text = "Objetos";
            this.objetosToolStripMenuItem.Click += new System.EventHandler(this.objetosToolStripMenuItem_Click);
            // 
            // emailsToolStripMenuItem
            // 
            this.emailsToolStripMenuItem.Name = "emailsToolStripMenuItem";
            this.emailsToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.emailsToolStripMenuItem.Text = "Emails";
            this.emailsToolStripMenuItem.Click += new System.EventHandler(this.emailsToolStripMenuItem_Click);
            // 
            // conciliadorToolStripMenuItem
            // 
            this.conciliadorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.basesToolStripMenuItem,
            this.deParaDeFiliaisToolStripMenuItem});
            this.conciliadorToolStripMenuItem.Name = "conciliadorToolStripMenuItem";
            this.conciliadorToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.conciliadorToolStripMenuItem.Text = "Consolidador";
            // 
            // basesToolStripMenuItem
            // 
            this.basesToolStripMenuItem.Name = "basesToolStripMenuItem";
            this.basesToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.basesToolStripMenuItem.Text = "Bases";
            this.basesToolStripMenuItem.Click += new System.EventHandler(this.basesToolStripMenuItem_Click);
            // 
            // deParaDeFiliaisToolStripMenuItem
            // 
            this.deParaDeFiliaisToolStripMenuItem.Name = "deParaDeFiliaisToolStripMenuItem";
            this.deParaDeFiliaisToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.deParaDeFiliaisToolStripMenuItem.Text = "De-Para de Filiais";
            this.deParaDeFiliaisToolStripMenuItem.Click += new System.EventHandler(this.deParaDeFiliaisToolStripMenuItem_Click);
            // 
            // ajudaToolStripMenuItem
            // 
            this.ajudaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manualToolStripMenuItem,
            this.sobreToolStripMenuItem,
            this.registroDeLogToolStripMenuItem});
            this.ajudaToolStripMenuItem.Name = "ajudaToolStripMenuItem";
            this.ajudaToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.ajudaToolStripMenuItem.Text = "Ajuda";
            // 
            // manualToolStripMenuItem
            // 
            this.manualToolStripMenuItem.Name = "manualToolStripMenuItem";
            this.manualToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.manualToolStripMenuItem.Text = "Manual";
            // 
            // sobreToolStripMenuItem
            // 
            this.sobreToolStripMenuItem.Name = "sobreToolStripMenuItem";
            this.sobreToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.sobreToolStripMenuItem.Text = "Sobre";
            // 
            // registroDeLogToolStripMenuItem
            // 
            this.registroDeLogToolStripMenuItem.Name = "registroDeLogToolStripMenuItem";
            this.registroDeLogToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.registroDeLogToolStripMenuItem.Text = "Registro de log";
            this.registroDeLogToolStripMenuItem.Click += new System.EventHandler(this.registroDeLogToolStripMenuItem_Click);
            // 
            // panelView
            // 
            this.panelView.AutoSize = true;
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.Location = new System.Drawing.Point(0, 24);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(709, 349);
            this.panelView.TabIndex = 1;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Configurador";
            this.notifyIcon1.Visible = true;
            // 
            // cVAToolStripMenuItem
            // 
            this.cVAToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acessoRestritoToolStripMenuItem});
            this.cVAToolStripMenuItem.Name = "cVAToolStripMenuItem";
            this.cVAToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.cVAToolStripMenuItem.Text = "CVA";
            // 
            // acessoRestritoToolStripMenuItem
            // 
            this.acessoRestritoToolStripMenuItem.Name = "acessoRestritoToolStripMenuItem";
            this.acessoRestritoToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.acessoRestritoToolStripMenuItem.Text = "Acesso Restrito";
            this.acessoRestritoToolStripMenuItem.Click += new System.EventHandler(this.acessoRestritoToolStripMenuItem_Click);
            // 
            // Init
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 373);
            this.Controls.Add(this.panelView);
            this.Controls.Add(this.mn_Init);
            this.MainMenuStrip = this.mn_Init;
            this.MaximizeBox = false;
            this.Name = "Init";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CVA Consultoria - Configuração";
            this.mn_Init.ResumeLayout(false);
            this.mn_Init.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mn_Init;
		private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mni_Timer;
		private System.Windows.Forms.ToolStripMenuItem mni_ConfigurarTimer;
		private System.Windows.Forms.ToolStripMenuItem cadastrosToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mni_Base;
		private System.Windows.Forms.ToolStripMenuItem objetosToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ajudaToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem manualToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sobreToolStripMenuItem;
		public System.Windows.Forms.Panel panelView;
		private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripMenuItem registroDeLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem conciliadorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem basesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deParaDeFiliaisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cVAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acessoRestritoToolStripMenuItem;
    }
}

