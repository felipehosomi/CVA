using CVA.AddOn.Common;
using CVA.AddOn.Common.Forms;
using CVA.Hub.HELPER;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.AddOn.Hub.Logic.VIEW
{
    /// <summary>
    /// Menu cancelar
    /// </summary>
    public class f1284 : BaseForm
    {
        private MenuEvent MenuItemEvent;

        public f1284(MenuEvent menuEvent)
        {
            this.MenuItemEvent = menuEvent;
        }

        public override bool MenuEvent()
        {
            if (MenuItemEvent.BeforeAction)
            {
                if (B1Forms.IsMarketingDocument(SBOApp.Application.Forms.ActiveForm.Type.ToString()))
                {
                    DocumentBaseView.IsCanceling = true;
                }
                if (SBOApp.Application.Forms.ActiveForm.TypeEx.Contains("142"))
                {
                    Form form = SBOApp.Application.Forms.ActiveForm;
                    form.Items.Item("16").Click();
                    return f142.OnClickMenuCancel(form);
                }
            }

            return true;
        }
    }
}
