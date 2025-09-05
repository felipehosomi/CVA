using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;
using System;

namespace CVA.View.Hybel.VIEW
{
    /// <summary>
    /// Menu - Form f2000003039 - Tabela Item - Remover Linha
    /// </summary>
    public class fM3939 : BaseFormParent
    {
        public fM3939(MenuEvent menuEvent)
        {
            this.menuEvent = menuEvent;
        }

        public override Boolean MenuEvent()
        {
            if (!menuEvent.BeforeAction)
            {
                Form form = SBOApp.Application.Forms.ActiveForm;
                if (form.UniqueID.Contains("2000003039"))
                {
                    Matrix mt_Item = (Matrix)form.Items.Item("mt_Item").Specific;
                    mt_Item.DeleteRow(mt_Item.GetNextSelectedRow());
                    mt_Item.FlushToDataSource();
                }
            }

            return true;
        }
    }
}
