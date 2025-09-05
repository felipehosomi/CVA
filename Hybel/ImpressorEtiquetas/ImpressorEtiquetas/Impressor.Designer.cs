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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbpPendente = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxOpAte = new System.Windows.Forms.NumericUpDown();
            this.btnBuscaPendente = new System.Windows.Forms.Button();
            this.tbxOpDe = new System.Windows.Forms.NumericUpDown();
            this.btnImprimePendente = new System.Windows.Forms.Button();
            this.tbxDataAte = new System.Windows.Forms.MaskedTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbxDataDe = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxReimpressao = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvPendente = new System.Windows.Forms.DataGridView();
            this.tbpAvulsa = new System.Windows.Forms.TabPage();
            this.tbxItemNameAvulsa = new System.Windows.Forms.TextBox();
            this.tbxQtdAvulsa = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.btnBuscaAvulsa = new System.Windows.Forms.Button();
            this.btnImprimirAvulsa = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbxSerieAvulsa = new System.Windows.Forms.TextBox();
            this.tbxItemCodeAvulsa = new System.Windows.Forms.TextBox();
            this.tbpOP = new System.Windows.Forms.TabPage();
            this.tbxQtdeOP = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxItemNameOP = new System.Windows.Forms.TextBox();
            this.btnBuscaPorOP = new System.Windows.Forms.Button();
            this.btnImprimePorOP = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxStatusOP = new System.Windows.Forms.TextBox();
            this.tbOrdem = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxImpressora = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1.SuspendLayout();
            this.tbpPendente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbxOpAte)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbxOpDe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendente)).BeginInit();
            this.tbpAvulsa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbxQtdAvulsa)).BeginInit();
            this.tbpOP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbxQtdeOP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbpPendente);
            this.tabControl1.Controls.Add(this.tbpAvulsa);
            this.tabControl1.Controls.Add(this.tbpOP);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(850, 455);
            this.tabControl1.TabIndex = 11;
            // 
            // tbpPendente
            // 
            this.tbpPendente.Controls.Add(this.splitContainer2);
            this.tbpPendente.Location = new System.Drawing.Point(4, 22);
            this.tbpPendente.Name = "tbpPendente";
            this.tbpPendente.Padding = new System.Windows.Forms.Padding(3);
            this.tbpPendente.Size = new System.Drawing.Size(842, 429);
            this.tbpPendente.TabIndex = 0;
            this.tbpPendente.Text = "Pendentes";
            this.tbpPendente.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label5);
            this.splitContainer2.Panel1.Controls.Add(this.tbxOpAte);
            this.splitContainer2.Panel1.Controls.Add(this.btnBuscaPendente);
            this.splitContainer2.Panel1.Controls.Add(this.tbxOpDe);
            this.splitContainer2.Panel1.Controls.Add(this.btnImprimePendente);
            this.splitContainer2.Panel1.Controls.Add(this.tbxDataAte);
            this.splitContainer2.Panel1.Controls.Add(this.label6);
            this.splitContainer2.Panel1.Controls.Add(this.tbxDataDe);
            this.splitContainer2.Panel1.Controls.Add(this.label8);
            this.splitContainer2.Panel1.Controls.Add(this.cbxReimpressao);
            this.splitContainer2.Panel1.Controls.Add(this.label7);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dgvPendente);
            this.splitContainer2.Size = new System.Drawing.Size(836, 423);
            this.splitContainer2.SplitterDistance = 57;
            this.splitContainer2.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Data";
            // 
            // tbxOpAte
            // 
            this.tbxOpAte.Location = new System.Drawing.Point(202, 35);
            this.tbxOpAte.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.tbxOpAte.Name = "tbxOpAte";
            this.tbxOpAte.Size = new System.Drawing.Size(100, 20);
            this.tbxOpAte.TabIndex = 40;
            this.tbxOpAte.Value = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.tbxOpAte.Click += new System.EventHandler(this.tbxOpAte_Click);
            this.tbxOpAte.Enter += new System.EventHandler(this.tbxOpAte_Enter);
            this.tbxOpAte.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxPendente_KeyDown);
            // 
            // btnBuscaPendente
            // 
            this.btnBuscaPendente.Location = new System.Drawing.Point(422, 8);
            this.btnBuscaPendente.Name = "btnBuscaPendente";
            this.btnBuscaPendente.Size = new System.Drawing.Size(75, 36);
            this.btnBuscaPendente.TabIndex = 60;
            this.btnBuscaPendente.Text = "Buscar";
            this.btnBuscaPendente.UseVisualStyleBackColor = true;
            this.btnBuscaPendente.Click += new System.EventHandler(this.btnBuscaPendente_Click);
            // 
            // tbxOpDe
            // 
            this.tbxOpDe.Location = new System.Drawing.Point(51, 35);
            this.tbxOpDe.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.tbxOpDe.Name = "tbxOpDe";
            this.tbxOpDe.Size = new System.Drawing.Size(100, 20);
            this.tbxOpDe.TabIndex = 30;
            this.tbxOpDe.Click += new System.EventHandler(this.tbxOpDe_Click);
            this.tbxOpDe.Enter += new System.EventHandler(this.tbxOpDe_Enter);
            this.tbxOpDe.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxPendente_KeyDown);
            // 
            // btnImprimePendente
            // 
            this.btnImprimePendente.Location = new System.Drawing.Point(503, 8);
            this.btnImprimePendente.Name = "btnImprimePendente";
            this.btnImprimePendente.Size = new System.Drawing.Size(75, 36);
            this.btnImprimePendente.TabIndex = 70;
            this.btnImprimePendente.Text = "Imprimir";
            this.btnImprimePendente.UseVisualStyleBackColor = true;
            this.btnImprimePendente.Click += new System.EventHandler(this.btnImprimePendente_Click);
            // 
            // tbxDataAte
            // 
            this.tbxDataAte.Location = new System.Drawing.Point(202, 8);
            this.tbxDataAte.Mask = "00/00/0000";
            this.tbxDataAte.Name = "tbxDataAte";
            this.tbxDataAte.Size = new System.Drawing.Size(100, 20);
            this.tbxDataAte.TabIndex = 20;
            this.tbxDataAte.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.tbxDataAte.ValidatingType = typeof(System.DateTime);
            this.tbxDataAte.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxPendente_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(164, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "até";
            // 
            // tbxDataDe
            // 
            this.tbxDataDe.Location = new System.Drawing.Point(51, 8);
            this.tbxDataDe.Mask = "00/00/0000";
            this.tbxDataDe.Name = "tbxDataDe";
            this.tbxDataDe.Size = new System.Drawing.Size(100, 20);
            this.tbxDataDe.TabIndex = 10;
            this.tbxDataDe.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.tbxDataDe.ValidatingType = typeof(System.DateTime);
            this.tbxDataDe.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxPendente_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "OP";
            // 
            // cbxReimpressao
            // 
            this.cbxReimpressao.AutoSize = true;
            this.cbxReimpressao.Location = new System.Drawing.Point(308, 10);
            this.cbxReimpressao.Name = "cbxReimpressao";
            this.cbxReimpressao.Size = new System.Drawing.Size(87, 17);
            this.cbxReimpressao.TabIndex = 50;
            this.cbxReimpressao.Text = "Reimpressão";
            this.cbxReimpressao.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(164, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(22, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "até";
            // 
            // dgvPendente
            // 
            this.dgvPendente.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPendente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPendente.Location = new System.Drawing.Point(0, 0);
            this.dgvPendente.Name = "dgvPendente";
            this.dgvPendente.RowHeadersVisible = false;
            this.dgvPendente.Size = new System.Drawing.Size(836, 362);
            this.dgvPendente.TabIndex = 0;
            // 
            // tbpAvulsa
            // 
            this.tbpAvulsa.Controls.Add(this.tbxItemNameAvulsa);
            this.tbpAvulsa.Controls.Add(this.tbxQtdAvulsa);
            this.tbpAvulsa.Controls.Add(this.label11);
            this.tbpAvulsa.Controls.Add(this.btnBuscaAvulsa);
            this.tbpAvulsa.Controls.Add(this.btnImprimirAvulsa);
            this.tbpAvulsa.Controls.Add(this.label10);
            this.tbpAvulsa.Controls.Add(this.label9);
            this.tbpAvulsa.Controls.Add(this.tbxSerieAvulsa);
            this.tbpAvulsa.Controls.Add(this.tbxItemCodeAvulsa);
            this.tbpAvulsa.Location = new System.Drawing.Point(4, 22);
            this.tbpAvulsa.Name = "tbpAvulsa";
            this.tbpAvulsa.Padding = new System.Windows.Forms.Padding(3);
            this.tbpAvulsa.Size = new System.Drawing.Size(842, 429);
            this.tbpAvulsa.TabIndex = 1;
            this.tbpAvulsa.Text = "Avulsa";
            this.tbpAvulsa.UseVisualStyleBackColor = true;
            // 
            // tbxItemNameAvulsa
            // 
            this.tbxItemNameAvulsa.Enabled = false;
            this.tbxItemNameAvulsa.Location = new System.Drawing.Point(9, 89);
            this.tbxItemNameAvulsa.Name = "tbxItemNameAvulsa";
            this.tbxItemNameAvulsa.Size = new System.Drawing.Size(309, 20);
            this.tbxItemNameAvulsa.TabIndex = 27;
            // 
            // tbxQtdAvulsa
            // 
            this.tbxQtdAvulsa.Location = new System.Drawing.Point(70, 63);
            this.tbxQtdAvulsa.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.tbxQtdAvulsa.Name = "tbxQtdAvulsa";
            this.tbxQtdAvulsa.Size = new System.Drawing.Size(100, 20);
            this.tbxQtdAvulsa.TabIndex = 25;
            this.tbxQtdAvulsa.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tbxQtdAvulsa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxAvulsa_KeyDown);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 65);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(62, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Quantidade";
            // 
            // btnBuscaAvulsa
            // 
            this.btnBuscaAvulsa.Location = new System.Drawing.Point(243, 6);
            this.btnBuscaAvulsa.Name = "btnBuscaAvulsa";
            this.btnBuscaAvulsa.Size = new System.Drawing.Size(75, 36);
            this.btnBuscaAvulsa.TabIndex = 22;
            this.btnBuscaAvulsa.Text = "Buscar";
            this.btnBuscaAvulsa.UseVisualStyleBackColor = true;
            this.btnBuscaAvulsa.Click += new System.EventHandler(this.btnBuscaAvulsa_Click);
            // 
            // btnImprimirAvulsa
            // 
            this.btnImprimirAvulsa.Location = new System.Drawing.Point(243, 42);
            this.btnImprimirAvulsa.Name = "btnImprimirAvulsa";
            this.btnImprimirAvulsa.Size = new System.Drawing.Size(75, 36);
            this.btnImprimirAvulsa.TabIndex = 23;
            this.btnImprimirAvulsa.Text = "Imprimir";
            this.btnImprimirAvulsa.UseVisualStyleBackColor = true;
            this.btnImprimirAvulsa.Click += new System.EventHandler(this.btnImprimirAvulsa_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 37);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Série";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Item";
            // 
            // tbxSerieAvulsa
            // 
            this.tbxSerieAvulsa.Location = new System.Drawing.Point(70, 37);
            this.tbxSerieAvulsa.Name = "tbxSerieAvulsa";
            this.tbxSerieAvulsa.Size = new System.Drawing.Size(100, 20);
            this.tbxSerieAvulsa.TabIndex = 1;
            this.tbxSerieAvulsa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxAvulsa_KeyDown);
            // 
            // tbxItemCodeAvulsa
            // 
            this.tbxItemCodeAvulsa.Location = new System.Drawing.Point(70, 10);
            this.tbxItemCodeAvulsa.Name = "tbxItemCodeAvulsa";
            this.tbxItemCodeAvulsa.Size = new System.Drawing.Size(100, 20);
            this.tbxItemCodeAvulsa.TabIndex = 0;
            this.tbxItemCodeAvulsa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxAvulsa_KeyDown);
            // 
            // tbpOP
            // 
            this.tbpOP.Controls.Add(this.tbxQtdeOP);
            this.tbpOP.Controls.Add(this.label2);
            this.tbpOP.Controls.Add(this.tbxItemNameOP);
            this.tbpOP.Controls.Add(this.btnBuscaPorOP);
            this.tbpOP.Controls.Add(this.btnImprimePorOP);
            this.tbpOP.Controls.Add(this.label4);
            this.tbpOP.Controls.Add(this.tbxStatusOP);
            this.tbpOP.Controls.Add(this.tbOrdem);
            this.tbpOP.Location = new System.Drawing.Point(4, 22);
            this.tbpOP.Name = "tbpOP";
            this.tbpOP.Size = new System.Drawing.Size(842, 429);
            this.tbpOP.TabIndex = 2;
            this.tbpOP.Text = "OP";
            this.tbpOP.UseVisualStyleBackColor = true;
            // 
            // tbxQtdeOP
            // 
            this.tbxQtdeOP.Location = new System.Drawing.Point(52, 34);
            this.tbxQtdeOP.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.tbxQtdeOP.Name = "tbxQtdeOP";
            this.tbxQtdeOP.Size = new System.Drawing.Size(120, 20);
            this.tbxQtdeOP.TabIndex = 23;
            this.tbxQtdeOP.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "OP:";
            // 
            // tbxItemNameOP
            // 
            this.tbxItemNameOP.Enabled = false;
            this.tbxItemNameOP.Location = new System.Drawing.Point(3, 92);
            this.tbxItemNameOP.Name = "tbxItemNameOP";
            this.tbxItemNameOP.Size = new System.Drawing.Size(309, 20);
            this.tbxItemNameOP.TabIndex = 22;
            // 
            // btnBuscaPorOP
            // 
            this.btnBuscaPorOP.Location = new System.Drawing.Point(237, 8);
            this.btnBuscaPorOP.Name = "btnBuscaPorOP";
            this.btnBuscaPorOP.Size = new System.Drawing.Size(75, 36);
            this.btnBuscaPorOP.TabIndex = 11;
            this.btnBuscaPorOP.Text = "Buscar";
            this.btnBuscaPorOP.UseVisualStyleBackColor = true;
            this.btnBuscaPorOP.Click += new System.EventHandler(this.bBuscar_Click);
            // 
            // btnImprimePorOP
            // 
            this.btnImprimePorOP.Location = new System.Drawing.Point(237, 50);
            this.btnImprimePorOP.Name = "btnImprimePorOP";
            this.btnImprimePorOP.Size = new System.Drawing.Size(75, 36);
            this.btnImprimePorOP.TabIndex = 21;
            this.btnImprimePorOP.Text = "Imprimir";
            this.btnImprimePorOP.UseVisualStyleBackColor = true;
            this.btnImprimePorOP.Click += new System.EventHandler(this.bImprimir_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Quant:";
            // 
            // tbxStatusOP
            // 
            this.tbxStatusOP.BackColor = System.Drawing.Color.Gray;
            this.tbxStatusOP.Location = new System.Drawing.Point(3, 118);
            this.tbxStatusOP.Name = "tbxStatusOP";
            this.tbxStatusOP.ReadOnly = true;
            this.tbxStatusOP.Size = new System.Drawing.Size(309, 20);
            this.tbxStatusOP.TabIndex = 19;
            this.tbxStatusOP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbOrdem
            // 
            this.tbOrdem.Location = new System.Drawing.Point(52, 8);
            this.tbOrdem.Name = "tbOrdem";
            this.tbOrdem.Size = new System.Drawing.Size(120, 20);
            this.tbOrdem.TabIndex = 13;
            this.tbOrdem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbOrdem_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Impressora:";
            // 
            // cbxImpressora
            // 
            this.cbxImpressora.FormattingEnabled = true;
            this.cbxImpressora.Location = new System.Drawing.Point(76, 3);
            this.cbxImpressora.Name = "cbxImpressora";
            this.cbxImpressora.Size = new System.Drawing.Size(500, 21);
            this.cbxImpressora.TabIndex = 15;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cbxImpressora);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Size = new System.Drawing.Size(850, 489);
            this.splitContainer1.SplitterDistance = 455;
            this.splitContainer1.TabIndex = 18;
            // 
            // Impressor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 489);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Impressor";
            this.Text = "CVA - Impressor de Etiquetas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Impressor_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tbpPendente.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbxOpAte)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbxOpDe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendente)).EndInit();
            this.tbpAvulsa.ResumeLayout(false);
            this.tbpAvulsa.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbxQtdAvulsa)).EndInit();
            this.tbpOP.ResumeLayout(false);
            this.tbpOP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbxQtdeOP)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbpPendente;
        private System.Windows.Forms.TabPage tbpAvulsa;
        private System.Windows.Forms.TabPage tbpOP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxItemNameOP;
        private System.Windows.Forms.Button btnBuscaPorOP;
        private System.Windows.Forms.Button btnImprimePorOP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxStatusOP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbOrdem;
        private System.Windows.Forms.ComboBox cbxImpressora;
        private System.Windows.Forms.Button btnBuscaPendente;
        private System.Windows.Forms.DataGridView dgvPendente;
        private System.Windows.Forms.Button btnImprimePendente;
        private System.Windows.Forms.NumericUpDown tbxOpAte;
        private System.Windows.Forms.NumericUpDown tbxOpDe;
        private System.Windows.Forms.MaskedTextBox tbxDataAte;
        private System.Windows.Forms.MaskedTextBox tbxDataDe;
        private System.Windows.Forms.CheckBox cbxReimpressao;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown tbxQtdAvulsa;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnBuscaAvulsa;
        private System.Windows.Forms.Button btnImprimirAvulsa;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbxSerieAvulsa;
        private System.Windows.Forms.TextBox tbxItemCodeAvulsa;
        private System.Windows.Forms.NumericUpDown tbxQtdeOP;
        private System.Windows.Forms.TextBox tbxItemNameAvulsa;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

