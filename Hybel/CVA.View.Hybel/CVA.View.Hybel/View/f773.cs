using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Colar
    /// </summary>
    public class f773 : BaseForm
    {
        private MenuEvent MenuItemEvent;

        public f773(MenuEvent menuEvent)
        {
            this.MenuItemEvent = menuEvent;
        }

        public override bool MenuEvent()
        {
            if (!MenuItemEvent.BeforeAction)
            {
                if (SBOApp.Application.Forms.ActiveForm.UniqueID.Contains("2000003038"))
                {
                    f2000003038.Changed = true;
                }
            }
            else
            {
                if (SBOApp.Application.Forms.ActiveForm.Type == 139 || SBOApp.Application.Forms.ActiveForm.Type == 149)
                {
                    return f139.BlockPasting(SBOApp.Application.Forms.ActiveForm);
                }
            }

            return true;
        }
    }
}
