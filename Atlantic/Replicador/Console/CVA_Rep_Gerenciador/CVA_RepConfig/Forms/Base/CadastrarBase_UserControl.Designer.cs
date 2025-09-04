namespace CVA_RepConfig.Forms.Base
{
    partial class CadastrarBase_UserControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.tbINS = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbUPD = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbUNAME = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDB_SRVR = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbPAS = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbSTU = new System.Windows.Forms.ComboBox();
            this.cbUSE_TRU = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.tbCOMP = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbDB_TYP = new System.Windows.Forms.ComboBox();
            this.tbDB_PAS = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbDB_UNAME = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbSRVR = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(489, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // tbID
            // 
            this.tbID.Enabled = false;
            this.tbID.Location = new System.Drawing.Point(564, 7);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(100, 20);
            this.tbID.TabIndex = 1;
            // 
            // tbINS
            // 
            this.tbINS.Enabled = false;
            this.tbINS.Location = new System.Drawing.Point(564, 32);
            this.tbINS.Name = "tbINS";
            this.tbINS.Size = new System.Drawing.Size(100, 20);
            this.tbINS.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(489, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Cadastrado";
            // 
            // tbUPD
            // 
            this.tbUPD.Enabled = false;
            this.tbUPD.Location = new System.Drawing.Point(564, 57);
            this.tbUPD.Name = "tbUPD";
            this.tbUPD.Size = new System.Drawing.Size(100, 20);
            this.tbUPD.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(489, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Atualizado";
            // 
            // tbUNAME
            // 
            this.tbUNAME.Location = new System.Drawing.Point(112, 7);
            this.tbUNAME.Name = "tbUNAME";
            this.tbUNAME.Size = new System.Drawing.Size(100, 20);
            this.tbUNAME.TabIndex = 6;
            this.tbUNAME.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUNAME_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Usuário";
            // 
            // tbDB_SRVR
            // 
            this.tbDB_SRVR.Location = new System.Drawing.Point(307, 57);
            this.tbDB_SRVR.Name = "tbDB_SRVR";
            this.tbDB_SRVR.Size = new System.Drawing.Size(176, 20);
            this.tbDB_SRVR.TabIndex = 5;
            this.tbDB_SRVR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDB_SRVR_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(216, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Banco de dados";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Status do cadastro";
            // 
            // tbPAS
            // 
            this.tbPAS.Location = new System.Drawing.Point(307, 7);
            this.tbPAS.Name = "tbPAS";
            this.tbPAS.PasswordChar = '*';
            this.tbPAS.Size = new System.Drawing.Size(176, 20);
            this.tbPAS.TabIndex = 7;
            this.tbPAS.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPAS_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(216, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Senha";
            // 
            // cbSTU
            // 
            this.cbSTU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSTU.FormattingEnabled = true;
            this.cbSTU.Location = new System.Drawing.Point(112, 133);
            this.cbSTU.Name = "cbSTU";
            this.cbSTU.Size = new System.Drawing.Size(100, 21);
            this.cbSTU.TabIndex = 4;
            // 
            // cbUSE_TRU
            // 
            this.cbUSE_TRU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUSE_TRU.FormattingEnabled = true;
            this.cbUSE_TRU.Location = new System.Drawing.Point(307, 108);
            this.cbUSE_TRU.Name = "cbUSE_TRU";
            this.cbUSE_TRU.Size = new System.Drawing.Size(176, 21);
            this.cbUSE_TRU.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(216, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Habilitar SSL";
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(3, 214);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 17;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(84, 214);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 18;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // tbCOMP
            // 
            this.tbCOMP.Location = new System.Drawing.Point(112, 108);
            this.tbCOMP.Name = "tbCOMP";
            this.tbCOMP.Size = new System.Drawing.Size(100, 20);
            this.tbCOMP.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Empresa";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Tipo de conexão";
            // 
            // cbDB_TYP
            // 
            this.cbDB_TYP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDB_TYP.FormattingEnabled = true;
            this.cbDB_TYP.Location = new System.Drawing.Point(112, 57);
            this.cbDB_TYP.Name = "cbDB_TYP";
            this.cbDB_TYP.Size = new System.Drawing.Size(100, 21);
            this.cbDB_TYP.TabIndex = 22;
            // 
            // tbDB_PAS
            // 
            this.tbDB_PAS.Location = new System.Drawing.Point(307, 83);
            this.tbDB_PAS.Name = "tbDB_PAS";
            this.tbDB_PAS.PasswordChar = '*';
            this.tbDB_PAS.Size = new System.Drawing.Size(176, 20);
            this.tbDB_PAS.TabIndex = 24;
            this.tbDB_PAS.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDB_PAS_KeyDown);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(216, 87);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "Senha do banco";
            // 
            // tbDB_UNAME
            // 
            this.tbDB_UNAME.Location = new System.Drawing.Point(112, 83);
            this.tbDB_UNAME.Name = "tbDB_UNAME";
            this.tbDB_UNAME.Size = new System.Drawing.Size(100, 20);
            this.tbDB_UNAME.TabIndex = 23;
            this.tbDB_UNAME.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDB_UNAME_KeyDown);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 87);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Usuário do banco";
            // 
            // tbSRVR
            // 
            this.tbSRVR.Location = new System.Drawing.Point(112, 32);
            this.tbSRVR.Name = "tbSRVR";
            this.tbSRVR.Size = new System.Drawing.Size(371, 20);
            this.tbSRVR.TabIndex = 27;
            this.tbSRVR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSRVR_KeyDown);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 36);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(103, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "Servidor de licenças";
            // 
            // CadastrarBase_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.LightGreen;
            this.Controls.Add(this.tbSRVR);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tbDB_PAS);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbDB_UNAME);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cbDB_TYP);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbCOMP);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.cbUSE_TRU);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbSTU);
            this.Controls.Add(this.tbPAS);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbUNAME);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbDB_SRVR);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbUPD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbINS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.label1);
            this.Name = "CadastrarBase_UserControl";
            this.Size = new System.Drawing.Size(667, 251);
            this.Load += new System.EventHandler(this.CadastrarBase_UserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbID;
		private System.Windows.Forms.TextBox tbINS;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbUPD;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbUNAME;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbDB_SRVR;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tbPAS;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cbSTU;
		private System.Windows.Forms.ComboBox cbUSE_TRU;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button btnSalvar;
		private System.Windows.Forms.Button btnCancelar;
		private System.Windows.Forms.TextBox tbCOMP;
		private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbDB_TYP;
        private System.Windows.Forms.TextBox tbDB_PAS;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbDB_UNAME;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbSRVR;
        private System.Windows.Forms.Label label13;
    }
}
