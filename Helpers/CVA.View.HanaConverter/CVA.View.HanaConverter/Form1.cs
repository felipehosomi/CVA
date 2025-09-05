using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Translator;

namespace CVA.View.HanaConverter
{
    public partial class Form1 : Form
    {
        public static TranslatorTool Translator { get; private set; }


        public Form1()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.tbxSQL.Text.Trim()))
            {
                this.tbxHana.Text = TranslateToHana(this.tbxSQL.Text.Trim());
            }
        }

        public static string TranslateToHana(string sql)
        {
            int count;
            int errCount;

            try
            {
                if (Translator == null)
                {
                    Translator = new TranslatorTool();
                }
                sql = Translator.TranslateQuery(sql, out count, out errCount);
                sql = sql.Substring(0, sql.Length - 3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return sql;
        }
    }
}
