namespace CVA_RepConfig.Forms.Objeto
{
	partial class CadastrarObjeto_UserControl
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
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.cbSTU = new System.Windows.Forms.ComboBox();
            this.tbDescricao = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbObjeto = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbUPD = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbINS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(90, 211);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 38;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(9, 211);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 37;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // cbSTU
            // 
            this.cbSTU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSTU.FormattingEnabled = true;
            this.cbSTU.Location = new System.Drawing.Point(83, 80);
            this.cbSTU.Name = "cbSTU";
            this.cbSTU.Size = new System.Drawing.Size(100, 21);
            this.cbSTU.TabIndex = 26;
            // 
            // tbDescricao
            // 
            this.tbDescricao.Location = new System.Drawing.Point(83, 133);
            this.tbDescricao.Name = "tbDescricao";
            this.tbDescricao.Size = new System.Drawing.Size(279, 20);
            this.tbDescricao.TabIndex = 29;
            this.tbDescricao.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUNAME_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Descrição";
            // 
            // tbObjeto
            // 
            this.tbObjeto.Location = new System.Drawing.Point(83, 107);
            this.tbObjeto.Name = "tbObjeto";
            this.tbObjeto.Size = new System.Drawing.Size(279, 20);
            this.tbObjeto.TabIndex = 27;
            this.tbObjeto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSRVR_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Objeto";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Status";
            // 
            // tbUPD
            // 
            this.tbUPD.Enabled = false;
            this.tbUPD.Location = new System.Drawing.Point(262, 58);
            this.tbUPD.Name = "tbUPD";
            this.tbUPD.Size = new System.Drawing.Size(100, 20);
            this.tbUPD.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(187, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Atualizado";
            // 
            // tbINS
            // 
            this.tbINS.Enabled = false;
            this.tbINS.Location = new System.Drawing.Point(262, 32);
            this.tbINS.Name = "tbINS";
            this.tbINS.Size = new System.Drawing.Size(100, 20);
            this.tbINS.TabIndex = 23;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Cadastrado";
            // 
            // tbID
            // 
            this.tbID.Enabled = false;
            this.tbID.Location = new System.Drawing.Point(262, 6);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(100, 20);
            this.tbID.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "ID";
            // 
            // CadastrarObjeto_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.cbSTU);
            this.Controls.Add(this.tbDescricao);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbObjeto);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbUPD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbINS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.label1);
            this.Name = "CadastrarObjeto_UserControl";
            this.Size = new System.Drawing.Size(370, 240);
            this.Load += new System.EventHandler(this.CadastrarObjeto_UserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button btnCancelar;
		private System.Windows.Forms.Button btnSalvar;
		private System.Windows.Forms.ComboBox cbSTU;
		private System.Windows.Forms.TextBox tbDescricao;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbObjeto;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tbUPD;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbINS;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbID;
		private System.Windows.Forms.Label label1;
	}
}
