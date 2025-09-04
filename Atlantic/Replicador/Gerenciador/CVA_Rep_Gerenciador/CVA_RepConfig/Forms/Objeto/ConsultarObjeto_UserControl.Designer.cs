namespace CVA_RepConfig.Forms.Objeto
{
	partial class ConsultarObjeto_UserControl
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
            this.OBJ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DSCR = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.OBJ,
            this.DSCR});
            this.dgvBases.Location = new System.Drawing.Point(19, 20);
            this.dgvBases.Name = "dgvBases";
            this.dgvBases.ReadOnly = true;
            this.dgvBases.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBases.Size = new System.Drawing.Size(628, 201);
            this.dgvBases.TabIndex = 3;
            // 
            // ID
            // 
            this.ID.Frozen = true;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // INS
            // 
            this.INS.HeaderText = "Data da inclusão";
            this.INS.Name = "INS";
            this.INS.ReadOnly = true;
            this.INS.Width = 140;
            // 
            // UPD
            // 
            this.UPD.HeaderText = "Data da alteração";
            this.UPD.Name = "UPD";
            this.UPD.ReadOnly = true;
            this.UPD.Width = 140;
            // 
            // STU
            // 
            this.STU.HeaderText = "Status";
            this.STU.Name = "STU";
            this.STU.ReadOnly = true;
            this.STU.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.STU.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // OBJ
            // 
            this.OBJ.HeaderText = "Nome do objeto";
            this.OBJ.Name = "OBJ";
            this.OBJ.ReadOnly = true;
            this.OBJ.Width = 140;
            // 
            // DSCR
            // 
            this.DSCR.HeaderText = "Descrição";
            this.DSCR.Name = "DSCR";
            this.DSCR.ReadOnly = true;
            this.DSCR.Width = 200;
            // 
            // ConsultarObjeto_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvBases);
            this.Name = "ConsultarObjeto_UserControl";
            this.Size = new System.Drawing.Size(667, 240);
            this.Load += new System.EventHandler(this.ConsultarObjeto_UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBases)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvBases;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn INS;
        private System.Windows.Forms.DataGridViewTextBoxColumn UPD;
        private System.Windows.Forms.DataGridViewTextBoxColumn STU;
        private System.Windows.Forms.DataGridViewTextBoxColumn OBJ;
        private System.Windows.Forms.DataGridViewTextBoxColumn DSCR;
    }
}
