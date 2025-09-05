namespace Conferencia
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
            this.tx_ID = new System.Windows.Forms.TextBox();
            this.btn_add = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tx_Cliente = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridConf1 = new System.Windows.Forms.DataGridView();
            this.tx_produto = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Excluir = new System.Windows.Forms.Button();
            this.tx_quantidade = new System.Windows.Forms.TextBox();
            this.tx_user = new System.Windows.Forms.Label();
            this.lbl_Escaneado = new System.Windows.Forms.Label();
            this.lbl_QtdTotal = new System.Windows.Forms.Label();
            this.lbl_diferenca = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lbl_desc = new System.Windows.Forms.Label();
            this.lbl_CodItem = new System.Windows.Forms.Label();
            this.lbl_CodBarras = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
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
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btn_FinalizaParcial = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridConf1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(491, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID:";
            this.label1.Visible = false;
            // 
            // tx_ID
            // 
            this.tx_ID.Location = new System.Drawing.Point(513, 92);
            this.tx_ID.Name = "tx_ID";
            this.tx_ID.Size = new System.Drawing.Size(100, 20);
            this.tx_ID.TabIndex = 1;
            this.tx_ID.Visible = false;
            this.tx_ID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tx_ID_KeyDown);
            // 
            // btn_add
            // 
            this.btn_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_add.Image = global::Conferencia.Properties.Resources.Finalizar;
            this.btn_add.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_add.Location = new System.Drawing.Point(1153, 652);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(93, 23);
            this.btn_add.TabIndex = 5;
            this.btn_add.Text = "Finalizar";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Image = global::Conferencia.Properties.Resources.Remover;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(1252, 652);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(93, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Cancelar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button2_MouseClick);
            // 
            // tx_Cliente
            // 
            this.tx_Cliente.Enabled = false;
            this.tx_Cliente.Location = new System.Drawing.Point(513, 118);
            this.tx_Cliente.Name = "tx_Cliente";
            this.tx_Cliente.Size = new System.Drawing.Size(100, 20);
            this.tx_Cliente.TabIndex = 2;
            this.tx_Cliente.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(466, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Cliente:";
            this.label2.Visible = false;
            // 
            // dataGridConf1
            // 
            this.dataGridConf1.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridConf1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridConf1.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridConf1.Location = new System.Drawing.Point(12, 172);
            this.dataGridConf1.Name = "dataGridConf1";
            this.dataGridConf1.ReadOnly = true;
            this.dataGridConf1.RowHeadersVisible = false;
            this.dataGridConf1.Size = new System.Drawing.Size(1333, 395);
            this.dataGridConf1.TabIndex = 8;
            // 
            // tx_produto
            // 
            this.tx_produto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tx_produto.Location = new System.Drawing.Point(112, 22);
            this.tx_produto.Name = "tx_produto";
            this.tx_produto.Size = new System.Drawing.Size(190, 20);
            this.tx_produto.TabIndex = 3;
            this.tx_produto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tx_produto_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Cód Barras:";
            // 
            // btn_Excluir
            // 
            this.btn_Excluir.Location = new System.Drawing.Point(538, 65);
            this.btn_Excluir.Name = "btn_Excluir";
            this.btn_Excluir.Size = new System.Drawing.Size(75, 23);
            this.btn_Excluir.TabIndex = 7;
            this.btn_Excluir.Text = "Excluir Linha";
            this.btn_Excluir.UseVisualStyleBackColor = true;
            this.btn_Excluir.Visible = false;
            this.btn_Excluir.Click += new System.EventHandler(this.btn_Excluir_Click);
            // 
            // tx_quantidade
            // 
            this.tx_quantidade.Location = new System.Drawing.Point(513, 146);
            this.tx_quantidade.Name = "tx_quantidade";
            this.tx_quantidade.Size = new System.Drawing.Size(100, 20);
            this.tx_quantidade.TabIndex = 4;
            this.tx_quantidade.Visible = false;
            this.tx_quantidade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tx_quantidade_KeyDown);
            // 
            // tx_user
            // 
            this.tx_user.AutoSize = true;
            this.tx_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_user.Location = new System.Drawing.Point(9, 673);
            this.tx_user.Name = "tx_user";
            this.tx_user.Size = new System.Drawing.Size(0, 13);
            this.tx_user.TabIndex = 14;
            // 
            // lbl_Escaneado
            // 
            this.lbl_Escaneado.AutoSize = true;
            this.lbl_Escaneado.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Escaneado.Location = new System.Drawing.Point(12, 579);
            this.lbl_Escaneado.Name = "lbl_Escaneado";
            this.lbl_Escaneado.Size = new System.Drawing.Size(133, 17);
            this.lbl_Escaneado.TabIndex = 15;
            this.lbl_Escaneado.Text = "Qtde Escaneado:";
            // 
            // lbl_QtdTotal
            // 
            this.lbl_QtdTotal.AutoSize = true;
            this.lbl_QtdTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_QtdTotal.Location = new System.Drawing.Point(55, 605);
            this.lbl_QtdTotal.Name = "lbl_QtdTotal";
            this.lbl_QtdTotal.Size = new System.Drawing.Size(90, 17);
            this.lbl_QtdTotal.TabIndex = 16;
            this.lbl_QtdTotal.Text = "Qtde Total:";
            // 
            // lbl_diferenca
            // 
            this.lbl_diferenca.AutoSize = true;
            this.lbl_diferenca.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_diferenca.Location = new System.Drawing.Point(61, 633);
            this.lbl_diferenca.Name = "lbl_diferenca";
            this.lbl_diferenca.Size = new System.Drawing.Size(83, 17);
            this.lbl_diferenca.TabIndex = 17;
            this.lbl_diferenca.Text = "Diferença:";
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
            this.panel1.Location = new System.Drawing.Point(112, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(335, 118);
            this.panel1.TabIndex = 20;
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
            this.panel2.Location = new System.Drawing.Point(794, 20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(555, 107);
            this.panel2.TabIndex = 27;
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
            this.textBox1.Size = new System.Drawing.Size(555, 21);
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
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 47);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(69, 13);
            this.label15.TabIndex = 22;
            this.label15.Text = "Data Pedido:";
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
            // btn_FinalizaParcial
            // 
            this.btn_FinalizaParcial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_FinalizaParcial.Image = global::Conferencia.Properties.Resources.Confirmar;
            this.btn_FinalizaParcial.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_FinalizaParcial.Location = new System.Drawing.Point(1015, 652);
            this.btn_FinalizaParcial.Name = "btn_FinalizaParcial";
            this.btn_FinalizaParcial.Size = new System.Drawing.Size(127, 23);
            this.btn_FinalizaParcial.TabIndex = 28;
            this.btn_FinalizaParcial.Text = "Conferência Parcial";
            this.btn_FinalizaParcial.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_FinalizaParcial.UseVisualStyleBackColor = true;
            this.btn_FinalizaParcial.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btn_FinalizaParcial_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1357, 694);
            this.Controls.Add(this.btn_FinalizaParcial);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbl_diferenca);
            this.Controls.Add(this.lbl_QtdTotal);
            this.Controls.Add(this.lbl_Escaneado);
            this.Controls.Add(this.tx_user);
            this.Controls.Add(this.btn_Excluir);
            this.Controls.Add(this.tx_quantidade);
            this.Controls.Add(this.tx_produto);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tx_Cliente);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.tx_ID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridConf1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Conferência de Mercadoria";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridConf1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tx_ID;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tx_Cliente;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridConf1;
        private System.Windows.Forms.TextBox tx_produto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Excluir;
        private System.Windows.Forms.TextBox tx_quantidade;
        private System.Windows.Forms.Label tx_user;
        private System.Windows.Forms.Label lbl_Escaneado;
        private System.Windows.Forms.Label lbl_QtdTotal;
        private System.Windows.Forms.Label lbl_diferenca;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_desc;
        private System.Windows.Forms.Label lbl_CodItem;
        private System.Windows.Forms.Label lbl_CodBarras;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label lbl_Origem;
        private System.Windows.Forms.Label lbl_Cliente;
        private System.Windows.Forms.Label lbl_DataPedido;
        private System.Windows.Forms.Label lbl_Pedido;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lbl_DataEntrega;
        private System.Windows.Forms.Label lbl_TipoEntrega;
        private System.Windows.Forms.Button btn_FinalizaParcial;
    }
}

