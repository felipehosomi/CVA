using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Ean13Barcode2005
{
	/// <summary>
	/// Summary description for Form1.
    /// // EAN-13 001234567890   Pais- 00 cod fabrica 12345  cod produto 67890 digito 5
    /// // codigo do Brsil 789
	/// </summary>
	partial class frmEan13 : Form
	{
        string folderTempImages = string.Empty;

        private Ean13 ean13 = null;

		public frmEan13( )
		{
			InitializeComponent( );
			cboScale.SelectedIndex = 2;
		}
        private void CreateEan13(
            string CodPais, string CodFabrica, string CodProduto, string Checksum
            )
        {
            ean13 = new Ean13();
            ean13.CountryCode = CodPais;
            ean13.ManufacturerCode = CodFabrica;
            ean13.ProductCode = CodProduto;
            if (Checksum.Length > 0)
                ean13.ChecksumDigit = Checksum;
            ean13.Scale = 2.0f;
            //ean13.Scale = .8f;

        }
        private void CreateEan13( )
		{
			ean13 = new Ean13( );
			ean13.CountryCode = txtCountryCode.Text;
			ean13.ManufacturerCode = txtManufacturerCode.Text;
			ean13.ProductCode = txtProductCode.Text;
			if( txtChecksumDigit.Text.Length > 0 )
				ean13.ChecksumDigit = txtChecksumDigit.Text;
		}

		private void butDraw_Click(object sender, EventArgs e)
		{
			System.Drawing.Graphics g = this.picBarcode.CreateGraphics( );

			g.FillRectangle( new System.Drawing.SolidBrush( System.Drawing.SystemColors.Control ),
				new Rectangle( 0, 0, picBarcode.Width, picBarcode.Height ) );

			CreateEan13( );
			ean13.Scale = (float)Convert.ToDecimal( cboScale.Items [cboScale.SelectedIndex] );
			ean13.DrawEan13Barcode( g, new System.Drawing.Point( 0, 0 ) );
			txtChecksumDigit.Text = ean13.ChecksumDigit;
			g.Dispose( );
		}

		private void butPrint_Click(object sender, EventArgs e)
		{
			System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument( );
			pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler( this.pd_PrintPage );
			pd.Print( );
		}

		private void pd_PrintPage( object sender, System.Drawing.Printing.PrintPageEventArgs ev )
		{
			CreateEan13( );
			ean13.Scale = ( float )Convert.ToDecimal( cboScale.Items [cboScale.SelectedIndex] );
			ean13.DrawEan13Barcode( ev.Graphics, new System.Drawing.Point( 0, 0 ) );
			txtChecksumDigit.Text = ean13.ChecksumDigit;

			// Add Code here to print other information.
			ev.Graphics.Dispose( );
		}

		private void butCreateBitmap_Click(object sender, EventArgs e)
		{
			CreateEan13( );
			ean13.Scale = ( float )Convert.ToDecimal( cboScale.Items [cboScale.SelectedIndex] );

			System.Drawing.Bitmap bmp = ean13.CreateBitmap( );
            bmp.Save(@"C:\Users\nms.joaob\Desktop\EAN13\1.bmp");
            this.picBarcode.Image = bmp;
		}

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(Ean13Barcode2005.Properties.Settings.Default.conexao);
            conn.Open();

            SqlCommand cmdDelete = new SqlCommand("delete from ImagesEAN13BarCode", conn);
            cmdDelete.ExecuteReader();
            conn.Close();

            conn.Open();
            SqlCommand command = new SqlCommand("select ItemCode, CodeBars from OITM  where len (CodeBars)=13 order by ItemCode ", conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                int numRows = dt.Rows.Count;

                progressBar1.Minimum = 0;
                progressBar1.Maximum = numRows;
                progressBar1.Value = 0;
            }

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string Itemcode = reader["ItemCode"].ToString();
                    string CodeBar = String.Format("{0}", reader["CodeBars"]);
                    ProcessaEtiquetaNoBancoDeDados(Itemcode, CodeBar);
                    progressBar1.Value = progressBar1.Value + 1;
                }
            }
            conn.Close();
            MessageBox.Show("Etiquetas Processadas com Sucesso!");
        }

        private void ProcessaEtiquetaNoBancoDeDados(string Itemcode, string CodeBar)
        {
            string CodPais = CodeBar.Substring(0, 2);
            string CodFabrica = CodeBar.Substring(2, 5);
            string CodProduto = CodeBar.Substring(7, 5);
            string Checksum = CodeBar.Substring(12, 1);

            CreateEan13(CodPais, CodFabrica, CodProduto, Checksum);

            System.Drawing.Bitmap bmp = ean13.CreateBitmap();

            this.picBarcode.Image = bmp;

            string fileBmp = string.Format(@"{0}\{1}.Jpeg", this.folderTempImages, Itemcode);
            bmp.Save(fileBmp);

            byte[] ImageInBytes = imageToByteArray(bmp);

            SqlConnection conn2 = new SqlConnection(Ean13Barcode2005.Properties.Settings.Default.conexao);
            conn2.Open();

            SqlCommand cmdInsert = new SqlCommand(
                string.Format("insert into ImagesEAN13BarCode (ItemCode,ImageEAN13,BarCode) values('{0}',@ImageEAN13,'{1}') ", Itemcode, CodeBar)
                , conn2);

            SqlParameter paramUserImage = new SqlParameter("ImageEAN13", SqlDbType.Binary, ImageInBytes.Length);
            paramUserImage.Value = ImageInBytes;

            cmdInsert.Parameters.Add(paramUserImage);

            cmdInsert.ExecuteNonQuery();
            conn2.Close();
            File.Delete(fileBmp);
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
                 
        private void frmEan13_Load(object sender, EventArgs e)
        {
            folderTempImages = Application.StartupPath + @"\TemImagens";
            if (!System.IO.Directory.Exists(folderTempImages))
            {
                System.IO.Directory.CreateDirectory(folderTempImages);
            }
            //
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(Ean13Barcode2005.Properties.Settings.Default.conexao);
            conn.Open();

            //SqlCommand cmdDelete = new SqlCommand("delete from ImagesEAN13BarCode", conn);
            //cmdDelete.ExecuteReader();
            //conn.Close();

            //conn.Open();
            SqlCommand command = new SqlCommand("select ItemCode, CodeBars from OITM  where len (CodeBars)=13 and ItemCode not in (select [ImagesEAN13BarCode].ItemCode from [ImagesEAN13BarCode] ) order by ItemCode ", conn);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                DataTable dt = new DataTable();
                dt.Load(reader);
                int numRows = dt.Rows.Count;

                progressBar1.Minimum = 0;
                progressBar1.Maximum = numRows;
                progressBar1.Value = 0;
            }

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string Itemcode = reader["ItemCode"].ToString();
                    string CodeBar = String.Format("{0}", reader["CodeBars"]);
                    ProcessaEtiquetaNoBancoDeDados(Itemcode, CodeBar);
                    progressBar1.Value = progressBar1.Value + 1;
                }
            }
            conn.Close();

            MessageBox.Show("Etiquetas Processadas com Sucesso!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBoxItemCode.Text.Length==0)
            {
                MessageBox.Show("Inorme o código do Item!");
            }
            else
            {
                SqlConnection conn = new SqlConnection(Ean13Barcode2005.Properties.Settings.Default.conexao);
                conn.Open();

                SqlCommand cmdDelete = new SqlCommand(string.Format( "delete from ImagesEAN13BarCode where ItemCode='{0}'", textBoxItemCode.Text), conn);
                cmdDelete.ExecuteReader();
                conn.Close();

                conn.Open();
                SqlCommand command = new SqlCommand(string.Format("select ItemCode, CodeBars from OITM  where len (CodeBars)=13 and ItemCode in ('{0}') order by ItemCode ", textBoxItemCode.Text), conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    int numRows = dt.Rows.Count;

                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = numRows;
                    progressBar1.Value = 0;
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string Itemcode = reader["ItemCode"].ToString();
                        string CodeBar = String.Format("{0}", reader["CodeBars"]);
                        ProcessaEtiquetaNoBancoDeDados(Itemcode, CodeBar);
                        progressBar1.Value = progressBar1.Value + 1;
                    }
                }
                conn.Close();

                MessageBox.Show("Etiquetas Processadas com Sucesso!");
            }
        }
    }
}

