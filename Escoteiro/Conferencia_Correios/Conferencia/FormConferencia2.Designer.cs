namespace Conferencia
{
    partial class FormConferencia2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConferencia2));
            this.btn_Excluir = new System.Windows.Forms.Button();
            this.tx_quantidade = new System.Windows.Forms.TextBox();
            this.tx_produto = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.tx_ID = new System.Windows.Forms.TextBox();
            this.dataGridConf2 = new System.Windows.Forms.DataGridView();
            this.btn_vol = new System.Windows.Forms.Button();
            this.lbl_DataEntrega = new System.Windows.Forms.Label();
            this.lbl_TipoEntrega = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.lbl_Origem = new System.Windows.Forms.Label();
            this.lbl_Cliente = new System.Windows.Forms.Label();
            this.lbl_DataPedido = new System.Windows.Forms.Label();
            this.lbl_Pedido = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lbl_desc = new System.Windows.Forms.Label();
            this.lbl_CodItem = new System.Windows.Forms.Label();
            this.lbl_CodBarras = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_diferenca = new System.Windows.Forms.Label();
            this.lbl_QtdTotal = new System.Windows.Forms.Label();
            this.lbl_Escaneado = new System.Windows.Forms.Label();
            this.tx_user = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridConf2)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Excluir
            // 
            this.btn_Excluir.Location = new System.Drawing.Point(491, 13);
            this.btn_Excluir.Name = "btn_Excluir";
            this.btn_Excluir.Size = new System.Drawing.Size(75, 23);
            this.btn_Excluir.TabIndex = 23;
            this.btn_Excluir.Text = "Excluir Linha";
            this.btn_Excluir.UseVisualStyleBackColor = true;
            this.btn_Excluir.Visible = false;
            this.btn_Excluir.Click += new System.EventHandler(this.btn_Excluir_Click);
            // 
            // tx_quantidade
            // 
            this.tx_quantidade.Location = new System.Drawing.Point(491, 42);
            this.tx_quantidade.Name = "tx_quantidade";
            this.tx_quantidade.Size = new System.Drawing.Size(75, 20);
            this.tx_quantidade.TabIndex = 19;
            this.tx_quantidade.Visible = false;
            this.tx_quantidade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tx_quantidade_KeyDown);
            // 
            // tx_produto
            // 
            this.tx_produto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tx_produto.Location = new System.Drawing.Point(117, 18);
            this.tx_produto.Name = "tx_produto";
            this.tx_produto.Size = new System.Drawing.Size(152, 20);
            this.tx_produto.TabIndex = 18;
            this.tx_produto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tx_produto_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(17, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 17);
            this.label3.TabIndex = 24;
            this.label3.Text = "Cód Barras:";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = global::Conferencia.Properties.Resources.Remover;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(1256, 664);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(87, 30);
            this.button2.TabIndex = 22;
            this.button2.Text = "Cancelar";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_add
            // 
            this.btn_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_add.Image = global::Conferencia.Properties.Resources.Finalizar;
            this.btn_add.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_add.Location = new System.Drawing.Point(1159, 664);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(80, 30);
            this.btn_add.TabIndex = 21;
            this.btn_add.Text = "Finalizar";
            this.btn_add.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // tx_ID
            // 
            this.tx_ID.Location = new System.Drawing.Point(491, 68);
            this.tx_ID.Name = "tx_ID";
            this.tx_ID.Size = new System.Drawing.Size(75, 20);
            this.tx_ID.TabIndex = 16;
            this.tx_ID.Visible = false;
            this.tx_ID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tx_ID_KeyDown);
            // 
            // dataGridConf2
            // 
            this.dataGridConf2.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridConf2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridConf2.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridConf2.Location = new System.Drawing.Point(12, 179);
            this.dataGridConf2.Name = "dataGridConf2";
            this.dataGridConf2.ReadOnly = true;
            this.dataGridConf2.RowHeadersVisible = false;
            this.dataGridConf2.Size = new System.Drawing.Size(1331, 406);
            this.dataGridConf2.TabIndex = 25;
            // 
            // btn_vol
            // 
            this.btn_vol.Enabled = false;
            this.btn_vol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_vol.Image = ((System.Drawing.Image)(resources.GetObject("btn_vol.Image")));
            this.btn_vol.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_vol.Location = new System.Drawing.Point(839, 664);
            this.btn_vol.Name = "btn_vol";
            this.btn_vol.Size = new System.Drawing.Size(148, 30);
            this.btn_vol.TabIndex = 28;
            this.btn_vol.Text = "Definição de Volumes";
            this.btn_vol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_vol.UseVisualStyleBackColor = true;
            this.btn_vol.Click += new System.EventHandler(this.btn_vol_Click);
            // 
            // lbl_DataEntrega
            // 
            this.lbl_DataEntrega.AutoSize = true;
            this.lbl_DataEntrega.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_DataEntrega.Location = new System.Drawing.Point(305, 49);
            this.lbl_DataEntrega.Name = "lbl_DataEntrega";
            this.lbl_DataEntrega.Size = new System.Drawing.Size(0, 13);
            this.lbl_DataEntrega.TabIndex = 37;
            // 
            // lbl_TipoEntrega
            // 
            this.lbl_TipoEntrega.AutoSize = true;
            this.lbl_TipoEntrega.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TipoEntrega.Location = new System.Drawing.Point(305, 28);
            this.lbl_TipoEntrega.Name = "lbl_TipoEntrega";
            this.lbl_TipoEntrega.Size = new System.Drawing.Size(0, 13);
            this.lbl_TipoEntrega.TabIndex = 36;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(227, 46);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(73, 13);
            this.label20.TabIndex = 35;
            this.label20.Text = "Data Entrega:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(228, 28);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(71, 13);
            this.label21.TabIndex = 34;
            this.label21.Text = "Tipo Entrega:";
            // 
            // lbl_Origem
            // 
            this.lbl_Origem.AutoSize = true;
            this.lbl_Origem.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Origem.Location = new System.Drawing.Point(77, 84);
            this.lbl_Origem.Name = "lbl_Origem";
            this.lbl_Origem.Size = new System.Drawing.Size(0, 13);
            this.lbl_Origem.TabIndex = 33;
            // 
            // lbl_Cliente
            // 
            this.lbl_Cliente.AutoSize = true;
            this.lbl_Cliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Cliente.Location = new System.Drawing.Point(76, 65);
            this.lbl_Cliente.Name = "lbl_Cliente";
            this.lbl_Cliente.Size = new System.Drawing.Size(0, 13);
            this.lbl_Cliente.TabIndex = 32;
            // 
            // lbl_DataPedido
            // 
            this.lbl_DataPedido.AutoSize = true;
            this.lbl_DataPedido.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_DataPedido.Location = new System.Drawing.Point(76, 46);
            this.lbl_DataPedido.Name = "lbl_DataPedido";
            this.lbl_DataPedido.Size = new System.Drawing.Size(0, 13);
            this.lbl_DataPedido.TabIndex = 31;
            // 
            // lbl_Pedido
            // 
            this.lbl_Pedido.AutoSize = true;
            this.lbl_Pedido.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Pedido.Location = new System.Drawing.Point(76, 28);
            this.lbl_Pedido.Name = "lbl_Pedido";
            this.lbl_Pedido.Size = new System.Drawing.Size(0, 13);
            this.lbl_Pedido.TabIndex = 30;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 85);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Origem:";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.PowderBlue;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(-1, -1);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(528, 21);
            this.textBox1.TabIndex = 28;
            this.textBox1.Text = "Informações do Pedido";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 66);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(42, 13);
            this.label14.TabIndex = 23;
            this.label14.Text = "Cliente:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(3, 29);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(43, 13);
            this.label16.TabIndex = 21;
            this.label16.Text = "Pedido:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lbl_DataEntrega);
            this.panel2.Controls.Add(this.lbl_TipoEntrega);
            this.panel2.Controls.Add(this.label20);
            this.panel2.Controls.Add(this.label21);
            this.panel2.Controls.Add(this.lbl_Origem);
            this.panel2.Controls.Add(this.lbl_Cliente);
            this.panel2.Controls.Add(this.lbl_DataPedido);
            this.panel2.Controls.Add(this.lbl_Pedido);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Location = new System.Drawing.Point(815, 43);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(528, 107);
            this.panel2.TabIndex = 29;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 47);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(69, 13);
            this.label15.TabIndex = 22;
            this.label15.Text = "Data Pedido:";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.PowderBlue;
            this.textBox2.Enabled = false;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(-1, -1);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(336, 21);
            this.textBox2.TabIndex = 28;
            this.textBox2.Text = "Último Item Escaneado";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbl_desc
            // 
            this.lbl_desc.AutoSize = true;
            this.lbl_desc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_desc.Location = new System.Drawing.Point(71, 66);
            this.lbl_desc.Name = "lbl_desc";
            this.lbl_desc.Size = new System.Drawing.Size(0, 13);
            this.lbl_desc.TabIndex = 26;
            // 
            // lbl_CodItem
            // 
            this.lbl_CodItem.AutoSize = true;
            this.lbl_CodItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CodItem.Location = new System.Drawing.Point(71, 47);
            this.lbl_CodItem.Name = "lbl_CodItem";
            this.lbl_CodItem.Size = new System.Drawing.Size(0, 13);
            this.lbl_CodItem.TabIndex = 25;
            // 
            // lbl_CodBarras
            // 
            this.lbl_CodBarras.AutoSize = true;
            this.lbl_CodBarras.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CodBarras.Location = new System.Drawing.Point(71, 27);
            this.lbl_CodBarras.Name = "lbl_CodBarras";
            this.lbl_CodBarras.Size = new System.Drawing.Size(0, 13);
            this.lbl_CodBarras.TabIndex = 24;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Descrição:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 22;
            this.label9.Text = "Cód Item:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Cód Barras:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.lbl_desc);
            this.panel1.Controls.Add(this.lbl_CodItem);
            this.panel1.Controls.Add(this.lbl_CodBarras);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(117, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(335, 118);
            this.panel1.TabIndex = 30;
            // 
            // lbl_diferenca
            // 
            this.lbl_diferenca.AutoSize = true;
            this.lbl_diferenca.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_diferenca.Location = new System.Drawing.Point(61, 642);
            this.lbl_diferenca.Name = "lbl_diferenca";
            this.lbl_diferenca.Size = new System.Drawing.Size(83, 17);
            this.lbl_diferenca.TabIndex = 34;
            this.lbl_diferenca.Text = "Diferença:";
            // 
            // lbl_QtdTotal
            // 
            this.lbl_QtdTotal.AutoSize = true;
            this.lbl_QtdTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_QtdTotal.Location = new System.Drawing.Point(55, 614);
            this.lbl_QtdTotal.Name = "lbl_QtdTotal";
            this.lbl_QtdTotal.Size = new System.Drawing.Size(90, 17);
            this.lbl_QtdTotal.TabIndex = 33;
            this.lbl_QtdTotal.Text = "Qtde Total:";
            // 
            // lbl_Escaneado
            // 
            this.lbl_Escaneado.AutoSize = true;
            this.lbl_Escaneado.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Escaneado.Location = new System.Drawing.Point(12, 588);
            this.lbl_Escaneado.Name = "lbl_Escaneado";
            this.lbl_Escaneado.Size = new System.Drawing.Size(133, 17);
            this.lbl_Escaneado.TabIndex = 32;
            this.lbl_Escaneado.Text = "Qtde Escaneado:";
            // 
            // tx_user
            // 
            this.tx_user.AutoSize = true;
            this.tx_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_user.Location = new System.Drawing.Point(12, 673);
            this.tx_user.Name = "tx_user";
            this.tx_user.Size = new System.Drawing.Size(0, 13);
            this.tx_user.TabIndex = 35;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = global::Conferencia.Properties.Resources.Confirmar;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(1006, 664);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 30);
            this.button1.TabIndex = 36;
            this.button1.Text = "Conferência Parcial";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormConferencia2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1377, 710);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tx_user);
            this.Controls.Add(this.lbl_diferenca);
            this.Controls.Add(this.lbl_QtdTotal);
            this.Controls.Add(this.lbl_Escaneado);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btn_vol);
            this.Controls.Add(this.btn_Excluir);
            this.Controls.Add(this.tx_quantidade);
            this.Controls.Add(this.tx_produto);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.tx_ID);
            this.Controls.Add(this.dataGridConf2);
            this.Name = "FormConferencia2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Conferência de Mercadoria";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormConferencia2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridConf2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_Excluir;
        private System.Windows.Forms.TextBox tx_quantidade;
        private System.Windows.Forms.TextBox tx_produto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.TextBox tx_ID;
        private System.Windows.Forms.DataGridView dataGridConf2;
        private System.Windows.Forms.Button btn_vol;
        private System.Windows.Forms.Label lbl_DataEntrega;
        private System.Windows.Forms.Label lbl_TipoEntrega;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label lbl_Origem;
        private System.Windows.Forms.Label lbl_Cliente;
        private System.Windows.Forms.Label lbl_DataPedido;
        private System.Windows.Forms.Label lbl_Pedido;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lbl_desc;
        private System.Windows.Forms.Label lbl_CodItem;
        private System.Windows.Forms.Label lbl_CodBarras;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_diferenca;
        private System.Windows.Forms.Label lbl_QtdTotal;
        private System.Windows.Forms.Label lbl_Escaneado;
        private System.Windows.Forms.Label tx_user;
        private System.Windows.Forms.Button button1;
    }
}