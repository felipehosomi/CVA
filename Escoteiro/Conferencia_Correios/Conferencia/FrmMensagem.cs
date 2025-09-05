using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Conferencia
{
    public partial class FrmMensagem : Form
    {
        public FrmMensagem()
        {
            InitializeComponent();
        }

        public FrmMensagem(string description)
        {
            InitializeComponent();
            lblMessage.Text = description;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Message_Load(object sender, EventArgs e)
        {
            SystemSounds.Exclamation.Play();
        }

    }

    public static class CustomMessageBox
    {
        public static void Show(string description)
        {
            // using construct ensures the resources are freed when form is closed
            using (var form = new FrmMensagem(description))
            {
                form.ShowDialog();
            }
        }
    }
}
