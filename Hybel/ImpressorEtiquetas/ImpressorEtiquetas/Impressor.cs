using BLL;
using MODEL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace ImpressorEtiquetas
{
    public partial class Impressor : Form
    {
        #region Atributos
        public EtiquetaModel Etiqueta { get; set; }
        public ImpressorBLL _impressorBLL { get; set; }
        #endregion

        #region Construtor
        public Impressor()
        {
            this._impressorBLL = new ImpressorBLL();
            InitializeComponent();
            var printers = _impressorBLL.GetPrinters();
            foreach (var p in printers)
            {
                this.cbxImpressora.Items.AddRange(new object[] { p });
            }
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultPrinter"]))
            {
                this.cbxImpressora.Text = ConfigurationManager.AppSettings["DefaultPrinter"];
            }
            this.btnBuscaPendente.Focus();
        }
        #endregion

        #region OP
        private void BuscaOP()
        {
            try
            {
                if (String.IsNullOrEmpty(tbOrdem.Text))
                {
                    MessageBox.Show("Informe a ordem de produção.", "Atenção");
                    return;
                }
                else
                {
                    this.Etiqueta = _impressorBLL.Get_By_Order(tbOrdem.Text);
                    this.Etiqueta.Ordem = Convert.ToInt32(this.tbOrdem.Text);
                }
                this.tbxQtdeOP.Value = Etiqueta.Quantidade;
                this.tbxStatusOP.Text = Etiqueta.Status.ToString();
                this.tbxItemNameOP.Text = Etiqueta.NomeProduto.ToString();
                if (this.tbxStatusOP.Text.ToUpper() == "ETIQUETA IMPRESSA")
                    this.tbxStatusOP.BackColor = Color.Green;
                if (this.tbxStatusOP.Text == "ETIQUETA NÃO IMPRESSA")
                    this.tbxStatusOP.BackColor = Color.Red;
                this.btnImprimePorOP.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bBuscar_Click(object sender, EventArgs e)
        {
            this.BuscaOP();  
        }

        private void bImprimir_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                if (this.tbxStatusOP.Text.ToUpper() == "ETIQUETA IMPRESSA")
                {
                    DialogResult dialog = MessageBox.Show("As etiquetas desta ordem de produção já foram impressas. Deseja imprimir novamente?", "Atenção", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                        ImprimirOP();
                    else if (dialog == DialogResult.No)
                        return;
                }
                else
                {
                    ImprimirOP();
                }
            }
        }

        public bool ValidateFields()
        {
            if (this.Etiqueta == null)
            {
                MessageBox.Show("Não existem etiquetas para serem impressas.", "Atenção");
                return false;
            }

            if (this.cbxImpressora.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione uma impressora.", "Atenção");
                return false;
            }
            return true;
        }

        public void ImprimirOP()
        {
            bool success = true;

            success = _impressorBLL.Print_Padrao(this.Etiqueta.Ordem, cbxImpressora.Text, (int)tbxQtdeOP.Value);
            
            if (success)
            {
                MessageBox.Show("Etiquetas geradas com sucesso.", "Sucesso");

                _impressorBLL.UpdateStatus(tbOrdem.Text);
                tbOrdem.Text = "";
                tbxQtdeOP.Value = 1;
                tbxItemNameOP.Text = "";
                this.tbxStatusOP.Text = "";
                tbxStatusOP.BackColor = Color.Gray;

                this.tbOrdem.Focus();
                this.Etiqueta = null;
            }
        }
        #endregion

        #region Pendentes
        private void BuscaPendente()
        {
            OPFiltroModel filtroModel = new OPFiltroModel();
            if (!String.IsNullOrEmpty(this.tbxDataDe.Text))
            {
                filtroModel.DataDe = DateTime.ParseExact(this.tbxDataDe.Text, "ddMMyyyy", CultureInfo.CurrentCulture, DateTimeStyles.None);
            }
            if (!String.IsNullOrEmpty(this.tbxDataAte.Text))
            {
                filtroModel.DataAte = DateTime.ParseExact(this.tbxDataAte.Text, "ddMMyyyy", CultureInfo.CurrentCulture, DateTimeStyles.None);
            }
            filtroModel.OPDe = this.tbxOpDe.Value;
            filtroModel.OPAte = this.tbxOpAte.Value;
            filtroModel.Reimpressao = this.cbxReimpressao.Checked;

            this.dgvPendente.DataSource = new OrdemBLL().GetOPList(filtroModel);
            this.dgvPendente.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            this.dgvPendente.Columns["CardCode"].HeaderText = "Cód. Cliente";
            this.dgvPendente.Columns["CardName"].HeaderText = "Nome";
            this.dgvPendente.Columns["ItemCode"].HeaderText = "Cód. Item";
            this.dgvPendente.Columns["ItemName"].HeaderText = "Descrição";

            for (int i = 1; i < this.dgvPendente.ColumnCount; i++)
            {
                this.dgvPendente.Columns[i].ReadOnly = true;
            }
            this.btnImprimePendente.Focus();
        }

        private void btnImprimePendente_Click(object sender, EventArgs e)
        {
            List<OrdemProducaoModel> list = this.dgvPendente.DataSource as List<OrdemProducaoModel>;
            list = list.Where(l => l.Imprimir).OrderBy(i => i.OP).ToList();

            if (list.Count == 0)
            {
                MessageBox.Show("Nenhum ordem selecionada para impressão!");
                return;
            }

            bool success = true;

            foreach (var item in list)
            {
                success = _impressorBLL.Print_Padrao(item.OP, this.cbxImpressora.Text, null);
                if (!success)
                {
                    break;
                }
                else
                {
                    _impressorBLL.UpdateStatus(item.OP.ToString());
                }
            }

            if (success)
            {
                MessageBox.Show("Etiquetas geradas com sucesso.", "Sucesso");
                this.dgvPendente.DataSource = null;
            }
        }

        private void btnBuscaPendente_Click(object sender, EventArgs e)
        {
            this.BuscaPendente();
        }
        #endregion

        #region Avulsa
        private void BuscaAvulsa()
        {
            Etiqueta = new ItemBLL().GetEtiqueta(this.tbxItemCodeAvulsa.Text, this.tbxSerieAvulsa.Text);
            if (Etiqueta == null)
            {
                this.tbxItemCodeAvulsa.Focus();
                return;
            }
            else if (String.IsNullOrEmpty(Etiqueta.ItemCode))
            {
                MessageBox.Show("Dados não encontrados!");
                Etiqueta = null;
                this.tbxItemNameAvulsa.Text = String.Empty;
            }
            else
            {
                this.tbxItemCodeAvulsa.Text = Etiqueta.ItemCode;
                this.tbxItemNameAvulsa.Text = Etiqueta.ItemName;
                this.btnImprimirAvulsa.Focus();
            }
        }

        private void btnBuscaAvulsa_Click(object sender, EventArgs e)
        {
            this.BuscaAvulsa();
        }

        private void btnImprimirAvulsa_Click(object sender, EventArgs e)
        {
            if (this.Etiqueta == null)
            {
                MessageBox.Show("Não existem etiquetas para serem impressas.", "Atenção");
                return;
            }

            bool success = true;
            this.Etiqueta.Quantidade = tbxQtdAvulsa.Value;
            this.Etiqueta.Serie = this.tbxSerieAvulsa.Text;

            success = _impressorBLL.Print_Padrao(this.Etiqueta, this.cbxImpressora.Text, null);

            if (success)
            {
                MessageBox.Show("Etiquetas geradas com sucesso.", "Sucesso");
                if (!String.IsNullOrEmpty(this.tbxSerieAvulsa.Text))
                {
                    this.tbxSerieAvulsa.Focus();
                }
                else
                {
                    this.tbxItemCodeAvulsa.Focus();
                }
                tbxItemCodeAvulsa.Text = "";
                tbxSerieAvulsa.Text = "";
                tbxItemNameAvulsa.Text = "";
                tbxQtdAvulsa.Value = 1;

                this.Etiqueta = null;
            }
        }
        #endregion

        #region Impressor_FormClosing
        private void Impressor_FormClosing(object sender, FormClosingEventArgs e)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings["DefaultPrinter"].Value = this.cbxImpressora.Text;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
        #endregion

        private void tbxPendente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.BuscaPendente();
            }
        }

        private void tbxAvulsa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.BuscaAvulsa();
            }
        }

        private void tbOrdem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.BuscaOP();
            }
        }

        private void tbxOpDe_Enter(object sender, EventArgs e)
        {
            this.tbxOpDe.Select(0, this.tbxOpDe.Value.ToString().Length);
        }

        private void tbxOpAte_Enter(object sender, EventArgs e)
        {
            this.tbxOpAte.Select(0, this.tbxOpAte.Value.ToString().Length);
        }

        private void tbxOpDe_Click(object sender, EventArgs e)
        {
            this.tbxOpDe.Select(0, this.tbxOpDe.Value.ToString().Length);
        }

        private void tbxOpAte_Click(object sender, EventArgs e)
        {
            this.tbxOpAte.Select(0, this.tbxOpAte.Value.ToString().Length);
        }
    }
}
