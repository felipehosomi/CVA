using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;

namespace CVA.View.Romaneio.VIEW
{
    /// <summary>
    /// Menu - Form f2000002003 - Tabela NF - Remover Linha
    /// </summary>
    public class fR2101 : BaseFormParent
    {
        public fR2101(SAPbouiCOM.MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                Form form = SBOApp.Application.Forms.ActiveForm;
                
                Matrix mt_NF = (Matrix)form.Items.Item("mt_NF").Specific;
                mt_NF.DeleteRow(mt_NF.GetNextSelectedRow());
                if (form.Mode != BoFormMode.fm_UPDATE_MODE && form.Mode != BoFormMode.fm_ADD_MODE)
                {
                    form.Mode = BoFormMode.fm_UPDATE_MODE;
                }
                mt_NF.FlushToDataSource();
            }

            return true;
        }
    }
}
