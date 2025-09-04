using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVA.Apetit.Addon
{
    public partial class FormInput : Form
    {
        public FormInput()
        {
            InitializeComponent();
        }

        //==================================================================================================================================//
        private void FormInput_Load(object sender, EventArgs e)
        //==================================================================================================================================//
        {
            try
            {
                //MessageBox.Show(grid.RowCount.ToString());

                if (grid.RowCount == 0)
                {
                   

                    grid.Columns.Clear();

                    // var col =                              Header,       Name, width, readOnly, visible, alinhamento, format, fonte.size, Sortable
                    var col01 = Class.Geral.retColTextBox("BPLId", "BPLId", 50, false, true, "direita", "#0", 0, false);
                    var col02 = Class.Geral.retColTextBox("Data", "Data", 75, false, true, "centro", "", 0, false);
                    var col03 = Class.Geral.retColTextBox("ItemCode", "ItemCode", 50, false, true, "", "", 0, false);
                    var col04 = Class.Geral.retColTextBox("Quant", "Quant", 50, false, true, "direita", "#0", 0, false);

                    grid.Columns.AddRange(new DataGridViewColumn[] { col01, col02, col03, col04 });
                    //grid.AllowUserToAddRows = false;
                    grid.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
                    grid.RowHeadersVisible = false;
                    grid.EditMode = DataGridViewEditMode.EditOnEnter;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }







    }
}
