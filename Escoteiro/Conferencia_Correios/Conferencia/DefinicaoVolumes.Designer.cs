namespace Conferencia
{
    partial class DefinicaoVolumes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefinicaoVolumes));
            this.label1 = new System.Windows.Forms.Label();
            this.tx_pedido = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tx_quantidade = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tx_peso = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.cb_Embalagem = new System.Windows.Forms.ComboBox();
            this.btn_Doc = new System.Windows.Forms.Button();
            this.dataGridEbalagem = new System.Windows.Forms.DataGridView();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_ExcluirLinha = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridEbalagem)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pedido:";
            // 
            // tx_pedido
            // 
            this.tx_pedido.Enabled = false;
            this.tx_pedido.Location = new System.Drawing.Point(103, 15);
            this.tx_pedido.Name = "tx_pedido";
            this.tx_pedido.Size = new System.Drawing.Size(233, 20);
            this.tx_pedido.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tipo Embalagem:";
            // 
            // tx_quantidade
            // 
            this.tx_quantidade.Location = new System.Drawing.Point(103, 67);
            this.tx_quantidade.Name = "tx_quantidade";
            this.tx_quantidade.Size = new System.Drawing.Size(233, 20);
            this.tx_quantidade.TabIndex = 5;
            this.tx_quantidade.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tx_quantidade_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Quantidade:";
            // 
            // tx_peso
            // 
            this.tx_peso.Location = new System.Drawing.Point(103, 93);
            this.tx_peso.Name = "tx_peso";
            this.tx_peso.Size = new System.Drawing.Size(233, 20);
            this.tx_peso.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Peso:";
            // 
            // btn_add
            // 
            this.btn_add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_add.Image = global::Conferencia.Properties.Resources.Confirmar;
            this.btn_add.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_add.Location = new System.Drawing.Point(22, 311);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 8;
            this.btn_add.Text = "Confirmar";
            this.btn_add.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cancel.Image = global::Conferencia.Properties.Resources.Remover;
            this.btn_cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_cancel.Location = new System.Drawing.Point(109, 311);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(78, 23);
            this.btn_cancel.TabIndex = 9;
            this.btn_cancel.Text = "Cancelar";
            this.btn_cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // cb_Embalagem
            // 
            this.cb_Embalagem.FormattingEnabled = true;
            this.cb_Embalagem.Location = new System.Drawing.Point(103, 40);
            this.cb_Embalagem.Name = "cb_Embalagem";
            this.cb_Embalagem.Size = new System.Drawing.Size(233, 21);
            this.cb_Embalagem.TabIndex = 10;
            // 
            // btn_Doc
            // 
            this.btn_Doc.Enabled = false;
            this.btn_Doc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Doc.Image = global::Conferencia.Properties.Resources.Documento;
            this.btn_Doc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Doc.Location = new System.Drawing.Point(202, 311);
            this.btn_Doc.Name = "btn_Doc";
            this.btn_Doc.Size = new System.Drawing.Size(182, 23);
            this.btn_Doc.TabIndex = 11;
            this.btn_Doc.Text = "Geração do Documento Fiscal";
            this.btn_Doc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Doc.UseVisualStyleBackColor = true;
            this.btn_Doc.Click += new System.EventHandler(this.btn_Doc_Click);
            // 
            // dataGridEbalagem
            // 
            this.dataGridEbalagem.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridEbalagem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridEbalagem.Location = new System.Drawing.Point(22, 147);
            this.dataGridEbalagem.Name = "dataGridEbalagem";
            this.dataGridEbalagem.ReadOnly = true;
            this.dataGridEbalagem.Size = new System.Drawing.Size(567, 150);
            this.dataGridEbalagem.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(19, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Lista de Volumes";
            // 
            // btn_ExcluirLinha
            // 
            this.btn_ExcluirLinha.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ExcluirLinha.Image = global::Conferencia.Properties.Resources.Remover;
            this.btn_ExcluirLinha.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_ExcluirLinha.Location = new System.Drawing.Point(473, 311);
            this.btn_ExcluirLinha.Name = "btn_ExcluirLinha";
            this.btn_ExcluirLinha.Size = new System.Drawing.Size(116, 23);
            this.btn_ExcluirLinha.TabIndex = 14;
            this.btn_ExcluirLinha.Text = "Excluir linhas sel.";
            this.btn_ExcluirLinha.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_ExcluirLinha.UseVisualStyleBackColor = true;
            this.btn_ExcluirLinha.Click += new System.EventHandler(this.btn_ExcluirLinha_Click);
            // 
            // DefinicaoVolumes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(601, 348);
            this.Controls.Add(this.btn_ExcluirLinha);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataGridEbalagem);
            this.Controls.Add(this.btn_Doc);
            this.Controls.Add(this.cb_Embalagem);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.tx_peso);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tx_quantidade);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tx_pedido);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DefinicaoVolumes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Definição Volumes";
            this.Load += new System.EventHandler(this.DefinicaoVolumes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridEbalagem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tx_pedido;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tx_quantidade;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tx_peso;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.ComboBox cb_Embalagem;
        private System.Windows.Forms.Button btn_Doc;
        private System.Windows.Forms.DataGridView dataGridEbalagem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_ExcluirLinha;
    }
}