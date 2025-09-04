namespace Ean13Barcode2005
{
	partial class frmEan13
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose( );
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEan13));
            this.butDraw = new System.Windows.Forms.Button();
            this.butPrint = new System.Windows.Forms.Button();
            this.txtManufacturerCode = new System.Windows.Forms.TextBox();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.txtChecksumDigit = new System.Windows.Forms.TextBox();
            this.cboScale = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.butCreateBitmap = new System.Windows.Forms.Button();
            this.txtCountryCode = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxItemCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picBarcode = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBarcode)).BeginInit();
            this.SuspendLayout();
            // 
            // butDraw
            // 
            this.butDraw.Location = new System.Drawing.Point(982, 81);
            this.butDraw.Name = "butDraw";
            this.butDraw.Size = new System.Drawing.Size(142, 23);
            this.butDraw.TabIndex = 0;
            this.butDraw.Text = "Gera Código de Barras";
            this.butDraw.Click += new System.EventHandler(this.butDraw_Click);
            // 
            // butPrint
            // 
            this.butPrint.Location = new System.Drawing.Point(982, 124);
            this.butPrint.Name = "butPrint";
            this.butPrint.Size = new System.Drawing.Size(142, 23);
            this.butPrint.TabIndex = 1;
            this.butPrint.Text = "Imprime Cod. Barras";
            this.butPrint.Click += new System.EventHandler(this.butPrint_Click);
            // 
            // txtManufacturerCode
            // 
            this.txtManufacturerCode.Location = new System.Drawing.Point(825, 129);
            this.txtManufacturerCode.Name = "txtManufacturerCode";
            this.txtManufacturerCode.Size = new System.Drawing.Size(121, 20);
            this.txtManufacturerCode.TabIndex = 3;
            this.txtManufacturerCode.Text = "34567";
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(825, 177);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(121, 20);
            this.txtProductCode.TabIndex = 4;
            this.txtProductCode.Text = "89012";
            // 
            // txtChecksumDigit
            // 
            this.txtChecksumDigit.Location = new System.Drawing.Point(825, 223);
            this.txtChecksumDigit.Name = "txtChecksumDigit";
            this.txtChecksumDigit.Size = new System.Drawing.Size(121, 20);
            this.txtChecksumDigit.TabIndex = 6;
            this.txtChecksumDigit.Text = "8";
            // 
            // cboScale
            // 
            this.cboScale.FormattingEnabled = true;
            this.cboScale.Items.AddRange(new object[] {
            "0,8",
            "0,9",
            "1,0",
            "1,1",
            "1,2",
            "1,3",
            "1,4",
            "1,5",
            "1,6",
            "1,7",
            "1,8",
            "1,9",
            "2,0"});
            this.cboScale.Location = new System.Drawing.Point(982, 222);
            this.cboScale.Name = "cboScale";
            this.cboScale.Size = new System.Drawing.Size(100, 21);
            this.cboScale.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(825, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Cod. País";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(825, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Cod. Fabrica";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(825, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Cod. Produto";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(825, 202);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 15);
            this.label4.TabIndex = 11;
            this.label4.Text = " Digito Checksum";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(982, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "Escala do Fator";
            // 
            // butCreateBitmap
            // 
            this.butCreateBitmap.Location = new System.Drawing.Point(982, 167);
            this.butCreateBitmap.Name = "butCreateBitmap";
            this.butCreateBitmap.Size = new System.Drawing.Size(100, 23);
            this.butCreateBitmap.TabIndex = 13;
            this.butCreateBitmap.Text = "Cria BitMap";
            this.butCreateBitmap.Click += new System.EventHandler(this.butCreateBitmap_Click);
            // 
            // txtCountryCode
            // 
            this.txtCountryCode.Location = new System.Drawing.Point(825, 81);
            this.txtCountryCode.Name = "txtCountryCode";
            this.txtCountryCode.Size = new System.Drawing.Size(121, 20);
            this.txtCountryCode.TabIndex = 14;
            this.txtCountryCode.Text = "12";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 42);
            this.button1.TabIndex = 15;
            this.button1.Text = "ReGerar Tudo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(22, 65);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(144, 50);
            this.button2.TabIndex = 16;
            this.button2.Text = "Gerar Faltantes";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(22, 135);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(144, 44);
            this.button3.TabIndex = 17;
            this.button3.Text = "Gerar Especifico";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxItemCode
            // 
            this.textBoxItemCode.Location = new System.Drawing.Point(27, 203);
            this.textBoxItemCode.Name = "textBoxItemCode";
            this.textBoxItemCode.Size = new System.Drawing.Size(100, 20);
            this.textBoxItemCode.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 182);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 15);
            this.label6.TabIndex = 19;
            this.label6.Text = "ItemCode";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(22, 250);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(250, 23);
            this.progressBar1.TabIndex = 20;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Ean13Barcode2005.Properties.Resources.alessiLogo;
            this.pictureBox2.Location = new System.Drawing.Point(288, 156);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(244, 163);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 22;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Ean13Barcode2005.Properties.Resources.cvasap__grande;
            this.pictureBox1.Location = new System.Drawing.Point(274, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(258, 115);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // picBarcode
            // 
            this.picBarcode.Location = new System.Drawing.Point(825, 263);
            this.picBarcode.Name = "picBarcode";
            this.picBarcode.Size = new System.Drawing.Size(350, 249);
            this.picBarcode.TabIndex = 2;
            this.picBarcode.TabStop = false;
            // 
            // frmEan13
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(544, 331);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxItemCode);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtCountryCode);
            this.Controls.Add(this.butCreateBitmap);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboScale);
            this.Controls.Add(this.txtChecksumDigit);
            this.Controls.Add(this.txtProductCode);
            this.Controls.Add(this.txtManufacturerCode);
            this.Controls.Add(this.picBarcode);
            this.Controls.Add(this.butPrint);
            this.Controls.Add(this.butDraw);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEan13";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EAN13 BarCode Generator by CVA Consultoria";
            this.Load += new System.EventHandler(this.frmEan13_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBarcode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button butDraw;
		private System.Windows.Forms.Button butPrint;
		private System.Windows.Forms.PictureBox picBarcode;
		private System.Windows.Forms.TextBox txtManufacturerCode;
		private System.Windows.Forms.TextBox txtProductCode;
		private System.Windows.Forms.TextBox txtChecksumDigit;
		private System.Windows.Forms.ComboBox cboScale;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button butCreateBitmap;
		private System.Windows.Forms.TextBox txtCountryCode;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxItemCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

