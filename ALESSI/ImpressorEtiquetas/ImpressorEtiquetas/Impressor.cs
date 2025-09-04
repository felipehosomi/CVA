using BLL;
using MODEL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImpressorEtiquetas
{
    public partial class Impressor : Form
    {
        #region Atributos
        public Etiqueta etiqueta { get; set; }
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
                this.cbImpressora.Items.AddRange(new object[] { p });
            }
        }
        #endregion


        private void bBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.etiqueta = new Etiqueta();

                if (String.IsNullOrEmpty(tbOrdem.Text))
                {
                    MessageBox.Show("Informe a ordem de produção.", "Atenção");
                    return;
                }

                else
                {
                    this.etiqueta = _impressorBLL.Get_By_Order(tbOrdem.Text);
                    this.etiqueta.Ordem = Convert.ToInt32(this.tbOrdem.Text);
                }
                this.tbQuantidade.Text = etiqueta.Quantidade.ToString();
                this.tbStatus.Text = etiqueta.Status.ToString();
                this.tbNomeProduto.Text = etiqueta.NomeProduto.ToString();
                if (this.tbStatus.Text == "ORDEM IMPRESSA")
                    this.tbStatus.BackColor = Color.Green;
                if (this.tbStatus.Text == "ORDEM NÃO IMPRESSA")
                    this.tbStatus.BackColor = Color.Red;
            }
            catch (Exception)
            {
                MessageBox.Show("Falha ao conectar no banco de dados. Verifique a conexão.", "Erro");
            }
        }

        private void bImprimir_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                if (this.tbStatus.Text == "ORDEM IMPRESSA")
                {
                    DialogResult dialog = MessageBox.Show("As etiquetas desta ordem de produção já foram impressas. Deseja imprimir novamente?", "Atenção", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                        Imprimir();
                    else if (dialog == DialogResult.No)
                        return;
                }
                else
                {
                    Imprimir();
                }
            }
        }

        public bool ValidateFields()
        {
            if (this.cbTipoEtiqueta.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione um tipo de etiqueta.", "Atenção");
                return false;
            }
            if (this.etiqueta == null)
            {
                MessageBox.Show("Não existem etiquetas para serem impressas.", "Atenção");
                return false;
            }
            if (String.IsNullOrEmpty(this.tbQuantidade.Text) || Convert.ToInt32(this.tbQuantidade.Text) <= 0)
            {
                MessageBox.Show("Quantidade de etiquetas a serem impressas é inválida.", "Atenção");
                return false;
            }

            if (this.cbImpressora.SelectedIndex == -1)
            {
                MessageBox.Show("Selecione uma impressora.", "Atenção");
                return false;
            }
            return true;
        }

        public void Imprimir()
        {
            var success = false;

            this.etiqueta.Quantidade = tbQuantidade.Text;
            this.etiqueta.Impressora = cbImpressora.Text;
            if (cbTipoEtiqueta.SelectedIndex == 0)
                success = _impressorBLL.Print_Padrao(this.etiqueta);
            if (cbTipoEtiqueta.SelectedIndex == 1)
                success = _impressorBLL.Print_PadraoIngles(this.etiqueta);
            if (cbTipoEtiqueta.SelectedIndex == 2)
                success = _impressorBLL.Print_Lote(this.etiqueta);
            if (cbTipoEtiqueta.SelectedIndex == 3)
                success = _impressorBLL.Print_Pacote(this.etiqueta);

            if (success)
            {
                MessageBox.Show("Etiquetas geradas com sucesso.", "Sucesso");

                _impressorBLL.UpdateStatus(tbOrdem.Text);
                tbOrdem.Text = "";
                tbQuantidade.Text = "";
                tbStatus.Text = "";
                tbStatus.BackColor = Color.Gray;
            }
            else
                MessageBox.Show("Falha ao gerar etiquetas.", "Erro");
        }
    }
}
