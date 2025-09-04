namespace CVA_RepConfig.Forms.Timer
{
    partial class Diario_UserControl
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
			this.dp_Diario = new System.Windows.Forms.DateTimePicker();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(170, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Efetuar processamento sempre às:";
			// 
			// dp_Diario
			// 
			this.dp_Diario.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.dp_Diario.Location = new System.Drawing.Point(19, 30);
			this.dp_Diario.Name = "dp_Diario";
			this.dp_Diario.ShowUpDown = true;
			this.dp_Diario.Size = new System.Drawing.Size(167, 20);
			this.dp_Diario.TabIndex = 1;
			// 
			// Diario_UserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.dp_Diario);
			this.Controls.Add(this.label1);
			this.Name = "Diario_UserControl";
			this.Size = new System.Drawing.Size(629, 143);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dp_Diario;
    }
}
