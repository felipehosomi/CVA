using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;

namespace CVA.AddOn.Hub.Logic.VIEW
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
                if (SBOApp.Application.Forms.ActiveForm.TypeEx.Contains("142"))
                {
                    Form form = SBOApp.Application.Forms.ActiveForm;
                    form.Freeze(true);
                    f142.OnClickMenuAdd(form);
                    form.Freeze(false);
                }
            }

            return true;
        }
    }
}
