namespace CVA_RepConfig.Forms.Emails
{
    partial class Emails_Form
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
            this.components = new System.ComponentModel.Container();
            this.bRemover = new System.Windows.Forms.Button();
            this.bSalvar = new System.Windows.Forms.Button();
            this.bCancelar = new System.Windows.Forms.Button();
            this.dgEmails = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NOME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EMAIL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tNome = new System.Windows.Forms.TextBox();
            this.tEmail = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgEmails)).BeginInit();
            this.SuspendLayout();
            // 
            // bRemover
            // 
            this.bRemover.Location = new System.Drawing.Point(617, 34);
            this.bRemover.Name = "bRemover";
            this.bRemover.Size = new System.Drawing.Size(75, 23);
            this.bRemover.TabIndex = 1;
            this.bRemover.Text = "Remover";
            this.bRemover.UseVisualStyleBackColor = true;
            this.bRemover.Click += new System.EventHandler(this.bRemover_Click);
            // 
            // bSalvar
            // 
            this.bSalvar.Location = new System.Drawing.Point(536, 34);
            this.bSalvar.Name = "bSalvar";
            this.bSalvar.Size = new System.Drawing.Size(75, 23);
            this.bSalvar.TabIndex = 2;
            this.bSalvar.Text = "Salvar";
            this.bSalvar.UseVisualStyleBackColor = true;
            this.bSalvar.Click += new System.EventHandler(this.bSalvar_Click);
            // 
            // bCancelar
            // 
            this.bCancelar.Location = new System.Drawing.Point(617, 315);
            this.bCancelar.Name = "bCancelar";
            this.bCancelar.Size = new System.Drawing.Size(75, 23);
            this.bCancelar.TabIndex = 3;
            this.bCancelar.Text = "Cancelar";
            this.bCancelar.UseVisualStyleBackColor = true;
            this.bCancelar.Click += new System.EventHandler(this.bCancelar_Click);
            // 
            // dgEmails
            // 
            this.dgEmails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEmails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.NOME,
            this.EMAIL});
            this.dgEmails.Location = new System.Drawing.Point(3, 63);
            this.dgEmails.Name = "dgEmails";
            this.dgEmails.Size = new System.Drawing.Size(689, 240);
            this.dgEmails.TabIndex = 4;
            this.dgEmails.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgEmails_Click);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ID.Width = 30;
            // 
            // NOME
            // 
            this.NOME.HeaderText = "Nome";
            this.NOME.Name = "NOME";
            this.NOME.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.NOME.Width = 315;
            // 
            // EMAIL
            // 
            this.EMAIL.HeaderText = "Email";
            this.EMAIL.Name = "EMAIL";
            this.EMAIL.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.EMAIL.Width = 300;
            // 
            // tNome
            // 
            this.tNome.Location = new System.Drawing.Point(54, 4);
            this.tNome.Name = "tNome";
            this.tNome.Size = new System.Drawing.Size(302, 20);
            this.tNome.TabIndex = 5;
            this.tNome.TextChanged += new System.EventHandler(this.tNome_TextChanged);
            // 
            // tEmail
            // 
            this.tEmail.Location = new System.Drawing.Point(399, 3);
            this.tEmail.Name = "tEmail";
            this.tEmail.Size = new System.Drawing.Size(293, 20);
            this.tEmail.TabIndex = 6;
            this.tEmail.TextChanged += new System.EventHandler(this.tEmail_TextChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Nome:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(362, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Email:";
            // 
            // Emails_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tEmail);
            this.Controls.Add(this.tNome);
            this.Controls.Add(this.dgEmails);
            this.Controls.Add(this.bCancelar);
            this.Controls.Add(this.bSalvar);
            this.Controls.Add(this.bRemover);
            this.Name = "Emails_Form";
            this.Size = new System.Drawing.Size(695, 341);
            ((System.ComponentModel.ISupportInitialize)(this.dgEmails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bRemover;
        private System.Windows.Forms.Button bSalvar;
        private System.Windows.Forms.Button bCancelar;
        private System.Windows.Forms.DataGridView dgEmails;
        private System.Windows.Forms.TextBox tNome;
        private System.Windows.Forms.TextBox tEmail;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NOME;
        private System.Windows.Forms.DataGridViewTextBoxColumn EMAIL;
    }
}
