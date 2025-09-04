using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.View.Romaneio.VIEW
{
    /// <summary>
    /// Menu - Form f2000002006 - Tabela Veículos - Remover Linha
    /// </summary>
    public class fR2102 : BaseFormParent
    {
        public fR2102(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                Form form = SBOApp.Application.Forms.ActiveForm;
                Grid gr_Veh = (Grid)form.Items.Item("gr_Veh").Specific;

                form.DataSources.DataTables.Item("dt_Veh").Rows.Remove(gr_Veh.GetCellFocus().rowIndex);

                if (form.Mode != BoFormMode.fm_UPDATE_MODE)
                {
                    form.Mode = BoFormMode.fm_UPDATE_MODE;
                }
            }

            return true;
        }
    }
}
