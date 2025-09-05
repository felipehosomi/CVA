using CVA.View.BancadaTeste.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.View.BancadaTeste
{
    public partial class frmBancadaTeste : Form
    {
        public frmBancadaTeste()
        {
            InitializeComponent();
            this.tbxPath.Text = ConfigurationManager.AppSettings["Path"];
            this.ofdPath.FileName = ConfigurationManager.AppSettings["Path"];
            this.tbxSerie.Focus();
            this.tbxSerie.Select();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.Generate();
        }

        private void tbxSerie_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void Generate()
        {
            //if (!String.IsNullOrEmpty(this.tbxSerie.Text.Trim()))
            //{
            //    string error = ItemBLL.GenerateFile(this.tbxPath.Text, this.tbxSerie.Text.Trim());
            //    this.lblError.Text = error;
            //    if (String.IsNullOrEmpty(error))
            //    {
            //        this.lblOK.Text = this.tbxSerie.Text + ": Arquvo gerado com sucesso!";
            //    }
            //    else
            //    {
            //        this.lblOK.Text = String.Empty;
            //    }
            //    this.tbxSerie.SelectAll();
            //}
        }

        private void tbxPath_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.tbxPath.Text))
            {
                this.SaveSettings();
            }
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if (this.ofdPath.ShowDialog() == DialogResult.OK)
            {
                this.tbxPath.Text = this.ofdPath.FileName;
                this.SaveSettings();
            }
        }

        private void tbxSerie_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                this.Generate();
            }
        }

        private void SaveSettings()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings["Path"].Value = this.tbxPath.Text;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}
