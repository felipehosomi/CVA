namespace Conferencia
{
    partial class FrmGesaoPicking
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGesaoPicking));
            this.tx_dataDe = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.tx_dataAte = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.tx_NumeroPedido = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cb__Status = new System.Windows.Forms.ComboBox();
            this.cb_Filial = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_transportadora = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.GridPicking = new System.Windows.Forms.DataGridView();
            this.tx_User = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_Check = new System.Windows.Forms.Button();
            this.btn_Remove = new System.Windows.Forms.Button();
            this.btn_UnCheck = new System.Windows.Forms.Button();
            this.btn_pesquisar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GridPicking)).BeginInit();
            this.SuspendLayout();
            // 
            // tx_dataDe
            // 
            this.tx_dataDe.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tx_dataDe.Location = new System.Drawing.Point(107, 31);
            this.tx_dataDe.Name = "tx_dataDe";
            this.tx_dataDe.Size = new System.Drawing.Size(84, 20);
            this.tx_dataDe.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(193, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Até:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // tx_dataAte
            // 
            this.tx_dataAte.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tx_dataAte.Location = new System.Drawing.Point(226, 31);
            this.tx_dataAte.Name = "tx_dataAte";
            this.tx_dataAte.Size = new System.Drawing.Size(84, 20);
            this.tx_dataAte.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Numero Pedido:";
            // 
            // tx_NumeroPedido
            // 
            this.tx_NumeroPedido.Location = new System.Drawing.Point(106, 61);
            this.tx_NumeroPedido.Name = "tx_NumeroPedido";
            this.tx_NumeroPedido.Size = new System.Drawing.Size(117, 20);
            this.tx_NumeroPedido.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(326, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Status:";
            // 
            // cb__Status
            // 
            this.cb__Status.FormattingEnabled = true;
            this.cb__Status.Location = new System.Drawing.Point(372, 60);
            this.cb__Status.Name = "cb__Status";
            this.cb__Status.Size = new System.Drawing.Size(302, 21);
            this.cb__Status.TabIndex = 8;
            // 
            // cb_Filial
            // 
            this.cb_Filial.FormattingEnabled = true;
            this.cb_Filial.Items.AddRange(new object[] {
            "Selecione",
            "Data de Entrega",
            "Data do Pedido"});
            this.cb_Filial.Location = new System.Drawing.Point(372, 29);
            this.cb_Filial.Name = "cb_Filial";
            this.cb_Filial.Size = new System.Drawing.Size(302, 21);
            this.cb_Filial.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(329, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Fiial:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // cb_transportadora
            // 
            this.cb_transportadora.FormattingEnabled = true;
            this.cb_transportadora.Items.AddRange(new object[] {
            "Selecione",
            "Data de Entrega",
            "Data do Pedido"});
            this.cb_transportadora.Location = new System.Drawing.Point(795, 30);
            this.cb_transportadora.Name = "cb_transportadora";
            this.cb_transportadora.Size = new System.Drawing.Size(302, 21);
            this.cb_transportadora.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(695, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "Tipo de Frete:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // GridPicking
            // 
            this.GridPicking.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.GridPicking.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.GridPicking.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridPicking.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.GridPicking.Location = new System.Drawing.Point(15, 166);
            this.GridPicking.Name = "GridPicking";
            this.GridPicking.RowHeadersVisible = false;
            this.GridPicking.Size = new System.Drawing.Size(1169, 490);
            this.GridPicking.TabIndex = 14;
            // 
            // tx_User
            // 
            this.tx_User.AutoSize = true;
            this.tx_User.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_User.Location = new System.Drawing.Point(12, 664);
            this.tx_User.Name = "tx_User";
            this.tx_User.Size = new System.Drawing.Size(0, 15);
            this.tx_User.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 15);
            this.label6.TabIndex = 20;
            this.label6.Text = "Data do Pedido:";
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(1190, 291);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(132, 36);
            this.button2.TabIndex = 22;
            this.button2.Text = "ReImprimir Lista ";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(1190, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(117, 36);
            this.button1.TabIndex = 21;
            this.button1.Text = "Imprimir Lista ";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button1_MouseClick);
            // 
            // btn_Check
            // 
            this.btn_Check.FlatAppearance.BorderSize = 0;
            this.btn_Check.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Check.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Check.Image = global::Conferencia.Properties.Resources.check;
            this.btn_Check.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Check.Location = new System.Drawing.Point(1190, 162);
            this.btn_Check.Name = "btn_Check";
            this.btn_Check.Size = new System.Drawing.Size(132, 23);
            this.btn_Check.TabIndex = 19;
            this.btn_Check.Text = "Selecionar Todos";
            this.btn_Check.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Check.UseVisualStyleBackColor = true;
            this.btn_Check.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_Check_MouseClick);
            // 
            // btn_Remove
            // 
            this.btn_Remove.FlatAppearance.BorderSize = 0;
            this.btn_Remove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Remove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Remove.Image = ((System.Drawing.Image)(resources.GetObject("btn_Remove.Image")));
            this.btn_Remove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Remove.Location = new System.Drawing.Point(1190, 220);
            this.btn_Remove.Name = "btn_Remove";
            this.btn_Remove.Size = new System.Drawing.Size(101, 23);
            this.btn_Remove.TabIndex = 18;
            this.btn_Remove.Text = "Remover";
            this.btn_Remove.UseVisualStyleBackColor = true;
            this.btn_Remove.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_Remove_MouseClick);
            // 
            // btn_UnCheck
            // 
            this.btn_UnCheck.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btn_UnCheck.FlatAppearance.BorderSize = 0;
            this.btn_UnCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_UnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_UnCheck.Image = global::Conferencia.Properties.Resources.SemCheck;
            this.btn_UnCheck.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_UnCheck.Location = new System.Drawing.Point(1190, 191);
            this.btn_UnCheck.Name = "btn_UnCheck";
            this.btn_UnCheck.Size = new System.Drawing.Size(132, 23);
            this.btn_UnCheck.TabIndex = 17;
            this.btn_UnCheck.Text = "Desmarcar Todos";
            this.btn_UnCheck.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_UnCheck.UseVisualStyleBackColor = true;
            this.btn_UnCheck.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_UnCheck_MouseClick);
            // 
            // btn_pesquisar
            // 
            this.btn_pesquisar.FlatAppearance.BorderSize = 0;
            this.btn_pesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_pesquisar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_pesquisar.Image = global::Conferencia.Properties.Resources.Pesquisa;
            this.btn_pesquisar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_pesquisar.Location = new System.Drawing.Point(15, 110);
            this.btn_pesquisar.Name = "btn_pesquisar";
            this.btn_pesquisar.Size = new System.Drawing.Size(135, 39);
            this.btn_pesquisar.TabIndex = 13;
            this.btn_pesquisar.Text = "Pesquisar";
            this.btn_pesquisar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_pesquisar.UseVisualStyleBackColor = true;
            this.btn_pesquisar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_pesquisar_MouseClick);
            // 
            // FrmGesaoPicking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1331, 688);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_Check);
            this.Controls.Add(this.btn_Remove);
            this.Controls.Add(this.btn_UnCheck);
            this.Controls.Add(this.tx_User);
            this.Controls.Add(this.GridPicking);
            this.Controls.Add(this.btn_pesquisar);
            this.Controls.Add(this.cb_transportadora);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cb_Filial);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cb__Status);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tx_NumeroPedido);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tx_dataAte);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tx_dataDe);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmGesaoPicking";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Gestão de Picking";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmGesaoPicking_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridPicking)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DateTimePicker tx_dataDe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker tx_dataAte;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox tx_NumeroPedido;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb__Status;
        private System.Windows.Forms.ComboBox cb_Filial;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_transportadora;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_pesquisar;
        private System.Windows.Forms.DataGridView GridPicking;
        private System.Windows.Forms.Label tx_User;
        private System.Windows.Forms.Button btn_UnCheck;
        private System.Windows.Forms.Button btn_Remove;
        private System.Windows.Forms.Button btn_Check;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}