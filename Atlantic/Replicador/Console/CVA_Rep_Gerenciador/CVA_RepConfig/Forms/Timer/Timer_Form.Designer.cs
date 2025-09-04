namespace CVA_RepConfig.Forms.Timer
{
	partial class Timer_Form
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.gb_Timer = new System.Windows.Forms.GroupBox();
			this.pn_Opcoes = new System.Windows.Forms.Panel();
			this.rb_Customizado = new System.Windows.Forms.RadioButton();
			this.rb_Intervalo = new System.Windows.Forms.RadioButton();
			this.rb_Diario = new System.Windows.Forms.RadioButton();
			this.bt_Salvar = new System.Windows.Forms.Button();
			this.gb_Timer.SuspendLayout();
			this.SuspendLayout();
			// 
			// gb_Timer
			// 
			this.gb_Timer.Controls.Add(this.pn_Opcoes);
			this.gb_Timer.Controls.Add(this.rb_Customizado);
			this.gb_Timer.Controls.Add(this.rb_Intervalo);
			this.gb_Timer.Controls.Add(this.rb_Diario);
			this.gb_Timer.Controls.Add(this.bt_Salvar);
			this.gb_Timer.Location = new System.Drawing.Point(0, 0);
			this.gb_Timer.Name = "gb_Timer";
			this.gb_Timer.Size = new System.Drawing.Size(712, 279);
			this.gb_Timer.TabIndex = 0;
			this.gb_Timer.TabStop = false;
			this.gb_Timer.Text = "Configurações do Timer";
			// 
			// pn_Opcoes
			// 
			this.pn_Opcoes.Location = new System.Drawing.Point(33, 52);
			this.pn_Opcoes.Name = "pn_Opcoes";
			this.pn_Opcoes.Size = new System.Drawing.Size(661, 143);
			this.pn_Opcoes.TabIndex = 6;
			// 
			// rb_Customizado
			// 
			this.rb_Customizado.AutoSize = true;
			this.rb_Customizado.Location = new System.Drawing.Point(163, 28);
			this.rb_Customizado.Name = "rb_Customizado";
			this.rb_Customizado.Size = new System.Drawing.Size(85, 17);
			this.rb_Customizado.TabIndex = 5;
			this.rb_Customizado.TabStop = true;
			this.rb_Customizado.Text = "Customizado";
			this.rb_Customizado.UseVisualStyleBackColor = true;
			this.rb_Customizado.CheckedChanged += new System.EventHandler(this.rb_Customizado_CheckedChanged);
			// 
			// rb_Intervalo
			// 
			this.rb_Intervalo.AutoSize = true;
			this.rb_Intervalo.Location = new System.Drawing.Point(91, 28);
			this.rb_Intervalo.Name = "rb_Intervalo";
			this.rb_Intervalo.Size = new System.Drawing.Size(66, 17);
			this.rb_Intervalo.TabIndex = 4;
			this.rb_Intervalo.TabStop = true;
			this.rb_Intervalo.Text = "Intervalo";
			this.rb_Intervalo.UseVisualStyleBackColor = true;
			this.rb_Intervalo.CheckedChanged += new System.EventHandler(this.rb_Intervalo_CheckedChanged);
			// 
			// rb_Diario
			// 
			this.rb_Diario.AutoSize = true;
			this.rb_Diario.Location = new System.Drawing.Point(33, 28);
			this.rb_Diario.Name = "rb_Diario";
			this.rb_Diario.Size = new System.Drawing.Size(52, 17);
			this.rb_Diario.TabIndex = 3;
			this.rb_Diario.TabStop = true;
			this.rb_Diario.Text = "Diário";
			this.rb_Diario.UseVisualStyleBackColor = true;
			this.rb_Diario.CheckedChanged += new System.EventHandler(this.rb_Diario_CheckedChanged);
			// 
			// bt_Salvar
			// 
			this.bt_Salvar.Location = new System.Drawing.Point(33, 201);
			this.bt_Salvar.Name = "bt_Salvar";
			this.bt_Salvar.Size = new System.Drawing.Size(109, 26);
			this.bt_Salvar.TabIndex = 2;
			this.bt_Salvar.Text = "Salvar";
			this.bt_Salvar.UseVisualStyleBackColor = true;
			this.bt_Salvar.Click += new System.EventHandler(this.bt_Salvar_Click);
			// 
			// Timer_Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gb_Timer);
			this.Name = "Timer_Form";
			this.Size = new System.Drawing.Size(712, 279);
			this.Load += new System.EventHandler(this.Timer_Form_Load);
			this.gb_Timer.ResumeLayout(false);
			this.gb_Timer.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gb_Timer;
		private System.Windows.Forms.Button bt_Salvar;
		private System.Windows.Forms.RadioButton rb_Diario;
		private System.Windows.Forms.RadioButton rb_Intervalo;
		private System.Windows.Forms.RadioButton rb_Customizado;
		private System.Windows.Forms.Panel pn_Opcoes;
	}
}
