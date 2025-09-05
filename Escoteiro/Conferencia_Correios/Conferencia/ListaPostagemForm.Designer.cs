namespace Conferencia
{
    partial class ListaPostagemForm
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
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.btn_ImpLista = new System.Windows.Forms.Button();
            this.btn_Integra = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_Desp = new System.Windows.Forms.ComboBox();
            this.tx_dtInicial = new System.Windows.Forms.MaskedTextBox();
            this.tx_dtFinal = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_Pesquisar = new System.Windows.Forms.Button();
            this.cb_transp = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 125);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(627, 183);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValidated);
            // 
            // btn_ImpLista
            // 
            this.btn_ImpLista.Location = new System.Drawing.Point(13, 324);
            this.btn_ImpLista.Name = "btn_ImpLista";
            this.btn_ImpLista.Size = new System.Drawing.Size(153, 23);
            this.btn_ImpLista.TabIndex = 1;
            this.btn_ImpLista.Text = "Imprimir Lista de Postagem";
            this.btn_ImpLista.UseVisualStyleBackColor = true;
            this.btn_ImpLista.Click += new System.EventHandler(this.btn_ImpLista_Click_1);
            // 
            // btn_Integra
            // 
            this.btn_Integra.Location = new System.Drawing.Point(172, 324);
            this.btn_Integra.Name = "btn_Integra";
            this.btn_Integra.Size = new System.Drawing.Size(153, 23);
            this.btn_Integra.TabIndex = 2;
            this.btn_Integra.Text = "Integração Correios";
            this.btn_Integra.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tipo de Despacho:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Código Tranportadora:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Período de Emissão de:";
            // 
            // cb_Desp
            // 
            this.cb_Desp.FormattingEnabled = true;
            this.cb_Desp.Location = new System.Drawing.Point(132, 15);
            this.cb_Desp.Name = "cb_Desp";
            this.cb_Desp.Size = new System.Drawing.Size(121, 21);
            this.cb_Desp.TabIndex = 6;
            // 
            // tx_dtInicial
            // 
            this.tx_dtInicial.Location = new System.Drawing.Point(132, 69);
            this.tx_dtInicial.Mask = "00/00/0000";
            this.tx_dtInicial.Name = "tx_dtInicial";
            this.tx_dtInicial.Size = new System.Drawing.Size(121, 20);
            this.tx_dtInicial.TabIndex = 8;
            this.tx_dtInicial.ValidatingType = typeof(System.DateTime);
            // 
            // tx_dtFinal
            // 
            this.tx_dtFinal.Location = new System.Drawing.Point(290, 69);
            this.tx_dtFinal.Mask = "00/00/0000";
            this.tx_dtFinal.Name = "tx_dtFinal";
            this.tx_dtFinal.Size = new System.Drawing.Size(121, 20);
            this.tx_dtFinal.TabIndex = 9;
            this.tx_dtFinal.ValidatingType = typeof(System.DateTime);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(259, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "até:";
            // 
            // btn_Pesquisar
            // 
            this.btn_Pesquisar.Location = new System.Drawing.Point(15, 96);
            this.btn_Pesquisar.Name = "btn_Pesquisar";
            this.btn_Pesquisar.Size = new System.Drawing.Size(95, 23);
            this.btn_Pesquisar.TabIndex = 11;
            this.btn_Pesquisar.Text = "Pesquisar";
            this.btn_Pesquisar.UseVisualStyleBackColor = true;
            this.btn_Pesquisar.Click += new System.EventHandler(this.btn_Pesquisar_Click);
            // 
            // cb_transp
            // 
            this.cb_transp.FormattingEnabled = true;
            this.cb_transp.Items.AddRange(new object[] {
            "SEDEX",
            "PAC",
            "TRANPORTADORA"});
            this.cb_transp.Location = new System.Drawing.Point(132, 42);
            this.cb_transp.Name = "cb_transp";
            this.cb_transp.Size = new System.Drawing.Size(121, 21);
            this.cb_transp.TabIndex = 12;
            // 
            // ListaPostagemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 362);
            this.Controls.Add(this.cb_transp);
            this.Controls.Add(this.btn_Pesquisar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tx_dtFinal);
            this.Controls.Add(this.tx_dtInicial);
            this.Controls.Add(this.cb_Desp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Integra);
            this.Controls.Add(this.btn_ImpLista);
            this.Controls.Add(this.dataGridView);
            this.Name = "ListaPostagemForm";
            this.Text = "Lista de Postagem";
            this.Load += new System.EventHandler(this.ListaPostagemForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button btn_ImpLista;
        private System.Windows.Forms.Button btn_Integra;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cb_Desp;
        private System.Windows.Forms.MaskedTextBox tx_dtInicial;
        private System.Windows.Forms.MaskedTextBox tx_dtFinal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_Pesquisar;
        private System.Windows.Forms.ComboBox cb_transp;
    }
}