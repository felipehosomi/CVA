namespace CVA_RepConfig.Forms.Conciliador
{
    partial class Conciliador_DeParaConsultar
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
            this.dgvBases = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BASE_DE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FILIAL_DE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NOME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CNPJ_FILIAL_DE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CNPJ_FILIAL_PARA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FILIAL_PARA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBases)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBases
            // 
            this.dgvBases.AllowUserToAddRows = false;
            this.dgvBases.AllowUserToDeleteRows = false;
            this.dgvBases.AllowUserToOrderColumns = true;
            this.dgvBases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBases.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.BASE_DE,
            this.FILIAL_DE,
            this.NOME,
            this.CNPJ_FILIAL_DE,
            this.CNPJ_FILIAL_PARA,
            this.FILIAL_PARA});
            this.dgvBases.Location = new System.Drawing.Point(19, 20);
            this.dgvBases.Name = "dgvBases";
            this.dgvBases.ReadOnly = true;
            this.dgvBases.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBases.Size = new System.Drawing.Size(629, 201);
            this.dgvBases.TabIndex = 4;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 40;
            // 
            // BASE_DE
            // 
            this.BASE_DE.HeaderText = "Base De";
            this.BASE_DE.Name = "BASE_DE";
            this.BASE_DE.ReadOnly = true;
            this.BASE_DE.Width = 140;
            // 
            // FILIAL_DE
            // 
            this.FILIAL_DE.HeaderText = "Filial";
            this.FILIAL_DE.Name = "FILIAL_DE";
            this.FILIAL_DE.ReadOnly = true;
            this.FILIAL_DE.Width = 50;
            // 
            // NOME
            // 
            this.NOME.HeaderText = "Nome";
            this.NOME.Name = "NOME";
            this.NOME.ReadOnly = true;
            this.NOME.Width = 190;
            // 
            // CNPJ_FILIAL_DE
            // 
            this.CNPJ_FILIAL_DE.HeaderText = "CNPJ Filial De";
            this.CNPJ_FILIAL_DE.Name = "CNPJ_FILIAL_DE";
            this.CNPJ_FILIAL_DE.ReadOnly = true;
            // 
            // CNPJ_FILIAL_PARA
            // 
            this.CNPJ_FILIAL_PARA.HeaderText = "CNPJ Filial Para";
            this.CNPJ_FILIAL_PARA.Name = "CNPJ_FILIAL_PARA";
            this.CNPJ_FILIAL_PARA.ReadOnly = true;
            this.CNPJ_FILIAL_PARA.Width = 120;
            // 
            // FILIAL_PARA
            // 
            this.FILIAL_PARA.HeaderText = "Filial Para";
            this.FILIAL_PARA.Name = "FILIAL_PARA";
            this.FILIAL_PARA.ReadOnly = true;
            this.FILIAL_PARA.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FILIAL_PARA.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FILIAL_PARA.Width = 60;
            // 
            // Conciliador_DeParaConsultar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.dgvBases);
            this.Name = "Conciliador_DeParaConsultar";
            this.Size = new System.Drawing.Size(667, 240);
            this.Load += new System.EventHandler(this.Conciliador_DeParaConsultar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBases)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBases;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn BASE_DE;
        private System.Windows.Forms.DataGridViewTextBoxColumn FILIAL_DE;
        private System.Windows.Forms.DataGridViewTextBoxColumn NOME;
        private System.Windows.Forms.DataGridViewTextBoxColumn CNPJ_FILIAL_DE;
        private System.Windows.Forms.DataGridViewTextBoxColumn CNPJ_FILIAL_PARA;
        private System.Windows.Forms.DataGridViewTextBoxColumn FILIAL_PARA;
    }
}
