using System;
using System.Linq;
using System.Windows.Forms;
using CVA_Rep_BLL;

namespace CVA_RepConfig.Forms.Objeto
{
    public partial class ConsultarObjeto_UserControl : UserControl
    {
        private readonly CVA_OBJ_BLL bll;

        public ConsultarObjeto_UserControl()
        {
            InitializeComponent();
            bll = new CVA_OBJ_BLL();
        }

        private void ConsultarObjeto_UserControl_Load(object sender, EventArgs e)
        {
            try
            {
                var lst = bll.GetAll().ToList();

                foreach (var bases in lst)
                {
                    dgvBases.Rows.Add(bases.ID, bases.INS, bases.UPD, bases.CVA_STU.STU, bases.OBJ, bases.DSCR);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}