using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using SAPbouiCOM;

namespace CVA.View.StockTransfer.VIEW
{
    class f1282 : BaseForm
    {
        private MenuEvent MenuItemEvent;

        public f1282(MenuEvent menuEvent)
        {
            this.MenuItemEvent = menuEvent;
        }

        public override bool MenuEvent()
        {
            if (MenuItemEvent.BeforeAction)
            {
                if (SBOApp.Application.Forms.ActiveForm.Type == 139)
                {
                    f139.Canceling = false;
                    f139.Closing = false;
                }
            }

            return true;
        }
    }
}
