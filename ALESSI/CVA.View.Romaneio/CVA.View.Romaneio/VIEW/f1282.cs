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
                if (SBOApp.Application.Forms.ActiveForm.UniqueID.Contains("2000002006"))
                {
                    Form form = SBOApp.Application.Forms.ActiveForm;
                    form.Freeze(true);
                    f2000002006.OnClickMenuAdd(form);
                    form.Freeze(false);
                }
            }

            return true;
        }
    }
}
