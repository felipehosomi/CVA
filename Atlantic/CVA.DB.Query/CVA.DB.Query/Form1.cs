using CVA.AddOn.Common.Controllers;
using CVA.AddOn.Common.DataBase;
using CVA.DB.Query.Model;
using CVA.DB.Query.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.DB.Query
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.txtServer.Text = "SERVERSAP";
            this.txtUserDB.Text = "sa";
            this.txtPasswDB.Text = "sa@#Atlantic";

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (ofdQuery.ShowDialog() == DialogResult.OK)
            {
                this.tbxQuery.Text = ofdQuery.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.tbxQuery.Text))
            {
                SqlController sql = new SqlController(this.txtServer.Text, "CVA_ATL_REP", this.txtUserDB.Text, this.txtPasswDB.Text);
                List<DatabaseModel> databaseList = sql.FillModelListAccordingToSql<DatabaseModel>(Resources.Database_Get);

                bool ok = true;
                foreach (var db in databaseList)
                {
                    try
                    {
                        StoredProcedureController sp = new StoredProcedureController(db.Server, db.Database, db.User, db.Password);
                        sp.CreateProcedureSQL(this.tbxQuery.Text);
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        MessageBox.Show(db.Database + ": " + ex.Message);
                    }
                }
                if (ok)
                {
                    MessageBox.Show("Comando executado com sucesso em todas as bases!");
                }
                else
                {
                    MessageBox.Show("Execução finalizada!");
                }
            }
            
        }
    }
}
