namespace CVA_RepConfig.Forms.Timer
{
    partial class Customizado_UserControl
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
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.cl_Dia = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cl_Horario = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.bt_Adicionar = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(26, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Dia:";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(19, 31);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 20);
			this.textBox1.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(167, 14);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Horário";
			// 
			// dateTimePicker1
			// 
			this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.dateTimePicker1.Location = new System.Drawing.Point(170, 30);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.ShowUpDown = true;
			this.dateTimePicker1.Size = new System.Drawing.Size(124, 20);
			this.dateTimePicker1.TabIndex = 3;
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cl_Dia,
            this.cl_Horario});
			this.dataGridView1.Location = new System.Drawing.Point(19, 58);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(589, 66);
			this.dataGridView1.TabIndex = 4;
			// 
			// cl_Dia
			// 
			this.cl_Dia.HeaderText = "Dia";
			this.cl_Dia.Name = "cl_Dia";
			this.cl_Dia.ReadOnly = true;
			// 
			// cl_Horario
			// 
			this.cl_Horario.HeaderText = "Horário";
			this.cl_Horario.Name = "cl_Horario";
			this.cl_Horario.ReadOnly = true;
			// 
			// bt_Adicionar
			// 
			this.bt_Adicionar.Location = new System.Drawing.Point(330, 27);
			this.bt_Adicionar.Name = "bt_Adicionar";
			this.bt_Adicionar.Size = new System.Drawing.Size(75, 23);
			this.bt_Adicionar.TabIndex = 5;
			this.bt_Adicionar.Text = "Adicionar";
			this.bt_Adicionar.UseVisualStyleBackColor = true;
			// 
			// Customizado_UserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.bt_Adicionar);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.dateTimePicker1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.Name = "Customizado_UserControl";
			this.Size = new System.Drawing.Size(629, 143);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button bt_Adicionar;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl_Dia;
        private System.Windows.Forms.DataGridViewTextBoxColumn cl_Horario;
    }
}
