using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;

namespace CVA.View.Hybel.View
{
    /// <summary>
    /// Menu Incluir
    /// </summary>
    public class f1282 : BaseForm
    {
        private MenuEvent MenuItemEvent;

        public f1282(MenuEvent menuEvent)
        {
            this.MenuItemEvent = menuEvent;
        }

        public override bool MenuEvent()
        {
            if (!MenuItemEvent.BeforeAction)
            {
                if (SBOApp.Application.Forms.ActiveForm.UniqueID.Contains("2000003031"))
                {
                    Form form = SBOApp.Application.Forms.ActiveForm;
                    form.Freeze(true);
                    f2000003031.OnClickMenuAdd(form);
                    form.Freeze(false);
                }
            }

            return true;
        }
    }
}
