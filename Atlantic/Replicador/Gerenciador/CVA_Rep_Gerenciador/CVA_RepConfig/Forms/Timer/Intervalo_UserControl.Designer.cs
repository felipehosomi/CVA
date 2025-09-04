namespace CVA_RepConfig.Forms.Timer
{
    partial class Intervalo_UserControl
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
			this.label1 = new System.Windows.Forms.Label();
			this.tbIntervalo = new System.Windows.Forms.TextBox();
			this.tbId = new System.Windows.Forms.TextBox();
			this.tbQuantidade = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(23, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(146, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Definir minutos para intervalo:";
			// 
			// tbIntervalo
			// 
			this.tbIntervalo.Location = new System.Drawing.Point(175, 11);
			this.tbIntervalo.Name = "tbIntervalo";
			this.tbIntervalo.Size = new System.Drawing.Size(100, 20);
			this.tbIntervalo.TabIndex = 1;
			// 
			// tbId
			// 
			this.tbId.Location = new System.Drawing.Point(175, 63);
			this.tbId.Name = "tbId";
			this.tbId.Size = new System.Drawing.Size(100, 20);
			this.tbId.TabIndex = 2;
			this.tbId.Visible = false;
			// 
			// tbQuantidade
			// 
			this.tbQuantidade.Location = new System.Drawing.Point(175, 37);
			this.tbQuantidade.Name = "tbQuantidade";
			this.tbQuantidade.Size = new System.Drawing.Size(100, 20);
			this.tbQuantidade.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(23, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(122, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Quantidade de registros:";
			// 
			// Intervalo_UserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.tbQuantidade);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbId);
			this.Controls.Add(this.tbIntervalo);
			this.Controls.Add(this.label1);
			this.Name = "Intervalo_UserControl";
			this.Size = new System.Drawing.Size(629, 143);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox tbIntervalo;
		public System.Windows.Forms.TextBox tbId;
		public System.Windows.Forms.TextBox tbQuantidade;
		private System.Windows.Forms.Label label2;
	}
}
