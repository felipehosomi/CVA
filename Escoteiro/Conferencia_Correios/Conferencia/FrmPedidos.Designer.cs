namespace Conferencia
{
    partial class FrmPedidos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPedidos));
            this.label1 = new System.Windows.Forms.Label();
            this.DataDe = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_Status = new System.Windows.Forms.ComboBox();
            this.cb_filial = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_TipoEnvio = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_UfCliente = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Lbl_Usuario = new System.Windows.Forms.Label();
            this.btn_UnCheck_Liberados = new System.Windows.Forms.Button();
            this.btn_Check_Liberados = new System.Windows.Forms.Button();
            this.btn_confirmar_Liberados = new System.Windows.Forms.Button();
            this.btn_Pesquisa = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.GridLiberados = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_UnCheck_Pendentes = new System.Windows.Forms.Button();
            this.btn_check_Pendentes = new System.Windows.Forms.Button();
            this.btn_Confirmar_Pendentes = new System.Windows.Forms.Button();
            this.GridPendentes = new System.Windows.Forms.DataGridView();
            this.DataAte = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridLiberados)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridPendentes)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(40, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data Pedido";
            // 
            // DataDe
            // 
            this.DataDe.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DataDe.Location = new System.Drawing.Point(43, 65);
            this.DataDe.Name = "DataDe";
            this.DataDe.Size = new System.Drawing.Size(100, 20);
            this.DataDe.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(299, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Origem";
            // 
            // cb_Status
            // 
            this.cb_Status.FormattingEnabled = true;
            this.cb_Status.Location = new System.Drawing.Point(302, 64);
            this.cb_Status.Name = "cb_Status";
            this.cb_Status.Size = new System.Drawing.Size(274, 21);
            this.cb_Status.TabIndex = 3;
            // 
            // cb_filial
            // 
            this.cb_filial.FormattingEnabled = true;
            this.cb_filial.Location = new System.Drawing.Point(608, 64);
            this.cb_filial.Name = "cb_filial";
            this.cb_filial.Size = new System.Drawing.Size(355, 21);
            this.cb_filial.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(605, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Filial";
            // 
            // cb_TipoEnvio
            // 
            this.cb_TipoEnvio.FormattingEnabled = true;
            this.cb_TipoEnvio.Location = new System.Drawing.Point(302, 104);
            this.cb_TipoEnvio.Name = "cb_TipoEnvio";
            this.cb_TipoEnvio.Size = new System.Drawing.Size(274, 21);
            this.cb_TipoEnvio.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(296, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Tipo de Envio";
            // 
            // cb_UfCliente
            // 
            this.cb_UfCliente.FormattingEnabled = true;
            this.cb_UfCliente.Location = new System.Drawing.Point(608, 104);
            this.cb_UfCliente.Name = "cb_UfCliente";
            this.cb_UfCliente.Size = new System.Drawing.Size(355, 21);
            this.cb_UfCliente.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(605, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "UF do Cliente";
            // 
            // Lbl_Usuario
            // 
            this.Lbl_Usuario.AutoSize = true;
            this.Lbl_Usuario.Location = new System.Drawing.Point(12, 598);
            this.Lbl_Usuario.Name = "Lbl_Usuario";
            this.Lbl_Usuario.Size = new System.Drawing.Size(0, 13);
            this.Lbl_Usuario.TabIndex = 12;
            // 
            // btn_UnCheck_Liberados
            // 
            this.btn_UnCheck_Liberados.FlatAppearance.BorderSize = 0;
            this.btn_UnCheck_Liberados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_UnCheck_Liberados.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_UnCheck_Liberados.Image = global::Conferencia.Properties.Resources.SemCheck;
            this.btn_UnCheck_Liberados.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_UnCheck_Liberados.Location = new System.Drawing.Point(1045, 62);
            this.btn_UnCheck_Liberados.Name = "btn_UnCheck_Liberados";
            this.btn_UnCheck_Liberados.Size = new System.Drawing.Size(132, 23);
            this.btn_UnCheck_Liberados.TabIndex = 15;
            this.btn_UnCheck_Liberados.Text = "Desmarcar Todos";
            this.btn_UnCheck_Liberados.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_UnCheck_Liberados.UseVisualStyleBackColor = true;
            this.btn_UnCheck_Liberados.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_UnCheck_Liberados_MouseClick);
            // 
            // btn_Check_Liberados
            // 
            this.btn_Check_Liberados.FlatAppearance.BorderSize = 0;
            this.btn_Check_Liberados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Check_Liberados.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Check_Liberados.Image = global::Conferencia.Properties.Resources.check;
            this.btn_Check_Liberados.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Check_Liberados.Location = new System.Drawing.Point(1045, 33);
            this.btn_Check_Liberados.Name = "btn_Check_Liberados";
            this.btn_Check_Liberados.Size = new System.Drawing.Size(132, 23);
            this.btn_Check_Liberados.TabIndex = 14;
            this.btn_Check_Liberados.Text = "Marcar Todos";
            this.btn_Check_Liberados.UseVisualStyleBackColor = true;
            this.btn_Check_Liberados.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_Check_Liberados_MouseClick);
            // 
            // btn_confirmar_Liberados
            // 
            this.btn_confirmar_Liberados.FlatAppearance.BorderSize = 0;
            this.btn_confirmar_Liberados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_confirmar_Liberados.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_confirmar_Liberados.Image = global::Conferencia.Properties.Resources.Confirmar;
            this.btn_confirmar_Liberados.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_confirmar_Liberados.Location = new System.Drawing.Point(1045, 7);
            this.btn_confirmar_Liberados.Name = "btn_confirmar_Liberados";
            this.btn_confirmar_Liberados.Size = new System.Drawing.Size(114, 20);
            this.btn_confirmar_Liberados.TabIndex = 13;
            this.btn_confirmar_Liberados.Text = "Confirmar";
            this.btn_confirmar_Liberados.UseVisualStyleBackColor = true;
            this.btn_confirmar_Liberados.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_confirmar_Liberados_MouseClick);
            // 
            // btn_Pesquisa
            // 
            this.btn_Pesquisa.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_Pesquisa.FlatAppearance.BorderSize = 0;
            this.btn_Pesquisa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Pesquisa.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Pesquisa.Image = global::Conferencia.Properties.Resources.Pesquisa;
            this.btn_Pesquisa.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Pesquisa.Location = new System.Drawing.Point(40, 137);
            this.btn_Pesquisa.Name = "btn_Pesquisa";
            this.btn_Pesquisa.Size = new System.Drawing.Size(127, 39);
            this.btn_Pesquisa.TabIndex = 11;
            this.btn_Pesquisa.Text = "Pesquisar";
            this.btn_Pesquisa.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Pesquisa.UseVisualStyleBackColor = true;
            this.btn_Pesquisa.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_Pesquisa_MouseClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(40, 182);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1191, 397);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.GridLiberados);
            this.tabPage1.Controls.Add(this.btn_UnCheck_Liberados);
            this.tabPage1.Controls.Add(this.btn_Check_Liberados);
            this.tabPage1.Controls.Add(this.btn_confirmar_Liberados);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1183, 371);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Liberados";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // GridLiberados
            // 
            this.GridLiberados.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.GridLiberados.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.GridLiberados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridLiberados.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.GridLiberados.Location = new System.Drawing.Point(6, 6);
            this.GridLiberados.Name = "GridLiberados";
            this.GridLiberados.RowHeadersVisible = false;
            this.GridLiberados.Size = new System.Drawing.Size(1033, 363);
            this.GridLiberados.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btn_UnCheck_Pendentes);
            this.tabPage2.Controls.Add(this.btn_check_Pendentes);
            this.tabPage2.Controls.Add(this.btn_Confirmar_Pendentes);
            this.tabPage2.Controls.Add(this.GridPendentes);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1183, 371);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Pendentes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_UnCheck_Pendentes
            // 
            this.btn_UnCheck_Pendentes.FlatAppearance.BorderSize = 0;
            this.btn_UnCheck_Pendentes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_UnCheck_Pendentes.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_UnCheck_Pendentes.Image = global::Conferencia.Properties.Resources.SemCheck;
            this.btn_UnCheck_Pendentes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_UnCheck_Pendentes.Location = new System.Drawing.Point(1047, 61);
            this.btn_UnCheck_Pendentes.Name = "btn_UnCheck_Pendentes";
            this.btn_UnCheck_Pendentes.Size = new System.Drawing.Size(132, 23);
            this.btn_UnCheck_Pendentes.TabIndex = 18;
            this.btn_UnCheck_Pendentes.Text = "Desmarcar Todos";
            this.btn_UnCheck_Pendentes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_UnCheck_Pendentes.UseVisualStyleBackColor = true;
            this.btn_UnCheck_Pendentes.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_UnCheck_Pendentes_MouseClick);
            // 
            // btn_check_Pendentes
            // 
            this.btn_check_Pendentes.FlatAppearance.BorderSize = 0;
            this.btn_check_Pendentes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_check_Pendentes.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_check_Pendentes.Image = global::Conferencia.Properties.Resources.check;
            this.btn_check_Pendentes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_check_Pendentes.Location = new System.Drawing.Point(1047, 32);
            this.btn_check_Pendentes.Name = "btn_check_Pendentes";
            this.btn_check_Pendentes.Size = new System.Drawing.Size(132, 23);
            this.btn_check_Pendentes.TabIndex = 17;
            this.btn_check_Pendentes.Text = "Marcar Todos";
            this.btn_check_Pendentes.UseVisualStyleBackColor = true;
            this.btn_check_Pendentes.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_check_Pendentes_MouseClick);
            // 
            // btn_Confirmar_Pendentes
            // 
            this.btn_Confirmar_Pendentes.FlatAppearance.BorderSize = 0;
            this.btn_Confirmar_Pendentes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Confirmar_Pendentes.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Confirmar_Pendentes.Image = global::Conferencia.Properties.Resources.Confirmar;
            this.btn_Confirmar_Pendentes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Confirmar_Pendentes.Location = new System.Drawing.Point(1047, 6);
            this.btn_Confirmar_Pendentes.Name = "btn_Confirmar_Pendentes";
            this.btn_Confirmar_Pendentes.Size = new System.Drawing.Size(114, 20);
            this.btn_Confirmar_Pendentes.TabIndex = 16;
            this.btn_Confirmar_Pendentes.Text = "Confirmar";
            this.btn_Confirmar_Pendentes.UseVisualStyleBackColor = true;
            this.btn_Confirmar_Pendentes.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_Confirmar_Pendentes_MouseClick);
            // 
            // GridPendentes
            // 
            this.GridPendentes.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.GridPendentes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridPendentes.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.GridPendentes.Location = new System.Drawing.Point(3, 6);
            this.GridPendentes.Name = "GridPendentes";
            this.GridPendentes.RowHeadersVisible = false;
            this.GridPendentes.Size = new System.Drawing.Size(1038, 359);
            this.GridPendentes.TabIndex = 0;
            // 
            // DataAte
            // 
            this.DataAte.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DataAte.Location = new System.Drawing.Point(181, 65);
            this.DataAte.Name = "DataAte";
            this.DataAte.Size = new System.Drawing.Size(100, 20);
            this.DataAte.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(149, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "até";
            // 
            // FrmPedidos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1238, 620);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.DataAte);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.Lbl_Usuario);
            this.Controls.Add(this.btn_Pesquisa);
            this.Controls.Add(this.cb_UfCliente);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cb_TipoEnvio);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cb_filial);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cb_Status);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DataDe);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmPedidos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lista de Pedidos";
            this.Load += new System.EventHandler(this.FrmPedidos_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridLiberados)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridPendentes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker DataDe;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_Status;
        private System.Windows.Forms.ComboBox cb_filial;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_TipoEnvio;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_UfCliente;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_Pesquisa;
        private System.Windows.Forms.Label Lbl_Usuario;
        private System.Windows.Forms.Button btn_confirmar_Liberados;
        private System.Windows.Forms.Button btn_Check_Liberados;
        private System.Windows.Forms.Button btn_UnCheck_Liberados;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView GridLiberados;
        private System.Windows.Forms.DataGridView GridPendentes;
        private System.Windows.Forms.Button btn_UnCheck_Pendentes;
        private System.Windows.Forms.Button btn_check_Pendentes;
        private System.Windows.Forms.Button btn_Confirmar_Pendentes;
        private System.Windows.Forms.DateTimePicker DataAte;
        private System.Windows.Forms.Label label6;
    }
}