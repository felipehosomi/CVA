using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.VIEW
{
    /// <summary>
    /// Menu - Form f142 - Tabela Doc. Frete - Remover Linha
    /// </summary>
    public class fM9142 : BaseFormParent
    {
        public fM9142(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                Form form = SBOApp.Application.Forms.ActiveForm;
                Grid gr_Docs = (Grid)form.Items.Item("gr_Docs").Specific;
                if (gr_Docs.GetCellFocus() != null)
                {
                    DataTable dt_Docs = form.DataSources.DataTables.Item("dt_Docs");
                    
                    dt_Docs.Rows.Remove(gr_Docs.GetCellFocus().rowIndex);
                }
            }

            return true;
        }
    }
}
