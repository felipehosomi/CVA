using CVA.Hybel.EtiquetaVerde.BLL;
using CVA.Hybel.EtiquetaVerde.MODEL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace CVA.Hybel.EtiquetaVerde.VIEW
{
    public partial class frmImpressor : Form
    {
        private static double LabelWidth;
        private static double LabelHeight;
        private static double LabelMarginTop;
        private static double LabelMarginBottom;
        private static double LabelMarginLeft;
        private static double LabelMarginRight;

        private static double InchesCalc = 2.54;

        #region Constructor
        public frmImpressor()
        {
            InitializeComponent();

            LabelWidth = (Convert.ToDouble(ConfigurationManager.AppSettings["Width"]) / InchesCalc) * 100;
            LabelHeight = (Convert.ToDouble(ConfigurationManager.AppSettings["Height"]) / InchesCalc) * 100;

            LabelMarginTop = (Convert.ToDouble(ConfigurationManager.AppSettings["MarginTop"]) / InchesCalc) * 100;
            LabelMarginBottom = (Convert.ToDouble(ConfigurationManager.AppSettings["MarginBottom"]) / InchesCalc) * 100;
            LabelMarginLeft = (Convert.ToDouble(ConfigurationManager.AppSettings["MarginLeft"]) / InchesCalc) * 100;
            LabelMarginRight = (Convert.ToDouble(ConfigurationManager.AppSettings["MarginRight"]) / InchesCalc) * 100;

            this.printDocument1.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument1_BeginPrint);
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            //this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            this.btnPageSetup.Click += new System.EventHandler(this.btnPageSetup_Click);

            var printers = PrinterBLL.GetPrinters();
            foreach (var p in printers)
            {
                this.cbxImpressora.Items.AddRange(new object[] { p });
            }
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultPrinter"]))
            {
                this.cbxImpressora.Text = ConfigurationManager.AppSettings["DefaultPrinter"];
            }
          
        }
        #endregion

        private void btnPrint_Click(object sender, System.EventArgs e)
        {
            if (String.IsNullOrEmpty(this.cbxImpressora.Text))
            {
                MessageBox.Show("Por favor, informe a impressora", "Impressora", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                List<ItemModel> itemList = this.dgvItem.DataSource as List<ItemModel>;
                if (itemList == null)
                {
                    MessageBox.Show("Nenhum item selecionado para impressão", "Itens", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                itemList = itemList.Where(i => i.Imprimir).ToList();
                if (itemList == null)
                {
                    MessageBox.Show("Nenhum item selecionado para impressão", "Itens", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                printDocument1.PrinterSettings.PrinterName = this.cbxImpressora.Text;

                PaperSize size = new PaperSize("Etiqueta Verde", (int)LabelWidth, (int)LabelHeight);
                printDocument1.DefaultPageSettings.PaperSize = size;
                printDocument1.DefaultPageSettings.Margins = new Margins((int)LabelMarginLeft, (int)LabelMarginRight, (int)LabelMarginTop, (int)LabelMarginBottom);

                foreach (var item in itemList)
                {
                    richTextBoxPrintCtrl1.Rtf = PrinterBLL.GetText(item);
                    for (int i = 0; i < item.QtdeEtiq; i++)
                    {
                        printDocument1.Print();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Print the content of RichTextBox. Store the last character printed.
            checkPrint = richTextBoxPrintCtrl1.Print(checkPrint, richTextBoxPrintCtrl1.TextLength, e);

            // Check for more pages
            if (checkPrint < richTextBoxPrintCtrl1.TextLength)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
        }

        #region Config Impressao - Não utilizado
        private int checkPrint;
        private void btnPageSetup_Click(object sender, System.EventArgs e)
        {
            pageSetupDialog1.ShowDialog();
        }

        private void btnPrintPreview_Click(object sender, System.EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            checkPrint = 0;
        }
        #endregion

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        private void tbxNF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Buscar();
            }
        }

        private void Buscar()
        {
            //            List<ItemModel> itemList = new List<ItemModel>();
            //            itemList.Add(new ItemModel()
            //            {
            //                Quantidade = 1,
            //                ItemCode = "33112064012",
            //                ItemName = "BOMBA ROCDEIRA LAVRALE - ALT. P/",
            //                NrNF = 94321,
            //                NrPedido = 10001,
            //                Transportadora = " ( 23336 ) EXPRESSO SÃO MIGUEL LTDA",
            //                Cliente = "ADILSON FERNANDES LEMOS - ME",
            //                Endereco = @"RUA ALBINO WOLF, 566
            //LAJEADO - RS - CEP 04325-000"
            //            });

            try
            {
                List<ItemModel> itemList = NotaFiscalBLL.GetList((int)this.tbxNF.Value);
                if (itemList == null || itemList.Count == 0)
                {
                    MessageBox.Show("Nota fiscal não encontrada!", "Nota Fiscal");
                }
                this.dgvItem.DataSource = itemList;

                this.dgvItem.Columns["Endereco"].Visible = false;
                this.dgvItem.Columns["Transportadora"].Visible = false;
                this.dgvItem.Columns["NrPedido"].Visible = false;

                for (int i = 1; i < this.dgvItem.ColumnCount - 1; i++)
                {
                    this.dgvItem.Columns[i].ReadOnly = true;
                    this.dgvItem.Columns[i].DefaultCellStyle.BackColor = Color.LightGray;
                }

                this.dgvItem.Columns["Imprimir"].HeaderText = "";
                this.dgvItem.Columns["ItemCode"].HeaderText = "Cód. Item";
                this.dgvItem.Columns["ItemName"].HeaderText = "Descrição";
                this.dgvItem.Columns["QtdeEtiq"].HeaderText = "Etiquetas";

                this.dgvItem.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                this.dgvItem.Columns["Imprimir"].Width = 50;
                this.dgvItem.Columns["Cliente"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dgvItem.Columns["ItemName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region frmImpressor_FormClosing
        private void frmImpressor_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Salva impressora selecionada como padrão
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings["DefaultPrinter"].Value = this.cbxImpressora.Text;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

        #endregion

        private void tbxNF_Enter(object sender, EventArgs e)
        {
            this.tbxNF.Select(0, this.tbxNF.Text.Length);
        }

        private void frmImpressor_Load(object sender, EventArgs e)
        {
            this.tbxNF.Focus();
            this.tbxNF.Select(0, this.tbxNF.Text.Length);
        }
    }
}
