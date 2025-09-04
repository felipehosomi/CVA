namespace CVA_RepConfig.Forms.Conciliador
{
    partial class Conciliador_DeParaCadastrar
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
            this.tbNOME = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCNPJ_DE = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbFILIAL_PARA = new System.Windows.Forms.ComboBox();
            this.cbFILIAL_DE = new System.Windows.Forms.ComboBox();
            this.cbBASE_DE = new System.Windows.Forms.ComboBox();
            this.tbCNPJ_PARA = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(91, 217);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(10, 217);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 8;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // tbNOME
            // 
            this.tbNOME.Enabled = false;
            this.tbNOME.Location = new System.Drawing.Point(314, 39);
            this.tbNOME.Name = "tbNOME";
            this.tbNOME.Size = new System.Drawing.Size(176, 20);
            this.tbNOME.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(275, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 53;
            this.label7.Text = "Nome";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "Base origem";
            // 
            // tbID
            // 
            this.tbID.Enabled = false;
            this.tbID.Location = new System.Drawing.Point(314, 10);
            this.tbID.Name = "tbID";
            this.tbID.Size = new System.Drawing.Size(176, 20);
            this.tbID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(275, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 57;
            this.label2.Text = "Filial origem";
            // 
            // tbCNPJ_DE
            // 
            this.tbCNPJ_DE.Enabled = false;
            this.tbCNPJ_DE.Location = new System.Drawing.Point(314, 64);
            this.tbCNPJ_DE.Name = "tbCNPJ_DE";
            this.tbCNPJ_DE.Size = new System.Drawing.Size(176, 20);
            this.tbCNPJ_DE.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(275, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 59;
            this.label3.Text = "CNPJ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 61;
            this.label5.Text = "Filial consolidadora";
            // 
            // cbFILIAL_PARA
            // 
            this.cbFILIAL_PARA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFILIAL_PARA.Enabled = false;
            this.cbFILIAL_PARA.FormattingEnabled = true;
            this.cbFILIAL_PARA.Location = new System.Drawing.Point(103, 90);
            this.cbFILIAL_PARA.Name = "cbFILIAL_PARA";
            this.cbFILIAL_PARA.Size = new System.Drawing.Size(166, 21);
            this.cbFILIAL_PARA.TabIndex = 6;
            this.cbFILIAL_PARA.SelectionChangeCommitted += new System.EventHandler(this.cbFILIAL_PARA_SelectionChangeCommitted);
            // 
            // cbFILIAL_DE
            // 
            this.cbFILIAL_DE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFILIAL_DE.Enabled = false;
            this.cbFILIAL_DE.FormattingEnabled = true;
            this.cbFILIAL_DE.Location = new System.Drawing.Point(103, 64);
            this.cbFILIAL_DE.Name = "cbFILIAL_DE";
            this.cbFILIAL_DE.Size = new System.Drawing.Size(166, 21);
            this.cbFILIAL_DE.TabIndex = 4;
            this.cbFILIAL_DE.SelectionChangeCommitted += new System.EventHandler(this.cbFILIAL_DE_SelectionChangeCommitted);
            // 
            // cbBASE_DE
            // 
            this.cbBASE_DE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBASE_DE.FormattingEnabled = true;
            this.cbBASE_DE.Location = new System.Drawing.Point(103, 39);
            this.cbBASE_DE.Name = "cbBASE_DE";
            this.cbBASE_DE.Size = new System.Drawing.Size(166, 21);
            this.cbBASE_DE.TabIndex = 2;
            this.cbBASE_DE.SelectionChangeCommitted += new System.EventHandler(this.cbBASE_DE_SelectionChangeCommitted);
            // 
            // tbCNPJ_PARA
            // 
            this.tbCNPJ_PARA.Enabled = false;
            this.tbCNPJ_PARA.Location = new System.Drawing.Point(314, 90);
            this.tbCNPJ_PARA.Name = "tbCNPJ_PARA";
            this.tbCNPJ_PARA.Size = new System.Drawing.Size(176, 20);
            this.tbCNPJ_PARA.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(275, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 66;
            this.label6.Text = "CNPJ";
            // 
            // Conciliador_DeParaCadastrar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.LightGreen;
            this.Controls.Add(this.tbCNPJ_PARA);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbBASE_DE);
            this.Controls.Add(this.cbFILIAL_DE);
            this.Controls.Add(this.cbFILIAL_PARA);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbCNPJ_DE);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.tbNOME);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbID);
            this.Controls.Add(this.label1);
            this.Name = "Conciliador_DeParaCadastrar";
            this.Size = new System.Drawing.Size(501, 251);
            this.Load += new System.EventHandler(this.Conciliador_DeParaCadastrar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.TextBox tbNOME;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCNPJ_DE;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbFILIAL_PARA;
        private System.Windows.Forms.ComboBox cbFILIAL_DE;
        private System.Windows.Forms.ComboBox cbBASE_DE;
        private System.Windows.Forms.TextBox tbCNPJ_PARA;
        private System.Windows.Forms.Label label6;
    }
}
