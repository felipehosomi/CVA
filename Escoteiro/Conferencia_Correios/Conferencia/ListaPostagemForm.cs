using Conferencia;
using Conferencia.BLL;
using System;
using System.Collections;
using System.Windows.Forms;

namespace Conferencia
{
    public partial class ListaPostagemForm : Form
    {

        ListaPostagemBLL _ListaPostagemBLL = new ListaPostagemBLL();

        public ListaPostagemForm()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void CarregaComboBox()
        {
            cb_transp.DataSource = _ListaPostagemBLL.GetCombo();
            cb_transp.DisplayMember = "CardName";
            cb_transp.ValueMember   = "CardCode";

            cb_Desp.DataSource = _ListaPostagemBLL.GetComboDespacho();
            cb_Desp.DisplayMember = "CardName";
            cb_Desp.ValueMember = "CardName";

        }

        private void btn_Pesquisar_Click(object sender, EventArgs e)
        {
            string tipoDespacho = cb_Desp.SelectedValue.ToString();
            string transp = cb_transp.SelectedValue.ToString();
            string dtIni = Convert.ToDateTime(tx_dtInicial.Text).ToString("yyyy/MM/dd");
            string dtFinal = Convert.ToDateTime(tx_dtFinal.Text).ToString("yyyy/MM/dd");

            DataGridViewCheckBoxColumn ck = new DataGridViewCheckBoxColumn();
            ck.HeaderText = "#";
            dataGridView.Columns.Add(ck);

            dataGridView.DataSource = null;
            dataGridView.DataSource = _ListaPostagemBLL.GetInfo(tipoDespacho,transp,dtIni,dtFinal);

            dataGridView.Columns[1].ReadOnly = true;
            dataGridView.Columns[2].ReadOnly = true;
            dataGridView.Columns[2].HeaderText = "Data Faturamento";
            dataGridView.Columns[3].ReadOnly = true;


            dataGridView.Columns[0].Width = 20;
            dataGridView.Columns[1].Width = 350;
            dataGridView.Columns[2].Width = 118;

            if (tipoDespacho == "SEDEX" || tipoDespacho == "PAC")
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].Cells[0].Value = true;
                }
            }
            else
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    dataGridView.Rows[i].Cells[0].Value = false;
                    ck.ReadOnly = true;
                }
            }

        }

        private void ListaPostagemForm_Load(object sender, EventArgs e)
        {
            CarregaComboBox();
            cb_Desp.SelectedItem = "Selecione";
        }


        private void btn_ImpLista_Click_1(object sender, EventArgs e)
        {
            string tipoDespacho = cb_Desp.SelectedValue.ToString();
            string transp = cb_transp.SelectedValue.ToString();
            string dtIni = Convert.ToDateTime(tx_dtInicial.Text).ToString("dd/MM/yyy");
            string dtFinal = Convert.ToDateTime(tx_dtFinal.Text).ToString("dd/MM/yyy");
            string listaSerial = string.Empty;

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                var t = dataGridView.Rows[i].Cells[0].Value.ToString();
                if ( t == "True")
                {
                    if (listaSerial == "")
                    {
                        listaSerial = dataGridView.Rows[i].Cells[3].Value.ToString();
                    }
                    else
                    {
                        listaSerial += ",";

                        listaSerial += dataGridView.Rows[i].Cells[3].Value.ToString();
                    }
                    
                }
            }

            GerarRelatorio(transp, listaSerial, dtIni, dtFinal);

            
        }

        public void GerarRelatorio(string Transp, string Serial, string DataIni, string DataFinal)
        {
            //Hashtable reportParams = new Hashtable();
            //reportParams.Add("Transp", Transp);
            //reportParams.Add("Serial", Serial);

            //reportParams.Add("DataInicial", DataIni);
            //reportParams.Add("DataFinal", DataFinal);

            //FormRPT frm = new FormRPT();
            //frm.Show();

            
            //CrystalReport crRelatorio = new CrystalReport();
            //crRelatorio.ExecuteCrystalReport(@"C:\Users\CVAPOC09\Source\Escoteiro\Conferencia_Correios\Conferencia\Reports\CrystalReport1.rpt", reportParams);



            //ReportDocument cryRpt = new ReportDocument();
            //CrystalReportViewer crystalReportViewer1 = new CrystalReportViewer();

            //cryRpt.Load(@"C:\Users\CVAPOC09\Source\Escoteiro\Conferencia_Correios\Conferencia\Reports\CrystalReport1.rpt");

            //crystalReportViewer1.ReportSource = cryRpt;
            //crystalReportViewer1.Refresh();


        }
    }
}
