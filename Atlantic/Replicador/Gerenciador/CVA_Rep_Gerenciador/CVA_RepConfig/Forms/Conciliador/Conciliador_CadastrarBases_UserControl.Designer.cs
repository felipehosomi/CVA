namespace CVA_RepConfig.Forms.Conciliador
{
    partial class Conciliador_CadastrarBases_UserControl
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
            this.tbSRVR = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbDB_PAS = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbDB_UNAME = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbDB_TYP = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbCOMP = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.cbUSE_TRU = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbPAS = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbUNAME = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDB_SRVR = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTIPO = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbSRVR
            // 
            this.tbSRVR.Location = new System.Drawing.Point(112, 64);
            this.tbSRVR.Name = "tbSRVR";
            this.tbSRVR.Size = new System.Drawing.Size(371, 20);
            this.tbSRVR.TabIndex = 4;
            this.tbSRVR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSRVR_KeyDown);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 68);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(103, 13);
            this.label13.TabIndex = 56;
            this.label13.Text = "Servidor de licenças";
            // 
            // tbDB_PAS
            // 
            this.tbDB_PAS.Location = new System.Drawing.Point(307, 115);
            this.tbDB_PAS.Name = "tbDB_PAS";
            this.tbDB_PAS.PasswordChar = '*';
            this.tbDB_PAS.Size = new System.Drawing.Size(176, 20);
            this.tbDB_PAS.TabIndex = 8;
            this.tbDB_PAS.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDB_PAS_KeyDown);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(216, 119);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 13);
            this.label11.TabIndex = 54;
            this.label11.Text = "Senha do banco";
            // 
            // tbDB_UNAME
            // 
            this.tbDB_UNAME.Location = new System.Drawing.Point(112, 115);
            this.tbDB_UNAME.Name = "tbDB_UNAME";
            this.tbDB_UNAME.Size = new System.Drawing.Size(100, 20);
            this.tbDB_UNAME.TabIndex = 7;
            this.tbDB_UNAME.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDB_UNAME_KeyDown);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 119);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 13);
            this.label12.TabIndex = 53;
            this.label12.Text = "Usuário do banco";
            // 
            // cbDB_TYP
            // 
            this.cbDB_TYP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDB_TYP.FormattingEnabled = true;
            this.cbDB_TYP.Location = new System.Drawing.Point(112, 89);
            this.cbDB_TYP.Name = "cbDB_TYP";
            this.cbDB_TYP.Size = new System.Drawing.Size(100, 21);
            this.cbDB_TYP.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 93);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 13);
            this.label10.TabIndex = 49;
            this.label10.Text = "Tipo de conexão";
            // 
            // tbCOMP
            // 
            this.tbCOMP.Location = new System.Drawing.Point(112, 140);
            this.tbCOMP.Name = "tbCOMP";
            this.tbCOMP.Size = new System.Drawing.Size(100, 20);
            this.tbCOMP.TabIndex = 9;
            this.tbCOMP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbCOMP_KeyDown);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 144);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 13);
            this.label9.TabIndex = 48;
            this.label9.Text = "Nome do banco";
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(84, 217);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 13;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(3, 217);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 12;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // cbUSE_TRU
            // 
            this.cbUSE_TRU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUSE_TRU.FormattingEnabled = true;
            this.cbUSE_TRU.Location = new System.Drawing.Point(307, 140);
            this.cbUSE_TRU.Name = "cbUSE_TRU";
            this.cbUSE_TRU.Size = new System.Drawing.Size(176, 21);
            this.cbUSE_TRU.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(216, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 45;
            this.label8.Text = "Habilitar SSL";
            // 
            // tbPAS
            // 
            this.tbPAS.Location = new System.Drawing.Point(307, 39);
            this.tbPAS.Name = "tbPAS";
            this.tbPAS.PasswordChar = '*';
            this.tbPAS.Size = new System.Drawing.Size(176, 20);
            this.tbPAS.TabIndex = 3;
            this.tbPAS.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPAS_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(216, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 44;
            this.label7.Text = "Senha";
            // 
            // tbUNAME
            // 
            this.tbUNAME.Location = new System.Drawing.Point(112, 39);
            this.tbUNAME.Name = "tbUNAME";
            this.tbUNAME.Size = new System.Drawing.Size(100, 20);
            this.tbUNAME.TabIndex = 2;
            this.tbUNAME.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbUNAME_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 43;
            this.label4.Text = "Usuário";
            // 
            // tbDB_SRVR
            // 
            this.tbDB_SRVR.Location = new System.Drawing.Point(307, 89);
            this.tbDB_SRVR.Name = "tbDB_SRVR";
            this.tbDB_SRVR.Size = new System.Drawing.Size(176, 20);
            this.tbDB_SRVR.TabIndex = 6;
            this.tbDB_SRVR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDB_SRVR_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(216, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 40;
            this.label5.Text = "End. Servidor";
            // 
            // tbID
            // 
            this.tbID.Enabled = false;
            this.tbID.Location = new System.Drawing.Point(307, 10);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(176, 20);
            this.tbID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(216, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "ID";
            // 
            // cbTIPO
            // 
            this.cbTIPO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTIPO.FormattingEnabled = true;
            this.cbTIPO.Location = new System.Drawing.Point(112, 166);
            this.cbTIPO.Name = "cbTIPO";
            this.cbTIPO.Size = new System.Drawing.Size(100, 21);
            this.cbTIPO.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 170);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 58;
            this.label6.Text = "Tipo de empresa";
            // 
            // Conciliador_CadastrarBases_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.LightGreen;
            this.Controls.Add(this.cbTIPO);
            this.Controls.Add(this.label6);
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
            this.Controls.Add(this.tbPAS);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbUNAME);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbDB_SRVR);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.label1);
            this.Name = "Conciliador_CadastrarBases_UserControl";
            this.Size = new System.Drawing.Size(501, 251);
            this.Load += new System.EventHandler(this.Conciliador_CadastrarBases_UserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSRVR;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbDB_PAS;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbDB_UNAME;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbDB_TYP;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbCOMP;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.ComboBox cbUSE_TRU;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbPAS;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbUNAME;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDB_SRVR;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbTIPO;
        private System.Windows.Forms.Label label6;
    }
}
