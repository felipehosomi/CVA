namespace CVA_RepConfig.Forms.AcessoCVA
{
    partial class AcessoCVA_Form
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
            this.dgCVA_TIM = new System.Windows.Forms.DataGridView();
            this.ID_TIM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INS_TIM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UPD_TIM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STU_TIM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TIM_TIM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NUM_OBJ_TIM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgCVA_REG = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bCVA_TIM = new System.Windows.Forms.Button();
            this.bCVA_REG = new System.Windows.Forms.Button();
            this.tSenha = new System.Windows.Forms.TextBox();
            this.bInput = new System.Windows.Forms.Button();
            this.tEmail = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ID_REG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.INS_REG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UPD_REG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STU_REG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BAS_REG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CODE_REG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OBJ_REG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FUNC_REG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BAS_ERR_REG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Changed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgCVA_TIM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgCVA_REG)).BeginInit();
            this.SuspendLayout();
            // 
            // dgCVA_TIM
            // 
            this.dgCVA_TIM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCVA_TIM.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID_TIM,
            this.INS_TIM,
            this.UPD_TIM,
            this.STU_TIM,
            this.TIM_TIM,
            this.NUM_OBJ_TIM});
            this.dgCVA_TIM.Location = new System.Drawing.Point(11, 54);
            this.dgCVA_TIM.Name = "dgCVA_TIM";
            this.dgCVA_TIM.Size = new System.Drawing.Size(677, 77);
            this.dgCVA_TIM.TabIndex = 0;
            // 
            // ID_TIM
            // 
            this.ID_TIM.HeaderText = "ID";
            this.ID_TIM.Name = "ID_TIM";
            this.ID_TIM.ReadOnly = true;
            this.ID_TIM.Width = 30;
            // 
            // INS_TIM
            // 
            this.INS_TIM.HeaderText = "Inserção";
            this.INS_TIM.Name = "INS_TIM";
            this.INS_TIM.ReadOnly = true;
            this.INS_TIM.Width = 130;
            // 
            // UPD_TIM
            // 
            this.UPD_TIM.HeaderText = "Última Atualização";
            this.UPD_TIM.Name = "UPD_TIM";
            this.UPD_TIM.ReadOnly = true;
            this.UPD_TIM.Width = 130;
            // 
            // STU_TIM
            // 
            this.STU_TIM.HeaderText = "Status";
            this.STU_TIM.Name = "STU_TIM";
            // 
            // TIM_TIM
            // 
            this.TIM_TIM.HeaderText = "Tempo";
            this.TIM_TIM.Name = "TIM_TIM";
            // 
            // NUM_OBJ_TIM
            // 
            this.NUM_OBJ_TIM.HeaderText = "Qtd. Objetos";
            this.NUM_OBJ_TIM.Name = "NUM_OBJ_TIM";
            // 
            // dgCVA_REG
            // 
            this.dgCVA_REG.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCVA_REG.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID_REG,
            this.INS_REG,
            this.UPD_REG,
            this.STU_REG,
            this.BAS_REG,
            this.CODE_REG,
            this.OBJ_REG,
            this.FUNC_REG,
            this.BAS_ERR_REG,
            this.Changed});
            this.dgCVA_REG.Location = new System.Drawing.Point(11, 179);
            this.dgCVA_REG.Name = "dgCVA_REG";
            this.dgCVA_REG.Size = new System.Drawing.Size(677, 150);
            this.dgCVA_REG.TabIndex = 1;
            this.dgCVA_REG.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgCVA_REG_CellEndEdit);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(8, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "CVA_TIM";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "CVA_REG";
            // 
            // bCVA_TIM
            // 
            this.bCVA_TIM.Location = new System.Drawing.Point(568, 28);
            this.bCVA_TIM.Name = "bCVA_TIM";
            this.bCVA_TIM.Size = new System.Drawing.Size(120, 23);
            this.bCVA_TIM.TabIndex = 4;
            this.bCVA_TIM.Text = "UPDATE CVA_TIM";
            this.bCVA_TIM.UseVisualStyleBackColor = true;
            this.bCVA_TIM.Click += new System.EventHandler(this.bCVA_TIM_Click);
            // 
            // bCVA_REG
            // 
            this.bCVA_REG.Location = new System.Drawing.Point(568, 153);
            this.bCVA_REG.Name = "bCVA_REG";
            this.bCVA_REG.Size = new System.Drawing.Size(120, 23);
            this.bCVA_REG.TabIndex = 5;
            this.bCVA_REG.Text = "UPDATE CVA_REG";
            this.bCVA_REG.UseVisualStyleBackColor = true;
            this.bCVA_REG.Click += new System.EventHandler(this.bCVA_REG_Click);
            // 
            // tSenha
            // 
            this.tSenha.Location = new System.Drawing.Point(257, 150);
            this.tSenha.Name = "tSenha";
            this.tSenha.Size = new System.Drawing.Size(208, 20);
            this.tSenha.TabIndex = 6;
            this.tSenha.UseSystemPasswordChar = true;
            // 
            // bInput
            // 
            this.bInput.Location = new System.Drawing.Point(320, 179);
            this.bInput.Name = "bInput";
            this.bInput.Size = new System.Drawing.Size(75, 20);
            this.bInput.TabIndex = 7;
            this.bInput.Text = "Acessar";
            this.bInput.UseVisualStyleBackColor = true;
            this.bInput.Click += new System.EventHandler(this.bInput_Click);
            // 
            // tEmail
            // 
            this.tEmail.Location = new System.Drawing.Point(257, 111);
            this.tEmail.Name = "tEmail";
            this.tEmail.Size = new System.Drawing.Size(208, 20);
            this.tEmail.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(213, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "E-mail:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(210, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Senha:";
            // 
            // ID_REG
            // 
            this.ID_REG.HeaderText = "ID";
            this.ID_REG.Name = "ID_REG";
            this.ID_REG.ReadOnly = true;
            this.ID_REG.Width = 30;
            // 
            // INS_REG
            // 
            this.INS_REG.HeaderText = "Inserção";
            this.INS_REG.Name = "INS_REG";
            this.INS_REG.ReadOnly = true;
            this.INS_REG.Width = 150;
            // 
            // UPD_REG
            // 
            this.UPD_REG.HeaderText = "Última Atualização";
            this.UPD_REG.Name = "UPD_REG";
            this.UPD_REG.ReadOnly = true;
            this.UPD_REG.Width = 150;
            // 
            // STU_REG
            // 
            this.STU_REG.HeaderText = "Status";
            this.STU_REG.Name = "STU_REG";
            this.STU_REG.Width = 30;
            // 
            // BAS_REG
            // 
            this.BAS_REG.HeaderText = "Base";
            this.BAS_REG.Name = "BAS_REG";
            this.BAS_REG.Width = 30;
            // 
            // CODE_REG
            // 
            this.CODE_REG.HeaderText = "Código";
            this.CODE_REG.Name = "CODE_REG";
            this.CODE_REG.Width = 50;
            // 
            // OBJ_REG
            // 
            this.OBJ_REG.HeaderText = "OBJ";
            this.OBJ_REG.Name = "OBJ_REG";
            this.OBJ_REG.Width = 30;
            // 
            // FUNC_REG
            // 
            this.FUNC_REG.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.FUNC_REG.HeaderText = "Função";
            this.FUNC_REG.Name = "FUNC_REG";
            this.FUNC_REG.Width = 66;
            // 
            // BAS_ERR_REG
            // 
            this.BAS_ERR_REG.FillWeight = 90F;
            this.BAS_ERR_REG.HeaderText = "Base com Erro";
            this.BAS_ERR_REG.Name = "BAS_ERR_REG";
            // 
            // Changed
            // 
            this.Changed.HeaderText = "Changed";
            this.Changed.Name = "Changed";
            this.Changed.Visible = false;
            // 
            // AcessoCVA_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tEmail);
            this.Controls.Add(this.bInput);
            this.Controls.Add(this.tSenha);
            this.Controls.Add(this.bCVA_REG);
            this.Controls.Add(this.bCVA_TIM);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgCVA_REG);
            this.Controls.Add(this.dgCVA_TIM);
            this.Name = "AcessoCVA_Form";
            this.Size = new System.Drawing.Size(695, 341);
            this.Load += new System.EventHandler(this.AcessoCVA_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgCVA_TIM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgCVA_REG)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgCVA_TIM;
        private System.Windows.Forms.DataGridView dgCVA_REG;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bCVA_TIM;
        private System.Windows.Forms.Button bCVA_REG;
        private System.Windows.Forms.TextBox tSenha;
        private System.Windows.Forms.Button bInput;
        private System.Windows.Forms.TextBox tEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID_TIM;
        private System.Windows.Forms.DataGridViewTextBoxColumn INS_TIM;
        private System.Windows.Forms.DataGridViewTextBoxColumn UPD_TIM;
        private System.Windows.Forms.DataGridViewTextBoxColumn STU_TIM;
        private System.Windows.Forms.DataGridViewTextBoxColumn TIM_TIM;
        private System.Windows.Forms.DataGridViewTextBoxColumn NUM_OBJ_TIM;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID_REG;
        private System.Windows.Forms.DataGridViewTextBoxColumn INS_REG;
        private System.Windows.Forms.DataGridViewTextBoxColumn UPD_REG;
        private System.Windows.Forms.DataGridViewTextBoxColumn STU_REG;
        private System.Windows.Forms.DataGridViewTextBoxColumn BAS_REG;
        private System.Windows.Forms.DataGridViewTextBoxColumn CODE_REG;
        private System.Windows.Forms.DataGridViewTextBoxColumn OBJ_REG;
        private System.Windows.Forms.DataGridViewTextBoxColumn FUNC_REG;
        private System.Windows.Forms.DataGridViewTextBoxColumn BAS_ERR_REG;
        private System.Windows.Forms.DataGridViewTextBoxColumn Changed;
    }
}
