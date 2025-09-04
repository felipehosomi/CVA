namespace CVA_RepConfig.Forms.Conciliador
{
    partial class Conciliador_BasesForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bt_Refresh = new System.Windows.Forms.Button();
            this.bt_Remover = new System.Windows.Forms.Button();
            this.bt_Editar = new System.Windows.Forms.Button();
            this.bt_Adicionar = new System.Windows.Forms.Button();
            this.pn_Opcoes = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.bt_Refresh);
            this.groupBox1.Controls.Add(this.bt_Remover);
            this.groupBox1.Controls.Add(this.bt_Editar);
            this.groupBox1.Controls.Add(this.bt_Adicionar);
            this.groupBox1.Controls.Add(this.pn_Opcoes);
            this.groupBox1.Location = new System.Drawing.Point(15, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(679, 321);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuração de Bases";
            // 
            // bt_Refresh
            // 
            this.bt_Refresh.Location = new System.Drawing.Point(583, 19);
            this.bt_Refresh.Name = "bt_Refresh";
            this.bt_Refresh.Size = new System.Drawing.Size(90, 23);
            this.bt_Refresh.TabIndex = 7;
            this.bt_Refresh.Text = "Atualizar lista";
            this.bt_Refresh.UseVisualStyleBackColor = true;
            this.bt_Refresh.Click += new System.EventHandler(this.bt_Refresh_Click);
            // 
            // bt_Remover
            // 
            this.bt_Remover.BackColor = System.Drawing.Color.Salmon;
            this.bt_Remover.Location = new System.Drawing.Point(168, 19);
            this.bt_Remover.Name = "bt_Remover";
            this.bt_Remover.Size = new System.Drawing.Size(75, 23);
            this.bt_Remover.TabIndex = 6;
            this.bt_Remover.Text = "Remover";
            this.bt_Remover.UseVisualStyleBackColor = false;
            this.bt_Remover.Click += new System.EventHandler(this.bt_Remover_Click);
            // 
            // bt_Editar
            // 
            this.bt_Editar.BackColor = System.Drawing.Color.Yellow;
            this.bt_Editar.Location = new System.Drawing.Point(87, 19);
            this.bt_Editar.Name = "bt_Editar";
            this.bt_Editar.Size = new System.Drawing.Size(75, 23);
            this.bt_Editar.TabIndex = 5;
            this.bt_Editar.Text = "Editar";
            this.bt_Editar.UseVisualStyleBackColor = false;
            this.bt_Editar.Click += new System.EventHandler(this.bt_Editar_Click);
            // 
            // bt_Adicionar
            // 
            this.bt_Adicionar.BackColor = System.Drawing.Color.LightGreen;
            this.bt_Adicionar.Location = new System.Drawing.Point(6, 19);
            this.bt_Adicionar.Name = "bt_Adicionar";
            this.bt_Adicionar.Size = new System.Drawing.Size(75, 23);
            this.bt_Adicionar.TabIndex = 4;
            this.bt_Adicionar.Text = "Adicionar";
            this.bt_Adicionar.UseVisualStyleBackColor = false;
            this.bt_Adicionar.Click += new System.EventHandler(this.bt_Adicionar_Click);
            // 
            // pn_Opcoes
            // 
            this.pn_Opcoes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pn_Opcoes.AutoSize = true;
            this.pn_Opcoes.Location = new System.Drawing.Point(6, 62);
            this.pn_Opcoes.Name = "pn_Opcoes";
            this.pn_Opcoes.Size = new System.Drawing.Size(667, 251);
            this.pn_Opcoes.TabIndex = 0;
            // 
            // Conciliador_BasesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.groupBox1);
            this.Name = "Conciliador_BasesForm";
            this.Size = new System.Drawing.Size(709, 337);
            this.Load += new System.EventHandler(this.Conciliador_BasesForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button bt_Refresh;
        private System.Windows.Forms.Button bt_Remover;
        private System.Windows.Forms.Button bt_Editar;
        private System.Windows.Forms.Button bt_Adicionar;
        private System.Windows.Forms.Panel pn_Opcoes;
    }
}
