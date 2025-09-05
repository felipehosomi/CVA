namespace CVA.View.HanaConverter
{
    partial class Form1
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
            this.tbxSQL = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.tbxHana = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Comando SQL";
            // 
            // tbxSQL
            // 
            this.tbxSQL.Location = new System.Drawing.Point(15, 30);
            this.tbxSQL.Multiline = true;
            this.tbxSQL.Name = "tbxSQL";
            this.tbxSQL.Size = new System.Drawing.Size(773, 140);
            this.tbxSQL.TabIndex = 1;
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(15, 177);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(112, 29);
            this.btnConvert.TabIndex = 2;
            this.btnConvert.Text = "Converter";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // tbxHana
            // 
            this.tbxHana.Location = new System.Drawing.Point(12, 212);
            this.tbxHana.Multiline = true;
            this.tbxHana.Name = "tbxHana";
            this.tbxHana.Size = new System.Drawing.Size(773, 214);
            this.tbxHana.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tbxHana);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.tbxSQL);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Conversor SQL - Hana";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxSQL;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.TextBox tbxHana;
    }
}

