using System.Drawing;

namespace ImpressorEtiquetas
{
    partial class Impressor
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbTipoEtiqueta = new System.Windows.Forms.ComboBox();
            this.bImprimir = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.tbOrdem = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbImpressora = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbQuantidade = new System.Windows.Forms.TextBox();
            this.bBuscar = new System.Windows.Forms.Button();
            this.tbNomeProduto = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cbTipoEtiqueta
            // 
            this.cbTipoEtiqueta.FormattingEnabled = true;
            this.cbTipoEtiqueta.Items.AddRange(new object[] {
            "Padrão",
            "Padrão (Inglês)",
            "Lote",
            "Pacote"});
            this.cbTipoEtiqueta.Location = new System.Drawing.Point(146, 38);
            this.cbTipoEtiqueta.Name = "cbTipoEtiqueta";
            this.cbTipoEtiqueta.Size = new System.Drawing.Size(115, 21);
            this.cbTipoEtiqueta.TabIndex = 2;
            // 
            // bImprimir
            // 
            this.bImprimir.Location = new System.Drawing.Point(295, 71);
            this.bImprimir.Name = "bImprimir";
            this.bImprimir.Size = new System.Drawing.Size(75, 36);
            this.bImprimir.TabIndex = 9;
            this.bImprimir.Text = "Imprimir";
            this.bImprimir.UseVisualStyleBackColor = true;
            this.bImprimir.Click += new System.EventHandler(this.bImprimir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(143, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Tipo de Etiqueta:";
            // 
            // tbStatus
            // 
            this.tbStatus.BackColor = System.Drawing.Color.Gray;
            this.tbStatus.Location = new System.Drawing.Point(39, 152);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(309, 20);
            this.tbStatus.TabIndex = 7;
            this.tbStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbOrdem
            // 
            this.tbOrdem.Location = new System.Drawing.Point(12, 36);
            this.tbOrdem.Name = "tbOrdem";
            this.tbOrdem.Size = new System.Drawing.Size(115, 20);
            this.tbOrdem.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "OP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Impressora:";
            // 
            // cbImpressora
            // 
            this.cbImpressora.FormattingEnabled = true;
            this.cbImpressora.Location = new System.Drawing.Point(12, 87);
            this.cbImpressora.Name = "cbImpressora";
            this.cbImpressora.Size = new System.Drawing.Size(159, 21);
            this.cbImpressora.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(192, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Quant:";
            // 
            // tbQuantidade
            // 
            this.tbQuantidade.Location = new System.Drawing.Point(193, 87);
            this.tbQuantidade.Name = "tbQuantidade";
            this.tbQuantidade.Size = new System.Drawing.Size(68, 20);
            this.tbQuantidade.TabIndex = 4;
            // 
            // bBuscar
            // 
            this.bBuscar.Location = new System.Drawing.Point(295, 23);
            this.bBuscar.Name = "bBuscar";
            this.bBuscar.Size = new System.Drawing.Size(75, 36);
            this.bBuscar.TabIndex = 0;
            this.bBuscar.Text = "Buscar";
            this.bBuscar.UseVisualStyleBackColor = true;
            this.bBuscar.Click += new System.EventHandler(this.bBuscar_Click);
            // 
            // tbNomeProduto
            // 
            this.tbNomeProduto.Enabled = false;
            this.tbNomeProduto.Location = new System.Drawing.Point(39, 126);
            this.tbNomeProduto.Name = "tbNomeProduto";
            this.tbNomeProduto.Size = new System.Drawing.Size(309, 20);
            this.tbNomeProduto.TabIndex = 10;
            // 
            // Impressor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 188);
            this.Controls.Add(this.tbNomeProduto);
            this.Controls.Add(this.bBuscar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbQuantidade);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbImpressora);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbOrdem);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bImprimir);
            this.Controls.Add(this.cbTipoEtiqueta);
            this.Name = "Impressor";
            this.Text = "CVA - Impressor de Etiquetas";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbTipoEtiqueta;
        private System.Windows.Forms.Button bImprimir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.TextBox tbOrdem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbImpressora;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbQuantidade;
        private System.Windows.Forms.Button bBuscar;
        private System.Windows.Forms.TextBox tbNomeProduto;
    }
}

