namespace Conferencia
{
    partial class FrmListaConf1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmListaConf1));
            this.GridLista1Conf = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tx_Pedido = new System.Windows.Forms.TextBox();
            this.tx_User = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GridLista1Conf)).BeginInit();
            this.SuspendLayout();
            // 
            // GridLista1Conf
            // 
            this.GridLista1Conf.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.GridLista1Conf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridLista1Conf.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.GridLista1Conf.Location = new System.Drawing.Point(12, 80);
            this.GridLista1Conf.Name = "GridLista1Conf";
            this.GridLista1Conf.ReadOnly = true;
            this.GridLista1Conf.RowHeadersVisible = false;
            this.GridLista1Conf.Size = new System.Drawing.Size(1338, 593);
            this.GridLista1Conf.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nº Pedido:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // tx_Pedido
            // 
            this.tx_Pedido.Location = new System.Drawing.Point(103, 40);
            this.tx_Pedido.Name = "tx_Pedido";
            this.tx_Pedido.Size = new System.Drawing.Size(140, 20);
            this.tx_Pedido.TabIndex = 2;
            this.tx_Pedido.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // tx_User
            // 
            this.tx_User.AutoSize = true;
            this.tx_User.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_User.Location = new System.Drawing.Point(12, 676);
            this.tx_User.Name = "tx_User";
            this.tx_User.Size = new System.Drawing.Size(0, 17);
            this.tx_User.TabIndex = 3;
            // 
            // FrmListaConf1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1354, 702);
            this.Controls.Add(this.tx_User);
            this.Controls.Add(this.tx_Pedido);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GridLista1Conf);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmListaConf1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lista 1º Conferência";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmLista1Conferencia_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridLista1Conf)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView GridLista1Conf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tx_Pedido;
        private System.Windows.Forms.Label tx_User;
    }
}