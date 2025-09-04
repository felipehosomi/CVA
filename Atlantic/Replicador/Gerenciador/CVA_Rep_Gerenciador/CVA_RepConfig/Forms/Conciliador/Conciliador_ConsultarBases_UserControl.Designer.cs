namespace CVA_RepConfig.Forms.Conciliador
{
    partial class Conciliador_ConsultarBases_UserControl
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
            this.SERVER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USERNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PASSWD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BASE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.USE_TRUSTED = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DB_SERVER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DB_USERNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DB_PASSWD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DB_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TIPO = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.SERVER,
            this.USERNAME,
            this.PASSWD,
            this.BASE,
            this.USE_TRUSTED,
            this.DB_SERVER,
            this.DB_USERNAME,
            this.DB_PASSWD,
            this.DB_TYPE,
            this.TIPO});
            this.dgvBases.Location = new System.Drawing.Point(19, 20);
            this.dgvBases.Name = "dgvBases";
            this.dgvBases.ReadOnly = true;
            this.dgvBases.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBases.Size = new System.Drawing.Size(629, 201);
            this.dgvBases.TabIndex = 3;
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
            // SERVER
            // 
            this.SERVER.HeaderText = "License Server";
            this.SERVER.Name = "SERVER";
            this.SERVER.ReadOnly = true;
            this.SERVER.Width = 140;
            // 
            // USERNAME
            // 
            this.USERNAME.HeaderText = "Usuário";
            this.USERNAME.Name = "USERNAME";
            this.USERNAME.ReadOnly = true;
            // 
            // PASSWD
            // 
            this.PASSWD.HeaderText = "Senha";
            this.PASSWD.Name = "PASSWD";
            this.PASSWD.ReadOnly = true;
            // 
            // BASE
            // 
            this.BASE.HeaderText = "Banco de dados";
            this.BASE.Name = "BASE";
            this.BASE.ReadOnly = true;
            this.BASE.Width = 140;
            // 
            // USE_TRUSTED
            // 
            this.USE_TRUSTED.HeaderText = "Habilitar SSL?";
            this.USE_TRUSTED.Name = "USE_TRUSTED";
            this.USE_TRUSTED.ReadOnly = true;
            this.USE_TRUSTED.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.USE_TRUSTED.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.USE_TRUSTED.Width = 80;
            // 
            // DB_SERVER
            // 
            this.DB_SERVER.HeaderText = "Endereço do banco de dados";
            this.DB_SERVER.Name = "DB_SERVER";
            this.DB_SERVER.ReadOnly = true;
            this.DB_SERVER.Width = 240;
            // 
            // DB_USERNAME
            // 
            this.DB_USERNAME.HeaderText = "Usuário do banco";
            this.DB_USERNAME.Name = "DB_USERNAME";
            this.DB_USERNAME.ReadOnly = true;
            this.DB_USERNAME.Width = 120;
            // 
            // DB_PASSWD
            // 
            this.DB_PASSWD.HeaderText = "Senha do banco";
            this.DB_PASSWD.Name = "DB_PASSWD";
            this.DB_PASSWD.ReadOnly = true;
            this.DB_PASSWD.Width = 120;
            // 
            // DB_TYPE
            // 
            this.DB_TYPE.HeaderText = "Tipo de conexão";
            this.DB_TYPE.Name = "DB_TYPE";
            this.DB_TYPE.ReadOnly = true;
            this.DB_TYPE.Width = 120;
            // 
            // TIPO
            // 
            this.TIPO.HeaderText = "Tipo de empresa";
            this.TIPO.Name = "TIPO";
            this.TIPO.ReadOnly = true;
            // 
            // Conciliador_ConsultarBases_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.dgvBases);
            this.Name = "Conciliador_ConsultarBases_UserControl";
            this.Size = new System.Drawing.Size(667, 240);
            this.Load += new System.EventHandler(this.Conciliador_ConsultarBases_UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBases)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBases;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SERVER;
        private System.Windows.Forms.DataGridViewTextBoxColumn USERNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn PASSWD;
        private System.Windows.Forms.DataGridViewTextBoxColumn BASE;
        private System.Windows.Forms.DataGridViewTextBoxColumn USE_TRUSTED;
        private System.Windows.Forms.DataGridViewTextBoxColumn DB_SERVER;
        private System.Windows.Forms.DataGridViewTextBoxColumn DB_USERNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn DB_PASSWD;
        private System.Windows.Forms.DataGridViewTextBoxColumn DB_TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn TIPO;
    }
}
