namespace CVA_RepConfig.Forms.Base
{
    partial class ConsultarBase_UserControl
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
            this.INS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UPD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SRVR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PAS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.COMP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USE_TRU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DB_SRVR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DB_UNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DB_PAS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DB_TYP = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.INS,
            this.UPD,
            this.STU,
            this.SRVR,
            this.UNAME,
            this.PAS,
            this.COMP,
            this.USE_TRU,
            this.DB_SRVR,
            this.DB_UNAME,
            this.DB_PAS,
            this.DB_TYP});
            this.dgvBases.Location = new System.Drawing.Point(21, 15);
            this.dgvBases.Name = "dgvBases";
            this.dgvBases.ReadOnly = true;
            this.dgvBases.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBases.Size = new System.Drawing.Size(629, 201);
            this.dgvBases.TabIndex = 2;
            this.dgvBases.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvBases_CellFormatting);
            this.dgvBases.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvBases_EditingControlShowing);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 40;
            // 
            // INS
            // 
            this.INS.HeaderText = "Data de cadastro";
            this.INS.Name = "INS";
            this.INS.ReadOnly = true;
            this.INS.Width = 120;
            // 
            // UPD
            // 
            this.UPD.HeaderText = "Data de atualização";
            this.UPD.Name = "UPD";
            this.UPD.ReadOnly = true;
            this.UPD.Width = 130;
            // 
            // STU
            // 
            this.STU.HeaderText = "Status";
            this.STU.Name = "STU";
            this.STU.ReadOnly = true;
            this.STU.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.STU.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SRVR
            // 
            this.SRVR.HeaderText = "License Server";
            this.SRVR.Name = "SRVR";
            this.SRVR.ReadOnly = true;
            this.SRVR.Width = 140;
            // 
            // UNAME
            // 
            this.UNAME.HeaderText = "Usuário";
            this.UNAME.Name = "UNAME";
            this.UNAME.ReadOnly = true;
            // 
            // PAS
            // 
            this.PAS.HeaderText = "Senha";
            this.PAS.Name = "PAS";
            this.PAS.ReadOnly = true;
            // 
            // COMP
            // 
            this.COMP.HeaderText = "Empresa";
            this.COMP.Name = "COMP";
            this.COMP.ReadOnly = true;
            // 
            // USE_TRU
            // 
            this.USE_TRU.HeaderText = "Habilitar SSL?";
            this.USE_TRU.Name = "USE_TRU";
            this.USE_TRU.ReadOnly = true;
            this.USE_TRU.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.USE_TRU.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.USE_TRU.Width = 80;
            // 
            // DB_SRVR
            // 
            this.DB_SRVR.HeaderText = "Base de dados";
            this.DB_SRVR.Name = "DB_SRVR";
            this.DB_SRVR.ReadOnly = true;
            this.DB_SRVR.Width = 140;
            // 
            // DB_UNAME
            // 
            this.DB_UNAME.HeaderText = "Usuário do banco";
            this.DB_UNAME.Name = "DB_UNAME";
            this.DB_UNAME.ReadOnly = true;
            this.DB_UNAME.Width = 120;
            // 
            // DB_PAS
            // 
            this.DB_PAS.HeaderText = "Senha do banco";
            this.DB_PAS.Name = "DB_PAS";
            this.DB_PAS.ReadOnly = true;
            this.DB_PAS.Width = 120;
            // 
            // DB_TYP
            // 
            this.DB_TYP.HeaderText = "Tipo de conexão";
            this.DB_TYP.Name = "DB_TYP";
            this.DB_TYP.ReadOnly = true;
            this.DB_TYP.Width = 120;
            // 
            // ConsultarBase_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.dgvBases);
            this.Name = "ConsultarBase_UserControl";
            this.Size = new System.Drawing.Size(667, 240);
            this.Load += new System.EventHandler(this.ConsultarBase_UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBases)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvBases;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn INS;
        private System.Windows.Forms.DataGridViewTextBoxColumn UPD;
        private System.Windows.Forms.DataGridViewTextBoxColumn STU;
        private System.Windows.Forms.DataGridViewTextBoxColumn SRVR;
        private System.Windows.Forms.DataGridViewTextBoxColumn UNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn PAS;
        private System.Windows.Forms.DataGridViewTextBoxColumn COMP;
        private System.Windows.Forms.DataGridViewTextBoxColumn USE_TRU;
        private System.Windows.Forms.DataGridViewTextBoxColumn DB_SRVR;
        private System.Windows.Forms.DataGridViewTextBoxColumn DB_UNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn DB_PAS;
        private System.Windows.Forms.DataGridViewTextBoxColumn DB_TYP;
    }
}
