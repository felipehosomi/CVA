namespace CVA.View.BancadaTeste
{
    partial class frmBancadaTeste
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbxPath = new System.Windows.Forms.TextBox();
            this.tbxSerie = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPath = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblOK = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.ofdPath = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Arquivo";
            // 
            // tbxPath
            // 
            this.tbxPath.Location = new System.Drawing.Point(64, 8);
            this.tbxPath.Name = "tbxPath";
            this.tbxPath.Size = new System.Drawing.Size(279, 20);
            this.tbxPath.TabIndex = 1;
            this.tbxPath.Leave += new System.EventHandler(this.tbxPath_Leave);
            // 
            // tbxSerie
            // 
            this.tbxSerie.Location = new System.Drawing.Point(64, 34);
            this.tbxSerie.Name = "tbxSerie";
            this.tbxSerie.Size = new System.Drawing.Size(279, 20);
            this.tbxSerie.TabIndex = 3;
            this.tbxSerie.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxSerie_KeyDown);
            this.tbxSerie.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.tbxSerie_PreviewKeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Série";
            // 
            // btnPath
            // 
            this.btnPath.Location = new System.Drawing.Point(349, 3);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(27, 23);
            this.btnPath.TabIndex = 4;
            this.btnPath.Text = "...";
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(349, 32);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(71, 23);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "Gerar";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblOK
            // 
            this.lblOK.AutoSize = true;
            this.lblOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOK.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblOK.Location = new System.Drawing.Point(12, 63);
            this.lblOK.Name = "lblOK";
            this.lblOK.Size = new System.Drawing.Size(166, 16);
            this.lblOK.TabIndex = 6;
            this.lblOK.Text = "Bipar Número de Série";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(12, 87);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 16);
            this.lblError.TabIndex = 7;
            // 
            // frmBancadaTeste
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 273);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblOK);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnPath);
            this.Controls.Add(this.tbxSerie);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxPath);
            this.Controls.Add(this.label1);
            this.Name = "frmBancadaTeste";
            this.Text = "Arquivo Bancada Teste";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxPath;
        private System.Windows.Forms.TextBox tbxSerie;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblOK;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.OpenFileDialog ofdPath;
    }
}

